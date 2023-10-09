using Rollback.Common.DesignPattern.Attributes;
using Rollback.Protocol.Enums;
using Rollback.World.Database.Npcs;
using Rollback.World.Game.Items.Types;
using Rollback.World.Game.Items.Types.Custom;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.RolePlayActors.Npcs.Replies
{
    [Identifier("Keyring")]
    public sealed class KeyringReply : TeleportReply
    {
        private short? _keyId;
        public short KeyId =>
            _keyId ??= GetParameterValue<short>(3);

        public KeyringReply(NpcReplyRecord record)
            : base(record) { }

        private static IEnumerable<PlayerItem> GetKeyringItems(Character character) =>
            character.Inventory.GetItems(x => x.Id == KeyringItem.KeyringId);

        public override bool CanExecute(Character character) =>
            base.CanExecute(character) && GetKeyringItems(character).Any(x => x is KeyringItem keyringItem && keyringItem.CanUseKey(KeyId));

        public override bool Execute(Npc npc, Character character)
        {
            var done = false;
            foreach (var keyring in GetKeyringItems(character))
                if (keyring is KeyringItem keyringItem && (done = keyringItem.TryUseKey(KeyId)))
                    break;

            if (done)
            {
                character.Inventory.Refresh();
                base.Execute(npc, character);
            }
            else
                // Certaines conditions ne sont pas satisfaites
                character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 1);

            return true;
        }
    }
}
