using Rollback.Common.Network.IPC.Messages.Characters;

namespace Rollback.World.Network.IPC.Handlers.Characters
{
    public static class IPCCharacterHandler
    {
        public static void SendCharacterAddedMessage(int accountId, int characterId) =>
            IPCService.Instance.Send(new CharacterAddedMessage(accountId, characterId));

        public static void SendCharacterDeletedMessage(int characterId) =>
            IPCService.Instance.Send(new CharacterDeletedMessage(characterId));
    }
}
