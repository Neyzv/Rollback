using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record CharacterCreationRequestMessage : Message
	{
        public string name;
        public sbyte breed;
        public bool sex;
        public int[] colors;

        public const int Id = 160;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public CharacterCreationRequestMessage()
        {
        }
        public CharacterCreationRequestMessage(string name, sbyte breed, bool sex, int[] colors)
        {
            this.name = name;
            this.breed = breed;
            this.sex = sex;
            this.colors = colors;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteString(name);
            writer.WriteSByte(breed);
            writer.WriteBoolean(sex);
            foreach (var entry in colors)
            {
                 writer.WriteInt(entry);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            name = reader.ReadString();
            breed = reader.ReadSByte();
            if (breed < (byte)Enums.BreedEnum.Feca || breed > (byte)Enums.BreedEnum.Pandawa)
                throw new Exception("Forbidden value on breed = " + breed + ", it doesn't respect the following condition : breed < (byte)Enums.BreedEnum.Feca || breed > (byte)Enums.BreedEnum.Pandawa");
            sex = reader.ReadBoolean();
            colors = new int[6];
            for (int i = 0; i < 6; i++)
            {
                 colors[i] = reader.ReadInt();
            }
		}
	}
}
