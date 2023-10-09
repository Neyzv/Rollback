using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Types
{
    public record IdentifiedEntityDispositionInformations : EntityDispositionInformations
	{ 
		public int id;
		public new const short Id = 107;
		public override short TypeId
		{
			get { return Id; }
		}
		public IdentifiedEntityDispositionInformations()
		{
		}
		public IdentifiedEntityDispositionInformations(short cellId, sbyte direction, int id) : base(cellId, direction)
		{
			this.id = id;
		}
		public override void Serialize(BigEndianWriter writer)
		{
			base.Serialize(writer);
			writer.WriteInt(id);
		}
		public override void Deserialize(BigEndianReader reader)
		{
			base.Deserialize(reader);
			id = reader.ReadInt();
		}
	}
}