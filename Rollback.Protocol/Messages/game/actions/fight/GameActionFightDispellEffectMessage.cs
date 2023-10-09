using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record GameActionFightDispellEffectMessage : GameActionFightDispellMessage
	{
        public int boostUID;

        public new const int Id = 6113;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameActionFightDispellEffectMessage()
        {
        }
        public GameActionFightDispellEffectMessage(short actionId, int sourceId, int targetId, int boostUID) : base(actionId, sourceId, targetId)
        {
            this.boostUID = boostUID;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(boostUID);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            boostUID = reader.ReadInt();
            if (boostUID < 0)
                throw new Exception("Forbidden value on boostUID = " + boostUID + ", it doesn't respect the following condition : boostUID < 0");
		}
	}
}
