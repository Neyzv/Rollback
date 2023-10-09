using Rollback.Common.Commands.Types;
using Rollback.Common.Extensions;
using Rollback.Protocol.Enums;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Game.World.Maps;
using Rollback.World.Handlers.Basics;

namespace Rollback.World.Commands.Commands
{
    public sealed class InteractiveCommand : CommandContainer
    {
        public override string[] Aliases =>
            new[] { "interactive", "interactives" };

        public override string Description =>
            "Command to manage interactives.";

        public override byte Role =>
            (byte)GameHierarchyEnum.ADMIN;
    }

    public sealed class InteractiveInfoCommand : InGameSubCommand
    {
        public override Type ParentCommand =>
            typeof(InteractiveCommand);

        public override string[] Aliases =>
            new[] { "info", "infos", "i" };

        public override string Description =>
            "Command to show basic interactive's informations";

        public override byte Role =>
            (byte)GameHierarchyEnum.ADMIN;

        protected override void Execute(Character sender)
        {
            sender.ChangeMap -= OnMapChanged;
            sender.ChangeMap += OnMapChanged;

            var interactives = sender.MapInstance.GetInteractives();
            for (var i = 0; i < interactives.Length; i++)
            {
                BasicHandler.SendDebugHighlightCellsMessage(sender.Client, ColorExtensions.ColorValues[i % ColorExtensions.ColorValues.Length].ToArgb(), new[] { interactives[i].Cell.Id });
                sender.SendServerMessage($"Id : {interactives[i].Id} | ElementId : {interactives[i].ElementId} | CellId : {interactives[i].Cell.Id} | Animated: {interactives[i].Animated} | State : {interactives[i].State}",
                    ColorExtensions.ColorValues[i % ColorExtensions.ColorValues.Length]);
            }
        }

        private void OnMapChanged(Character character, MapInstance map)
        {
            BasicHandler.SendDebugClearHighlightCellsMessage(character.Client);
            character.ChangeMap -= OnMapChanged;
        }
    }
}
