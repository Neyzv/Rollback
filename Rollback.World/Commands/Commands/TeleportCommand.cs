using Rollback.Protocol.Enums;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Game.World;

namespace Rollback.World.Commands.Commands
{
    public sealed class TeleportCommand : InGameCommand
    {
        public override string[] Aliases =>
            new[] { "tp", "teleport", "go" };

        public override string Description =>
            "Teleport your character to a map";

        public override byte Role =>
            (byte)GameHierarchyEnum.ADMIN;

        public TeleportCommand()
        {
            AddParameter("Id of the map.");
            AddParameter("Name of the targeted character.", optional: true);
            AddParameter("Id of the destination cell.", optional: true);
        }

        protected override void Execute(Character sender)
        {
            var mapId = GetParameterValue<int>(0);

            var map = WorldManager.Instance.GetMapById(mapId);

            if (map is not null)
            {
                Character? targetedCharacter;

                if (GetParameterValue<string?>(1) is not null)
                    targetedCharacter = GetParameterValue<Character>(1);
                else
                    targetedCharacter = sender;

                if (targetedCharacter is not null)
                {
                    targetedCharacter.Teleport(map, GetParameterValue<short?>(2));
                    sender.Reply($"{targetedCharacter.Name} have been teleported to map {mapId} !");
                }
                else
                    sender.ReplyError($"Can not find the target...");
            }
            else
                sender.ReplyError($"Unknown map with id {mapId}...");
        }
    }
}
