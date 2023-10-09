using Rollback.Common.Commands.Types;
using Rollback.Common.Extensions;
using Rollback.Protocol.Enums;
using Rollback.World.Game.Interactions;
using Rollback.World.Game.Interactions.Dialogs.Npcs;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Game.RolePlayActors.Npcs;
using Rollback.World.Game.World.Maps;
using Rollback.World.Handlers.Basics;

namespace Rollback.World.Commands.Commands
{
    public sealed class NpcCommand : CommandContainer
    {
        public override string[] Aliases =>
            new[] { "npc", "npcs" };

        public override string Description =>
            "Command to manage npcs.";

        public override byte Role =>
            (byte)GameHierarchyEnum.ADMIN;
    }

    public sealed class NpcSpawnCommand : InGameSubCommand
    {
        public override Type ParentCommand =>
            typeof(NpcCommand);

        public override string[] Aliases =>
            new[] { "spawn", "s" };

        public override string Description =>
            "Command to spawn an npc.";

        public override byte Role =>
            (byte)GameHierarchyEnum.ADMIN;

        public NpcSpawnCommand()
        {
            AddParameter("Id of the npc.");
            AddParameter("Id of the cell.", optional: true);
            AddParameter("Id of the direction.", optional: true);
        }

        protected override void Execute(Character sender)
        {
            var npcRecord = NpcManager.Instance.GetNpcRecordById(GetParameterValue<short>(0));

            if (npcRecord is not null)
            {
                var cellId = GetParameterValue<short?>(1);
                cellId ??= sender.Cell.Id;

                var direction = (DirectionsEnum?)GetParameterValue<byte?>(2);
                direction ??= sender.Direction;

                sender.MapInstance?.AddNpc(npcRecord, cellId.Value, direction.Value);
            }
            else
                sender.ReplyError($"Can not find this npc...");
        }
    }

    public sealed class NpcDialogCommand : InGameSubCommand
    {
        public override Type ParentCommand =>
            typeof(NpcCommand);

        public override string[] Aliases =>
            new[] { "dialog", "d" };

        public override string Description =>
            "Command to create a dialog with a temporary npc.";

        public override byte Role =>
            (byte)GameHierarchyEnum.ADMIN;

        public NpcDialogCommand()
        {
            AddParameter("Id of the npc.");
            AddParameter("Id of the message.");
            AddParameter("Ids of the replies (ex : Id1,Id2,...,Idn).", optional: true);
        }

        protected override void Execute(Character sender)
        {
            var npcRecord = NpcManager.Instance.GetNpcRecordById(GetParameterValue<short>(0));
            if (npcRecord is not null)
            {
                var npc = sender.MapInstance.AddNpc(npcRecord, sender.Cell.Id, sender.Direction);

                var dialog = new NpcDialog(sender, npc);
                sender.LeaveInteraction += OnDialogClosed;

                dialog.Open();

                var replyIds = Array.Empty<short>();

                var strReplies = GetParameterValue<string?>(2);
                if (strReplies is not null)
                {
                    try
                    {
                        replyIds = strReplies.Split(',').Select(x => short.Parse(x)).ToArray();
                    }
                    catch
                    {
                        dialog.Close();
                        sender.ReplyError("Can not parse reply ids...");
                    }
                }

                dialog.SetCustomDialog(GetParameterValue<short>(1), replyIds);
            }
            else
                sender.ReplyError($"Can not find this npc...");
        }

        private void OnDialogClosed(Character character, IInteraction interaction)
        {
            if (interaction is NpcDialog dialog)
                character.MapInstance.RemoveActor(dialog.Dialoger);
        }
    }

    public sealed class NpcInfoCommand : InGameSubCommand
    {
        public override Type ParentCommand =>
            typeof(NpcCommand);

        public override string[] Aliases =>
            new[] { "infos", "info", "i" };

        public override string Description =>
            "Command to show basic npc's informations.";

        public override byte Role =>
            (byte)GameHierarchyEnum.ADMIN;

        protected override void Execute(Character sender)
        {
            sender.ChangeMap -= OnMapChanged;
            sender.ChangeMap += OnMapChanged;

            var npcs = sender.MapInstance.GetActors<Npc>();
            for (var i = 0; i < npcs.Length; i++)
            {
                BasicHandler.SendDebugHighlightCellsMessage(sender.Client, ColorExtensions.ColorValues[i % ColorExtensions.ColorValues.Length].ToArgb(), new[] { npcs[i].Cell.Id });
                sender.SendServerMessage($"Template Id : {npcs[i].Record.Id} | CellId : {npcs[i].Cell.Id} | Direction : {(byte)npcs[i].Direction!}", ColorExtensions.ColorValues[i % ColorExtensions.ColorValues.Length]);
            }
        }

        private void OnMapChanged(Character character, MapInstance map)
        {
            BasicHandler.SendDebugClearHighlightCellsMessage(character.Client);
            character.ChangeMap -= OnMapChanged;
        }
    }
}
