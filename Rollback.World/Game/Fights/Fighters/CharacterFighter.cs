using Rollback.Protocol.Enums;
using Rollback.Protocol.Types;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Fights.Types;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Game.Spells;
using Rollback.World.Game.World.Maps;
using Rollback.World.Handlers.Fights;

namespace Rollback.World.Game.Fights.Fighters
{
    public sealed class CharacterFighter : FightActor
    {
        private readonly int _hpBeforeFight;
        private (MapInstance, short)? _savePos;

        public Character Character { get; }

        public override int Id =>
            Character.Id;

        public override short Level =>
            Character.Level;

        private bool _ready;
        public override bool Ready =>
            _ready;

        public override bool CanMove =>
            Stats.MP.Total > 0;

        public override FightTeamMemberInformations FightTeamMemberInformations =>
            new FightTeamMemberCharacterInformations(Id, Character.Name, Level);

        public CharacterFighter(Character character)
            : base(character.Look.Clone(), character.Stats,
                  character.GetSpells().ToDictionary(x => x.Id, x => (Spell)x), character.Cell)
        {
            Character = character;
            _hpBeforeFight = Stats.Health.Actual;

            JoinFight += OnCharacterJoinFight;
            QuitFight += OnCharacterQuitFight;
        }

        public override GameFightFighterInformations GameFightFighterInformations(FightActor fighter) =>
            new GameFightCharacterInformations(Id,
                Look.GetEntityLook(),
                EntityDispositionInformations(fighter),
                (sbyte)Team!.Side,
                Alive,
                GameFightMinimalStats,
                Character.Name,
                Level,
                Character.ActorAlignmentInformations);

        private void OnCharacterJoinFight()
        {
            FightHandler.SendGameFightStartingMessage(Character.Client, Team!.Fight.Type);
            FightHandler.SendGameFightJoinMessage(Character.Client, Team.Fight);

            if (!Team.Fight.Started)
                FightHandler.SendGameFightPlacementPossiblePositionsMessage(Character.Client, Team);

            var fighters = Team!.Fight.GetAllFighters<FightActor>(x => x.Alive);
            foreach (var fighter in fighters)
                FightHandler.SendGameFightShowFighterMessage(Character.Client, fighter);

            FightHandler.SendGameEntitiesDispositionMessage(Character.Client, fighters);

            FightHandler.SendGameFightUpdateTeamMessage(Character.Client, Team);
        }

        private void OnCharacterQuitFight()
        {
            var isDuel = Team!.Fight is FightDuel;
            if (isDuel && _hpBeforeFight > Stats.Health.Actual)
                Stats.Health.Actual = _hpBeforeFight;
            else if ((isDuel || (Team.Fight.Winners.Count is not 0 && Team.Fight.Winners.First().Team!.Side == Team.Side)) && Stats.Health.Actual < Stats.Health.ActualMax / 2)
                Stats.Health.Actual = Stats.Health.ActualMax / 2;

            Character.QuitFight();

            if (!isDuel && Team.Fight.Losers.FirstOrDefault()?.Team!.Side == Team.Side)
                Character.TeleportToSpawnPoint(true);
            else if (_savePos.HasValue)
                Character.Teleport(_savePos.Value.Item1, _savePos.Value.Item2);
        }

        public bool CanKick(FightActor target) =>
            target is CharacterFighter && IsLeader && IsFriendlyWith(target);

        public void SetReady(bool isReady)
        {
            _ready = isReady;
            Team!.Fight.Send(FightHandler.SendGameFightHumanReadyStateMessage, new object[] { this });
            Team.Fight.CheckAllStatus();
        }

        public void SetReadyForNextTurn() =>
            Team!.Fight.Checker.ToggleReady(this);

        public override SpellCastResult CanCastSpell(Spell spell, Cell cell)
        {
            var result = base.CanCastSpell(spell, cell);

            if (result is not SpellCastResult.OK)
                switch (result)
                {
                    case SpellCastResult.NoLos:
                        // Impossible de lancer ce sort : un obstacle gène votre vue !
                        Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 174);
                        break;
                    case SpellCastResult.HasNotSpell:
                        // Impossible de lancer ce sort : vous ne le possédez pas !
                        Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 169);
                        break;
                    case SpellCastResult.NotEnoughAP:
                        // Impossible de lancer ce sort : Vous avez %1 PA disponible(s) et il vous en faut %2 pour ce sort !
                        Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 170, Stats[Stat.ActionPoints].Total, spell.APCost);
                        break;
                    case SpellCastResult.UnwalkableCell:
                        // Impossible de lancer ce sort : la cellule visée n'est pas disponible !
                        Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 172);
                        break;
                    case SpellCastResult.CellNotFree:
                        // Impossible de lancer ce sort : la cellule visée n'est pas valide !
                        Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 193);
                        break;
                    case SpellCastResult.NotInZone:
                        // Impossible de lancer ce sort : Vous avez une portée de %1 à %2 et vous visez à %3 !
                        Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 171, spell.MinRange, (spell.RangeCanBeBoosted && Stats[Stat.Range].Total > 0 ? Stats[Stat.Range].Total : 0) + spell.MaxRange, cell.Point.ManhattanDistanceTo(Cell.Point));
                        break;
                    case SpellCastResult.NotInline:
                        // Impossible de lancer ce sort autrement qu\'en ligne droite !
                        Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 173);
                        break;
                    default:
                        // Impossible de lancer ce sort actuellement.
                        Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 175);
                        break;
                }

            return result;
        }

        public void SetSaveMap(MapInstance map, short cellId) =>
            _savePos = (map, cellId);
    }
}
