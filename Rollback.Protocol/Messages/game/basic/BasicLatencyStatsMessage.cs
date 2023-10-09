using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record BasicLatencyStatsMessage : Message
	{
        public ushort latency;
        public short sampleCount;
        public short max;

        public const int Id = 5663;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public BasicLatencyStatsMessage()
        {
        }
        public BasicLatencyStatsMessage(ushort latency, short sampleCount, short max)
        {
            this.latency = latency;
            this.sampleCount = sampleCount;
            this.max = max;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUShort(latency);
            writer.WriteShort(sampleCount);
            writer.WriteShort(max);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            latency = reader.ReadUShort();
            if (latency < 0 || latency > 65535)
                throw new Exception("Forbidden value on latency = " + latency + ", it doesn't respect the following condition : latency < 0 || latency > 65535");
            sampleCount = reader.ReadShort();
            if (sampleCount < 0)
                throw new Exception("Forbidden value on sampleCount = " + sampleCount + ", it doesn't respect the following condition : sampleCount < 0");
            max = reader.ReadShort();
            if (max < 0)
                throw new Exception("Forbidden value on max = " + max + ", it doesn't respect the following condition : max < 0");
		}
	}
}
