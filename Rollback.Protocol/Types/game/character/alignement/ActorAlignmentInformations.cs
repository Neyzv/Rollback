using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Types
{
    public record ActorAlignmentInformations
	{ 
		public sbyte alignmentSide;
        public sbyte alignmentValue;
        public sbyte alignmentGrade;
        public int characterPower;
		public const short Id = 201;
		public virtual short TypeId
		{
			get { return Id; }
		}
		public ActorAlignmentInformations()
		{
		}
		public ActorAlignmentInformations(sbyte alignmentSide, sbyte alignmentValue, sbyte alignmentGrade, int characterPower)
		{
			this.alignmentSide = alignmentSide;
			this.alignmentValue = alignmentValue;
			this.alignmentGrade = alignmentGrade;
			this.characterPower = characterPower;
		}

		public virtual void Serialize(BigEndianWriter writer)
		{
            writer.WriteSByte(alignmentSide);
			writer.WriteSByte(alignmentValue);
			writer.WriteSByte(alignmentGrade);
			writer.WriteInt(characterPower);
		}
		public virtual void Deserialize(BigEndianReader reader)
		{
            alignmentSide = reader.ReadSByte();
			alignmentValue = reader.ReadSByte();
			if (alignmentValue < 0)
				throw new Exception("Forbidden value on alignmentValue = " + alignmentValue + ", it doesn't respect the following condition : alignmentValue < 0");
			alignmentGrade = reader.ReadSByte();
			if (alignmentGrade < 0)
				throw new Exception("Forbidden value on alignmentGrade = " + alignmentGrade + ", it doesn't respect the following condition : alignmentGrade < 0");
			characterPower = reader.ReadInt();
			if (characterPower < 0)
				throw new Exception("Forbidden value on characterPower = " + characterPower + ", it doesn't respect the following condition : characterPower < 0");
		}
	}
}