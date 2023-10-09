using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record ExchangeStartOkJobIndexMessage : Message
	{
        public int[] jobs;

        public const int Id = 5819;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeStartOkJobIndexMessage()
        {
        }
        public ExchangeStartOkJobIndexMessage(int[] jobs)
        {
            this.jobs = jobs;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUShort((ushort)jobs.Length);
            foreach (var entry in jobs)
            {
                 writer.WriteInt(entry);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            var limit = reader.ReadUShort();
            jobs = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 jobs[i] = reader.ReadInt();
            }
		}
	}
}
