using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record MountInformationInPaddockRequestMessage : Message
	{
        public int mapRideId;

        public const int Id = 5975;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public MountInformationInPaddockRequestMessage()
        {
        }
        public MountInformationInPaddockRequestMessage(int mapRideId)
        {
            this.mapRideId = mapRideId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(mapRideId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            mapRideId = reader.ReadInt();
            if (mapRideId < 0)
                throw new Exception("Forbidden value on mapRideId = " + mapRideId + ", it doesn't respect the following condition : mapRideId < 0");
		}
	}
}
