using Rollback.Protocol.Enums;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Handlers.Documents;

namespace Rollback.World.Commands.Commands
{
    internal class ReadBookCommand : InGameCommand
    {
        public override string[] Aliases =>
            new[] { "read", "book" };

        public override string Description =>
            "Read a document";

        public override byte Role =>
            (byte)GameHierarchyEnum.MODERATOR;

        public ReadBookCommand() =>
            AddParameter("The id of the document to read");

        protected override void Execute(Character sender) =>
            DocumentHandler.SendDocumentReadingBeginMessage(sender.Client, GetParameterValue<short>(0));
    }
}
