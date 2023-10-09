using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record GameActionFightSummonMessage : AbstractGameActionMessage
    {
        public GameFightFighterInformations summon;

        public new const int Id = 5825;
        public override uint MessageId
        {
            get { return Id; }
        }
        public GameActionFightSummonMessage()
        {
        }
        public GameActionFightSummonMessage(short actionId, int sourceId, GameFightFighterInformations summon) : base(actionId, sourceId)
        {
            this.summon = summon;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(summon.TypeId);
            summon.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            summon = (GameFightFighterInformations)ProtocolManager.Instance.GetType(reader.ReadUShort());
            summon.Deserialize(reader);
        }
    }
}
