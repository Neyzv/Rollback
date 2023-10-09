using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Types
{
    public record ActorExtendedAlignmentInformations : ActorAlignmentInformations
	{ 
		public ushort honor;
        public ushort dishonor;
        public bool pvpEnabled;
		public new const short Id = 202;
		public override short TypeId
		{
			get { return Id; }
		}
		public ActorExtendedAlignmentInformations()
		{
		}
		public ActorExtendedAlignmentInformations(sbyte alignmentSide, sbyte alignmentValue, sbyte alignmentGrade, int characterPower, ushort honor, ushort dishonor, bool pvpEnabled) : base(alignmentSide, alignmentValue, alignmentGrade, characterPower)
		{
			this.honor = honor;
			this.dishonor = dishonor;
			this.pvpEnabled = pvpEnabled;
		}
		public override void Serialize(BigEndianWriter writer)
		{
			base.Serialize(writer);
			writer.WriteUShort(honor);
			writer.WriteUShort(dishonor);
			writer.WriteBoolean(pvpEnabled);
		}
		public override void Deserialize(BigEndianReader reader)
		{
			base.Deserialize(reader);
			honor = reader.ReadUShort();
			if (honor < 0 || honor > 18000)
				throw new Exception("Forbidden value on honor = " + honor + ", it doesn't respect the following condition : honor < 0 || honor > 18000");
			dishonor = reader.ReadUShort();
			if (dishonor < 0 || dishonor > 500)
				throw new Exception("Forbidden value on dishonor = " + dishonor + ", it doesn't respect the following condition : dishonor < 0 || dishonor > 500");
			pvpEnabled = reader.ReadBoolean();
		}
	}
}