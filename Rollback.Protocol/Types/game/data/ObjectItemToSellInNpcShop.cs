using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rollback.Protocol.Types
{
    public record ObjectItemToSellInNpcShop : ObjectItemMinimalInformation
    {
        public new const short Id = 352;
        public override short TypeId
        {
            get { return Id; }
        }

        public int objectPrice;
        public string buyCriterion;

        public ObjectItemToSellInNpcShop()
        {
        }
        public ObjectItemToSellInNpcShop(short objectGID, ObjectEffect[] effects, int objectPrice, string buyCriterion) : base(objectGID, effects)
        {
            this.objectPrice = objectPrice;
            this.buyCriterion = buyCriterion;
        }

        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(objectPrice);
            writer.WriteString(buyCriterion);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            objectPrice = reader.ReadInt();
            if (objectPrice < 0)
            {
                throw new Exception("Forbidden value on objectPrice = " + objectPrice + ", it doesn't respect the following condition : objectPrice < 0");
            }
            buyCriterion = reader.ReadString();
        }
    }
}
