using Rollback.Common.Commands.Types;
using Rollback.Protocol.Enums;
using Rollback.World.Game.Items;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Commands.Commands
{
    public sealed class ItemCommand : CommandContainer
    {
        public override string[] Aliases =>
            new[] { "item", "items" };

        public override string Description =>
            "Command to manage items.";

        public override byte Role =>
            (byte)GameHierarchyEnum.ADMIN;
    }

    public sealed class ItemAddCommand : InGameSubCommand
    {
        public override Type ParentCommand =>
            typeof(ItemCommand);

        public override string[] Aliases =>
            new[] { "add" };

        public override string Description =>
            "Command to add an item to your character.";

        public override byte Role =>
            (byte)GameHierarchyEnum.ADMIN;

        public ItemAddCommand()
        {
            AddParameter("Id of the item.");
            AddParameter("Quantity to add.", defaultValue: 1);
            AddParameter("Character to target.", optional: true);
        }

        protected override void Execute(Character sender)
        {
            Character? targetedCharacter;

            if (GetParameterValue<string?>(2) is not null)
                targetedCharacter = GetParameterValue<Character>(2);
            else
                targetedCharacter = sender;

            if (targetedCharacter is not null && GetParameterValue<short?>(0) is { } itemId)
                targetedCharacter.Inventory.AddItem(itemId, GetParameterValue<int>(1));
            else
                sender.ReplyError($"Can not find the target...");
        }
    }

    public sealed class ItemAddTypeCommand : InGameSubCommand
    {
        public override Type ParentCommand =>
            typeof(ItemCommand);

        public override string[] Aliases =>
            new[] { "addtype" };

        public override string Description =>
            "Command to add an item to your character.";

        public override byte Role =>
            (byte)GameHierarchyEnum.ADMIN;

        public ItemAddTypeCommand()
        {
            AddParameter("Id of the type.");
            AddParameter("Name of the targeted character.", optional: true);
        }

        protected override void Execute(Character sender)
        {
            Character? targetedCharacter;

            if (GetParameterValue<string?>(1) is not null)
                targetedCharacter = GetParameterValue<Character>(1);
            else
                targetedCharacter = sender;

            var typeId = GetParameterValue<short>(0);

            if (targetedCharacter is not null)
                foreach (var item in ItemManager.Instance.GetTemplateRecords(x => (short)x.TypeId == typeId))
                    targetedCharacter.Inventory.AddItem(item.Id);
            else
                sender.ReplyError($"Can not find the target...");
        }
    }
}
