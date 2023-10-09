using Rollback.Common.IO.Binary;

namespace Rollback.Common.Network.IPC.Messages.World
{
    public sealed record WorldStateUpdateMessage : IPCMessage
    {
        public const int Id = 2;
        public override uint MessageId =>
            Id;

        public sbyte State { get; set; }

        public WorldStateUpdateMessage() { }

        public WorldStateUpdateMessage(sbyte state) =>
            State = state;

        protected override void InternalSerialize(BigEndianWriter writer) =>
            writer.WriteSByte(State);

        protected override void InternalDeserialize(BigEndianReader reader) =>
            State = reader.ReadSByte();
    }
}
