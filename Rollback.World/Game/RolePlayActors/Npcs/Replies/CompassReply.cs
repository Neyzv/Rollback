using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.Database.Npcs;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Handlers.Compass;

namespace Rollback.World.Game.RolePlayActors.Npcs.Replies
{
    [Identifier("Compass")]
    public sealed class CompassReply : NpcReply
    {
        private sbyte? _x;
        public sbyte X =>
            _x ??= GetParameterValue<sbyte>(0);

        private sbyte? _y;
        public sbyte Y =>
            _y ??= GetParameterValue<sbyte>(1);

        public CompassReply(NpcReplyRecord record)
            : base(record) { }

        public override bool Execute(Npc npc, Character character)
        {
            CompassHandler.SendCompassUpdateMessage(character.Client, X, Y);

            return true;
        }
    }
}
