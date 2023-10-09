using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record StartupActionsListMessage : Message
	{
        public StartupActionAddObject[] actions;

        public const int Id = 1301;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public StartupActionsListMessage()
        {
        }
        public StartupActionsListMessage(StartupActionAddObject[] actions)
        {
            this.actions = actions;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUShort((ushort)actions.Length);
            foreach (var entry in actions)
            {
                 entry.Serialize(writer);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            var limit = reader.ReadUShort();
            actions = new StartupActionAddObject[limit];
            for (int i = 0; i < limit; i++)
            {
                 actions[i] = new StartupActionAddObject();
                 actions[i].Deserialize(reader);
            }
		}
	}
}
