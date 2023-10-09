using Rollback.Auth.Database;
using Rollback.Common.Network.IPC.Handler;
using Rollback.Common.Network.IPC.Messages.Characters;
using Rollback.Common.ORM;

namespace Rollback.Auth.Network.IPC.Handlers.Characters
{
    public static class IPCCharacterHandler
    {
        [IPCHandler(CharacterAddedMessage.Id)]
        public static void HandleCharacterAddedMessage(IPCReceiver client, CharacterAddedMessage message) =>
            DatabaseAccessor.Instance.Insert(new WorldCharacterRecord()
            {
                AccountId = message.AccountId,
                CharacterId = message.CharacterId,
                WorldId = client.World!.Record.Id
            });

        [IPCHandler(CharacterDeletedMessage.Id)]
        public static void HandleCharacterDeletedMessage(IPCReceiver client, CharacterDeletedMessage message) =>
            DatabaseAccessor.Instance.ExecuteNonQuery(string.Format(WorldCharacterRelator.DeleteWorldCharacterByWorldAndCharacterId,
                client.World!.Record.Id, message.CharacterId));
    }
}
