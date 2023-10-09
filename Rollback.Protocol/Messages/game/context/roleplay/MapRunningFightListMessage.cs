using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record MapRunningFightListMessage : Message
	{
        public FightExternalInformations[] fights;

        public const int Id = 5743;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public MapRunningFightListMessage()
        {
        }
        public MapRunningFightListMessage(FightExternalInformations[] fights)
        {
            this.fights = fights;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUShort((ushort)fights.Length);
            foreach (var entry in fights)
            {
                 entry.Serialize(writer);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            var limit = reader.ReadUShort();
            fights = new FightExternalInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 fights[i] = new FightExternalInformations();
                 fights[i].Deserialize(reader);
            }
		}
	}
}
