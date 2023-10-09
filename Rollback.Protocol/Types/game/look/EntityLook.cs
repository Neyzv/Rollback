using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Types
{
    public record EntityLook
    {
        public short bonesId;
        public short[] skins;
        public int[] indexedColors;
        public short[] scales;
        public SubEntity[] subentities;
        public const short Id = 55;
        public virtual short TypeId
        {
            get { return Id; }
        }
        public EntityLook()
        {
        }
        public EntityLook(short bonesId, short[] skins, int[] indexedColors, short[] scales, SubEntity[] subentities)
        {
            this.bonesId = bonesId;
            this.skins = skins;
            this.indexedColors = indexedColors;
            this.scales = scales;
            this.subentities = subentities;
        }

        public virtual void Serialize(BigEndianWriter writer)
        {
            writer.WriteShort(bonesId);
            writer.WriteUShort((ushort)skins.Length);
            foreach (var entry in skins)
            {
                writer.WriteShort(entry);
            }
            writer.WriteUShort((ushort)indexedColors.Length);
            foreach (var entry in indexedColors)
            {
                writer.WriteInt(entry);
            }
            writer.WriteUShort((ushort)scales.Length);
            foreach (var entry in scales)
            {
                writer.WriteShort(entry);
            }
            writer.WriteUShort((ushort)subentities.Length);
            foreach (var entry in subentities)
            {
                entry.Serialize(writer);
            }
        }
        public virtual void Deserialize(BigEndianReader reader)
        {
            bonesId = reader.ReadShort();
            if (bonesId < 0)
                throw new Exception("Forbidden value on bonesId = " + bonesId + ", it doesn't respect the following condition : bonesId < 0");
            var limit = reader.ReadUShort();
            skins = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                skins[i] = reader.ReadShort();
            }
            limit = reader.ReadUShort();
            indexedColors = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                indexedColors[i] = reader.ReadInt();
            }
            limit = reader.ReadUShort();
            scales = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                scales[i] = reader.ReadShort();
            }
            limit = reader.ReadUShort();
            subentities = new SubEntity[limit];
            for (int i = 0; i < limit; i++)
            {
                subentities[i] = new SubEntity();
                subentities[i].Deserialize(reader);
            }
        }
    }
}
                                        
