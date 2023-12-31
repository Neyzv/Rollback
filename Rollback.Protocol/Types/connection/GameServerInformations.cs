﻿using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Types
{
    public record GameServerInformations
    {
        public ushort id;
        public sbyte status;
        public sbyte completion;
        public bool isSelectable;
        public sbyte charactersCount;
        public const short Id = 25;
        public virtual short TypeId
        {
            get { return Id; }
        }
        public GameServerInformations()
        {
        }
        public GameServerInformations(ushort id, sbyte status, sbyte completion, bool isSelectable, sbyte charactersCount)
        {
            this.id = id;
            this.status = status;
            this.completion = completion;
            this.isSelectable = isSelectable;
            this.charactersCount = charactersCount;
        }
        public virtual void Serialize(BigEndianWriter writer)
        {
            writer.WriteUShort(id);
            writer.WriteSByte(status);
            writer.WriteSByte(completion);
            writer.WriteBoolean(isSelectable);
            writer.WriteSByte(charactersCount);
        }
        public virtual void Deserialize(BigEndianReader reader)
        {
            id = reader.ReadUShort();
            if (id < 0 || id > 65535)
                throw new Exception("Forbidden value on id = " + id + ", it doesn't respect the following condition : id < 0 || id > 65535");
            status = reader.ReadSByte();
            if (status < 0)
                throw new Exception("Forbidden value on status = " + status + ", it doesn't respect the following condition : status < 0");
            completion = reader.ReadSByte();
            if (completion < 0 || completion > 100)
                throw new Exception("Forbidden value on completion = " + completion + ", it doesn't respect the following condition : ompletion < 0 || completion > 100");
            isSelectable = reader.ReadBoolean();
            charactersCount = reader.ReadSByte();
            if (charactersCount < 0)
                throw new Exception("Forbidden value on charactersCount = " + charactersCount + ", it doesn't respect the following condition : charactersCount < 0");
        }
    }
}