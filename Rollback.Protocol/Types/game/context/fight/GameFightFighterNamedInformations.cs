using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Types
{
    public record GameFightFighterNamedInformations : GameFightFighterInformations
	{ 
		public string name;
		public new const short Id = 158;
		public override short TypeId
		{
			get { return Id; }
		}
		public GameFightFighterNamedInformations()
		{
		}
		public GameFightFighterNamedInformations(int contextualId, EntityLook look, EntityDispositionInformations disposition, sbyte teamId, bool alive, GameFightMinimalStats stats, string name) : base(contextualId, look, disposition, teamId, alive, stats)
		{
			this.name = name;
		}
		public override void Serialize(BigEndianWriter writer)
		{
			base.Serialize(writer);
			writer.WriteString(name);
		}
		public override void Deserialize(BigEndianReader reader)
		{
			base.Deserialize(reader);
			name = reader.ReadString();
		}
	}
}