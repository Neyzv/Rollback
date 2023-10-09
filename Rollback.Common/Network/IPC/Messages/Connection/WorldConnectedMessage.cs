using System.Diagnostics.CodeAnalysis;
using Rollback.Common.IO.Binary;

namespace Rollback.Common.Network.IPC.Messages.Connection
{
    public sealed record WorldConnectedMessage : IPCMessage
    {
        public const int Id = 1;
        public override uint MessageId =>
            Id;

        public short WorldId { get; set; }

        [NotNull]
        public string? IPAddress { get; set; }

        public ushort Port { get; set; }

        public WorldConnectedMessage() { }

        public WorldConnectedMessage(short worldId, string ipAddress, ushort port)
        {
            WorldId = worldId;
            IPAddress = ipAddress;
            Port = port;
        }

        protected override void InternalSerialize(BigEndianWriter writer)
        {
            writer.WriteShort(WorldId);
            writer.WriteString(IPAddress);
            writer.WriteUShort(Port);
        }

        protected override void InternalDeserialize(BigEndianReader reader)
        {
            WorldId = reader.ReadShort();
            IPAddress = reader.ReadString();
            Port = reader.ReadUShort();
        }
    }
}
