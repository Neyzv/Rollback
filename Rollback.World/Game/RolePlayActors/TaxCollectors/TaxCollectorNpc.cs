using Rollback.Protocol.Enums;
using Rollback.Protocol.Types;
using Rollback.World.Game.Fights.Results;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Game.World.Maps;

namespace Rollback.World.Game.RolePlayActors.TaxCollectors
{
    public sealed class TaxCollectorNpc : AutoMoveActor, ILooter
    {
        public TaxCollector TaxCollector { get; set; }

        public IFightResult Result { get; }

        public TaxCollectorNpc(int id, MapInstance mapInstance, Cell cell, TaxCollector taxCollector)
            : base(id, mapInstance, cell, DirectionsEnum.DIRECTION_SOUTH_EAST, taxCollector.Look)
        {
            TaxCollector = taxCollector;
            Result = IFightResult.CreateResult(this);
        }

        public override GameRolePlayActorInformations GameRolePlayActorInformations(Character character) =>
            new GameRolePlayTaxCollectorInformations(Id,
                Look.GetEntityLook(),
                EntityDispositionInformations,
                TaxCollector.FirstNameId,
                TaxCollector.LastNameId,
                TaxCollector.Guild.GuildInformations,
                TaxCollector.Guild.Level);
    }
}
