using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;


namespace Rollback.Protocol.Messages
{
    public record SetUpdateMessage : Message
    {
        public short setId;
        public short[] setObjects;
        public ObjectEffect[] setEffects;

        public const int Id = 5503;
        public override uint MessageId
        {
            get { return Id; }
        }
        public SetUpdateMessage()
        {
        }
        public SetUpdateMessage(short setId, short[] setObjects, ObjectEffect[] setEffects)
        {
            this.setId = setId;
            this.setObjects = setObjects;
            this.setEffects = setEffects;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteShort(setId);
            writer.WriteUShort((ushort)setObjects.Length);
            foreach (var entry in setObjects)
            {
                writer.WriteShort(entry);
            }
            writer.WriteUShort((ushort)setEffects.Length);
            foreach (var entry in setEffects)
            {
                writer.WriteShort(entry.TypeId);
                entry.Serialize(writer);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            setId = reader.ReadShort();
            if (setId < 0)
                throw new Exception("Forbidden value on setId = " + setId + ", it doesn't respect the following condition : setId < 0");
            var limit = reader.ReadUShort();
            setObjects = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                setObjects[i] = reader.ReadShort();
            }
            limit = reader.ReadUShort();
            setEffects = new ObjectEffect[limit];
            for (int i = 0; i < limit; i++)
            {
                setEffects[i] = (ObjectEffect)ProtocolManager.Instance.GetType(reader.ReadUShort());
                setEffects[i].Deserialize(reader);
            }
        }
    }
}
