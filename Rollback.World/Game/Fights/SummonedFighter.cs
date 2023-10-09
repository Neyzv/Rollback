using Rollback.Protocol.Enums;
using Rollback.Protocol.Types;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Looks;
using Rollback.World.Game.Spells;
using Rollback.World.Game.Stats;
using Rollback.World.Game.World.Maps;
using Rollback.World.Handlers.Fights;

namespace Rollback.World.Game.Fights
{
    public abstract class SummonedFighter : AIFighter
    {
        public FightActor Summoner { get; }

        public override GameFightMinimalStats GameFightMinimalStats =>
            new(Stats.Health.Actual,
                Stats.Health.ActualMax,
                Stats.AP.Total,
                Stats.MP.Total,
                Summoner.Id,
                (short)(Stats[Stat.NeutralElementResistPercent].Total + (Team!.Fight.IsPvP ? Stats[Stat.PvpNeutralElementResistPercent].Total : 0)),
                (short)(Stats[Stat.EarthElementResistPercent].Total + (Team!.Fight.IsPvP ? Stats[Stat.PvpEarthElementResistPercent].Total : 0)),
                (short)(Stats[Stat.WaterElementResistPercent].Total + (Team!.Fight.IsPvP ? Stats[Stat.PvpWaterElementResistPercent].Total : 0)),
                (short)(Stats[Stat.AirElementResistPercent].Total + (Team!.Fight.IsPvP ? Stats[Stat.PvpAirElementResistPercent].Total : 0)),
                (short)(Stats[Stat.FireElementResistPercent].Total + (Team!.Fight.IsPvP ? Stats[Stat.PvpFireElementResistPercent].Total : 0)),
                Stats[Stat.DodgeApLostProbability].Total,
                Stats[Stat.DodgeMpLostProbability].Total,
                (sbyte)Visibility);

        protected SummonedFighter(ActorLook look, StatsData stats, Dictionary<short, Spell> spells, Cell cell, DirectionsEnum direction, FightActor summoner)
            : base(summoner.Team!.Fight.GetFreeContextualId(), look, stats, spells, cell)
        {
            Summoner = summoner;
            Direction = direction;

            JoinFight += OnSummonJoinFight;
            Died += OnSummonDied;
        }

        protected virtual void OnSummonJoinFight() =>
            Team!.Fight.Send(FightHandler.SendGameActionFightSummonMessage, new object[] { this });

        private void OnSummonDied() =>
            Summoner.RemoveSummon(this);
    }
}
