using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Types
{
    public record HumanInformations
    {
        public EntityLook[] followingCharactersLook;
        public sbyte emoteId;
        public ushort emoteEndTime;
        public ActorRestrictionsInformations restrictions;
        public short titleId;
        public const short Id = 157;
        public virtual short TypeId
        {
            get { return Id; }
        }
        public HumanInformations()
        {
        }
        public HumanInformations(EntityLook[] followingCharactersLook, sbyte emoteId, ushort emoteEndTime, ActorRestrictionsInformations restrictions, short titleId)
        {
            this.followingCharactersLook = followingCharactersLook;
            this.emoteId = emoteId;
            this.emoteEndTime = emoteEndTime;
            this.restrictions = restrictions;
            this.titleId = titleId;
        }
        public virtual void Serialize(BigEndianWriter writer)
        {
            writer.WriteUShort((ushort)followingCharactersLook.Length);
            foreach (var entry in followingCharactersLook)
            {
                entry.Serialize(writer);
            }
            writer.WriteSByte(emoteId);
            writer.WriteUShort(emoteEndTime);
            restrictions.Serialize(writer);
            writer.WriteShort(titleId);
        }
        public virtual void Deserialize(BigEndianReader reader)
        {
            var limit = reader.ReadUShort();
            followingCharactersLook = new EntityLook[limit];
            for (int i = 0; i < limit; i++)
            {
                followingCharactersLook[i] = new EntityLook();
                followingCharactersLook[i].Deserialize(reader);
            }
            emoteId = reader.ReadSByte();
            emoteEndTime = reader.ReadUShort();
            if (emoteEndTime < 0 || emoteEndTime > 255)
                throw new Exception("Forbidden value on emoteEndTime = " + emoteEndTime + ", it doesn't respect the following condition : emoteEndTime < 0 || emoteEndTime > 255");
            restrictions = new ActorRestrictionsInformations();
            restrictions.Deserialize(reader);
            titleId = reader.ReadShort();
            if (titleId < 0)
                throw new Exception("Forbidden value on titleId = " + titleId + ", it doesn't respect the following condition : titleId < 0");
        }
    }
}
