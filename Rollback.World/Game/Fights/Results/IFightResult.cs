using Rollback.Protocol.Enums;
using Rollback.Protocol.Types;
using Rollback.World.Game.Fights.Fighters;
using Rollback.World.Game.Fights.Results.Types;
using Rollback.World.Game.RolePlayActors.TaxCollectors;

namespace Rollback.World.Game.Fights.Results
{
    public interface IFightResult
    {
        bool CanDrop { get; }

        int Kamas { get; }

        short Wisdom { get; }

        short Prospecting { get; }

        public void AddEarnedKamas(int amount);

        public void AddEarnedItem(short itemId, short quantity);

        void Apply();

        FightResultFighterListEntry GetResult(FightOutcomeEnum outcome);

        public static IFightResult CreateResult(ILooter looter) =>
            looter switch
            {
                CharacterFighter characterFighter => new CharacterResult(characterFighter),
                TaxCollectorFighter taxCollectorFighter => new TaxCollectorResult(taxCollectorFighter),
                FightActor fighter => new FighterResult(fighter),
                TaxCollectorNpc taxCollector => new TaxCollectorProspectingResult(taxCollector),
                _ => throw new Exception($"Unhandled {looter.GetType().Name}'s result...")
            };
    }
}
