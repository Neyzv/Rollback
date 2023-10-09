using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record CharacterFirstSelectionMessage : CharacterSelectionMessage
	{
        public bool doTutorial;

        public new const int Id = 6084;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public CharacterFirstSelectionMessage()
        {
        }
        public CharacterFirstSelectionMessage(int id, bool doTutorial) : base(id)
        {
            this.doTutorial = doTutorial;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteBoolean(doTutorial);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            doTutorial = reader.ReadBoolean();
		}
	}
}
