using Rollback.Common.DesignPattern.Attributes;
using Rollback.Protocol.Enums;
using Rollback.World.Database.Interactives;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Interactives.Skills
{
    [Identifier(184)]
    public sealed class TeleportSkill : Skill
    {
        private int? _mapId;
        public int? MapId =>
            _mapId ??= GetParameterValue<int>(0);

        private short? _cellId;
        public short? CellId =>
            _cellId ??= GetParameterValue<short>(1);

        private DirectionsEnum? _direction;
        public DirectionsEnum? Direction =>
            _direction ??= (DirectionsEnum?)GetParameterValue<byte>(2);

        public TeleportSkill(InteractiveObject interactive, InteractiveSkillRecord record)
            : base(interactive, record) { }

        public override void Execute(Character character)
        {
            if (MapId.HasValue && CellId.HasValue)
                character.Teleport(MapId.Value, CellId, Direction);
            else
                character.ReplyError("Can not teleport your character, missing parameters mapId or CellId...");
        }
    }
}
