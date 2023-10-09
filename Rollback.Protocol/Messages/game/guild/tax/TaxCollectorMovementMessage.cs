using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record TaxCollectorMovementMessage : Message
    {
        public bool hireOrFire;
        public TaxCollectorBasicInformations basicInfos;
        public string playerName;

        public const int Id = 5633;
        public override uint MessageId
        {
            get { return Id; }
        }
        public TaxCollectorMovementMessage()
        {
        }
        public TaxCollectorMovementMessage(bool hireOrFire, TaxCollectorBasicInformations basicInfos, string playerName)
        {
            this.hireOrFire = hireOrFire;
            this.basicInfos = basicInfos;
            this.playerName = playerName;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteBoolean(hireOrFire);
            basicInfos.Serialize(writer);
            writer.WriteString(playerName);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            hireOrFire = reader.ReadBoolean();
            basicInfos = new TaxCollectorBasicInformations();
            basicInfos.Deserialize(reader);
            playerName = reader.ReadString();
        }
    }
}
