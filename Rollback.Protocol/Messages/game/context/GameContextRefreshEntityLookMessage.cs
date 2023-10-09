using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record GameContextRefreshEntityLookMessage : Message
	{
        public int id;
        public EntityLook look;

        public const int Id = 5637;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameContextRefreshEntityLookMessage()
        {
        }
        public GameContextRefreshEntityLookMessage(int id, EntityLook look)
        {
            this.id = id;
            this.look = look;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(id);
            look.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            id = reader.ReadInt();
            look = new EntityLook();
            look.Deserialize(reader);
		}
	}
}
