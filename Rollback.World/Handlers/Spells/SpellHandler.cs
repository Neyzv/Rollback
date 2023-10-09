using Rollback.Protocol.Messages;
using Rollback.World.Game.Spells;
using Rollback.World.Network;
using Rollback.World.Network.Handler;

namespace Rollback.World.Handlers.Spells
{
    public static class SpellHandler
    {
        [WorldHandler(SpellMoveMessage.Id)]
        public static void HandleSpellMoveMessage(WorldClient client, SpellMoveMessage message) =>
            client.Account!.Character!.MoveSpellShortcut(message.spellId, message.position);

        [WorldHandler(SpellUpgradeRequestMessage.Id)]
        public static void HandleSpellUpgradeRequestMessage(WorldClient client, SpellUpgradeRequestMessage message)
        {
            if (client.Account!.Character!.Fighter is null)
                client.Account.Character.BoostSpell(message.spellId);
        }

        public static void SendSpellMovementMessage(WorldClient client, CharacterSpell spell) =>
            client.Send(new SpellMovementMessage(spell.Id, spell.Position));

        public static void SendSpellUpgradeSuccessMessage(WorldClient client, CharacterSpell spell) =>
            client.Send(new SpellUpgradeSuccessMessage(spell.Id, spell.Level));

        public static void SendSpellUpgradeFailureMessage(WorldClient client) =>
            client.Send(new SpellUpgradeFailureMessage());
    }
}
