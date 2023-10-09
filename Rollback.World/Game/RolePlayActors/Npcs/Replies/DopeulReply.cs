using Rollback.Common.DesignPattern.Attributes;
using Rollback.Protocol.Enums;
using Rollback.World.Database.Npcs;
using Rollback.World.Game.Fights;
using Rollback.World.Game.Fights.Fighters;
using Rollback.World.Game.Interactions;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Game.RolePlayActors.Monsters;

namespace Rollback.World.Game.RolePlayActors.Npcs.Replies
{
    [Identifier("Dopeul")]
    public sealed class DopeulReply : NpcReply
    {
        private static readonly Dictionary<BreedEnum, (short, short)> _dopeulInfos = new()
        {
            [BreedEnum.Feca] = (160, 10293),
            [BreedEnum.Osamodas] = (161, 10295),
            [BreedEnum.Enutrof] = (162, 10292),
            [BreedEnum.Sram] = (163, 10299),
            [BreedEnum.Xelor] = (164, 10300),
            [BreedEnum.Ecaflip] = (165, 10290),
            [BreedEnum.Eniripsa] = (166, 10291),
            [BreedEnum.Iop] = (167, 10294),
            [BreedEnum.Cra] = (168, 10289),
            [BreedEnum.Sadida] = (169, 10298),
            [BreedEnum.Sacrieur] = (455, 10297),
            [BreedEnum.Pandawa] = (2691, 10296),
        };

        private BreedEnum? _breed;
        public BreedEnum Breed =>
            _breed ??= (BreedEnum)GetParameterValue<sbyte>(0);

        public DopeulReply(NpcReplyRecord record)
            : base(record) { }

        public override bool Execute(Npc npc, Character character)
        {
            character.LeaveInteraction += OnLeaveInteraction;

            return true;
        }

        private void OnLeaveInteraction(Character character, IInteraction interaction)
        {
            character.LeaveInteraction -= OnLeaveInteraction;

            if (_dopeulInfos.TryGetValue(Breed, out var dopeulInfos))
            {
                if (MonsterManager.Instance.GetMonsterRecordById(dopeulInfos.Item1) is { } dopeulRecord)
                {
                    var fight = FightManager.CreatePvM(character.MapInstance, character,
                            new[] { new Monster(dopeulRecord, (sbyte)Math.Ceiling(character.Level / 20d)) }, false);

                    if (fight is not null)
                    {
                        fight.WinnersDeterminated += OnWinnersDeterminated;
                        fight.StartPlacement();
                    }
                }
                else
                    character.ReplyError($"Can not find dopeul monster record {dopeulInfos.Item1} for breed {Breed}...");
            }
            else
                character.ReplyError($"Can not find dopeuls informations of breed {Breed}...");

        }

        private void OnWinnersDeterminated(IFight fight)
        {
            fight.WinnersDeterminated -= OnWinnersDeterminated;

            if (_dopeulInfos.TryGetValue(Breed, out var dopeulInfos) && fight.Losers.Any(x => x is MonsterFighter))
                foreach (var winner in fight.Winners)
                    winner.Result.AddEarnedItem(dopeulInfos.Item2, 1);
        }
    }
}
