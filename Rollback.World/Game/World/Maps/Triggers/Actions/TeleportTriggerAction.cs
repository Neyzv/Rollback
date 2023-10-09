using Rollback.Common.DesignPattern.Attributes;
using Rollback.Protocol.Enums;
using Rollback.World.Database.World;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.World.Maps.Triggers.Actions
{
    [Identifier("Teleport")]
    public sealed class TeleportTriggerAction : CellTrigger
    {
        private int? _mapId;
        public int? MapId =>
            _mapId ??= GetParameterValue<int>(0);

        private short? _cellId;
        public short? Cell =>
            _cellId ??= GetParameterValue<short>(1);

        private DirectionsEnum? _direction;
        public DirectionsEnum? Direction =>
            _direction ??= (DirectionsEnum?)GetParameterValue<sbyte?>(2);

        public TeleportTriggerAction(WorldCellsTriggersRecord record) : base(record) { }

        public override void Trigger(Character character)
        {
            if (MapId.HasValue)
                character.Teleport(MapId.Value, Cell, Direction);
            else
                character.ReplyError("Can not teleport with out a mapId...");
        }
    }
}
