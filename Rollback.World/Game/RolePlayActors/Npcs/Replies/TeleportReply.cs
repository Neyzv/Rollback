using Rollback.Common.DesignPattern.Attributes;
using Rollback.Protocol.Enums;
using Rollback.World.Database.Npcs;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.RolePlayActors.Npcs.Replies
{
    [Identifier("Teleport")]
    public class TeleportReply : NpcReply
    {
        private int? _mapId;
        public int? MapId =>
            _mapId ??= GetParameterValue<int>(0);

        private short? _cellId;
        public short? CellId =>
            _cellId ??= GetParameterValue<short>(1);

        private DirectionsEnum? _direction;
        public DirectionsEnum? Direction =>
            _direction ??= (DirectionsEnum?)GetParameterValue<int>(2);

        public TeleportReply(NpcReplyRecord record) : base(record) { }

        public override bool Execute(Npc npc, Character character)
        {
            if (MapId.HasValue)
                character.Teleport(MapId.Value, CellId, Direction);
            else
                character.ReplyError("Can not teleport with out a mapId...");

            return true;
        }
    }
}
