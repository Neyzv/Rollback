using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record GameActionFightCloseCombatMessage : AbstractGameActionFightTargetedAbilityMessage
	{
        public int weaponGenericId;

        public new const int Id = 6116;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameActionFightCloseCombatMessage()
        {
        }
        public GameActionFightCloseCombatMessage(short actionId, int sourceId, short destinationCellId, sbyte critical, bool silentCast, int weaponGenericId) : base(actionId, sourceId, destinationCellId, critical, silentCast)
        {
            this.weaponGenericId = weaponGenericId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(weaponGenericId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            weaponGenericId = reader.ReadInt();
            if (weaponGenericId < 0)
                throw new Exception("Forbidden value on weaponGenericId = " + weaponGenericId + ", it doesn't respect the following condition : weaponGenericId < 0");
		}
	}
}
