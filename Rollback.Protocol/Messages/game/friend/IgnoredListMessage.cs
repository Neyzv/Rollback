using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record IgnoredListMessage : Message
    {
        public IgnoredInformations[] ignoredList;

        public const int Id = 5674;
        public override uint MessageId
        {
            get { return Id; }
        }
        public IgnoredListMessage()
        {
        }
        public IgnoredListMessage(IgnoredInformations[] ignoredList)
        {
            this.ignoredList = ignoredList;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUShort((ushort)ignoredList.Length);
            foreach (var entry in ignoredList)
            {
                writer.WriteShort(entry.TypeId);
                entry.Serialize(writer);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            var limit = reader.ReadUShort();
            ignoredList = new IgnoredInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                ignoredList[i] = (IgnoredInformations)ProtocolManager.Instance.GetType(reader.ReadUShort());
                ignoredList[i].Deserialize(reader);
            }
        }
    }
}
