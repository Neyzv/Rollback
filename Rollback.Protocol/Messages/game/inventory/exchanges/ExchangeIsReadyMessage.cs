using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record ExchangeIsReadyMessage : Message
    {
        public int id;
        public bool ready;

        public const int Id = 5509;
        public override uint MessageId
        {
            get { return Id; }
        }
        public ExchangeIsReadyMessage()
        {
        }
        public ExchangeIsReadyMessage(int id, bool ready)
        {
            this.id = id;
            this.ready = ready;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(id);
            writer.WriteBoolean(ready);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            id = reader.ReadInt();
            if (id < 0)
                throw new Exception("Forbidden value on id = " + id + ", it doesn't respect the following condition : id < 0");
            ready = reader.ReadBoolean();
        }
    }
}
