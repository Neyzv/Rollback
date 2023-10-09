using Rollback.Protocol.Types;
using Rollback.World.Game.RolePlayActors.TaxCollectors;
using Rollback.World.Game.Spells;
using Rollback.World.Game.Stats;
using Rollback.World.Game.World.Maps;
using Rollback.World.Handlers.Guilds;

namespace Rollback.World.Game.Fights.Fighters
{
    public sealed class TaxCollectorFighter : AIFighter
    {
        public TaxCollector TaxCollector { get; }

        public override short Level =>
            TaxCollector.Guild.Level;

        public TaxCollectorFightersInformation TaxCollectorFightersInformation =>
            new(TaxCollector.Id, Team!.GetFighters<CharacterFighter>().Select(x => x.Character.CharacterMinimalPlusLookInformations).ToArray(),
                Team.OpposedTeam.GetFighters<CharacterFighter>().Select(x => x.Character.CharacterMinimalPlusLookInformations).ToArray());

        public override FightTeamMemberInformations FightTeamMemberInformations =>
            new FightTeamMemberTaxCollectorInformations(Id, TaxCollector.FirstNameId, TaxCollector.LastNameId, TaxCollector.Guild.Level);

        public TaxCollectorFighter(int contextualId, TaxCollector taxCollector, Cell cell, Dictionary<short, Spell> spells)
            : base(contextualId, taxCollector.Look.Clone(), StatsData.CreateStats(taxCollector.Guild), spells, cell)
        {
            TaxCollector = taxCollector;

            QuitFight += OnTaxCollectorQuitFight;
        }

        private void OnTaxCollectorQuitFight()
        {
            var loose = Team!.Fight.Losers.FirstOrDefault()?.Team!.Side == Team!.Side;
            TaxCollector.Guild.Send(GuildHandler.SendTaxCollectorAttackedResultMessage, new object[] { loose, TaxCollector });

            if (loose)
                TaxCollector.Guild.RemoveTaxCollector(TaxCollector.Id);
            else
                TaxCollector.QuitFight();
        }

        public override GameFightFighterInformations GameFightFighterInformations(FightActor fighter) =>
            new GameFightTaxCollectorInformations(Id, Look.GetEntityLook(), EntityDispositionInformations(fighter),
                (sbyte)Team!.Side, Alive, GameFightMinimalStats, TaxCollector.FirstNameId, TaxCollector.LastNameId,
                Level);
    }
}
