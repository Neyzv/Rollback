using Rollback.Protocol.Messages;
using Rollback.World.Network;

namespace Rollback.World.Handlers.Documents
{
    public static class DocumentHandler
    {
        public static void SendDocumentReadingBeginMessage(WorldClient client, short documentId) =>
            client.Send(new DocumentReadingBeginMessage(documentId));
    }
}
