using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Types
{
    public record FriendInformations
    {
        public string name;
        public sbyte playerState;
        public int lastConnection;
        public const short Id = 78;
        public virtual short TypeId
        {
            get { return Id; }
        }
        public FriendInformations()
        {
        }
        public FriendInformations(string name, sbyte playerState, int lastConnection)
        {
            this.name = name;
            this.playerState = playerState;
            this.lastConnection = lastConnection;
        }
        public virtual void Serialize(BigEndianWriter writer)
        {
            writer.WriteString(name);
            writer.WriteSByte(playerState);
            writer.WriteInt(lastConnection);
        }
        public virtual void Deserialize(BigEndianReader reader)
        {
            name = reader.ReadString();
            playerState = reader.ReadSByte();
            if (playerState < 0)
                throw new Exception("Forbidden value on playerState = " + playerState + ", it doesn't respect the following condition : playerState < 0");
            lastConnection = reader.ReadInt();
            if (lastConnection < 0)
                throw new Exception("Forbidden value on lastConnection = " + lastConnection + ", it doesn't respect the following condition : lastConnection < 0");
        }
    }
}

