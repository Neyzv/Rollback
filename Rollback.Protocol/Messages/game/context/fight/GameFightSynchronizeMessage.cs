using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record GameFightSynchronizeMessage : Message
    {
        public GameFightFighterInformations[] fighters;

        public const int Id = 5921;
        public override uint MessageId
        {
            get { return Id; }
        }
        public GameFightSynchronizeMessage()
        {
        }
        public GameFightSynchronizeMessage(GameFightFighterInformations[] fighters)
        {
            this.fighters = fighters;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUShort((ushort)fighters.Length);
            foreach (var entry in fighters)
            {
                writer.WriteShort(entry.TypeId);
                entry.Serialize(writer);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            var limit = reader.ReadUShort();
            fighters = new GameFightFighterInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                fighters[i] = (GameFightFighterInformations)ProtocolManager.Instance.GetType(reader.ReadUShort());
                fighters[i].Deserialize(reader);
            }
        }
    }
}
