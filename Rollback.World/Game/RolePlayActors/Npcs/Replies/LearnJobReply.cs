using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.CustomEnums;
using Rollback.World.Database.Npcs;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.RolePlayActors.Npcs.Replies
{
    [Identifier("LearnJob")]
    public sealed class LearnJobReply : NpcReply
    {
        private int? _jobId;
        public int JobId =>
            _jobId ??= GetParameterValue<int>(0);

        public LearnJobReply(NpcReplyRecord record) : base(record) { }

        public override bool Execute(Npc npc, Character character)
        {
            character.LearnJob((JobIds)JobId);

            return true;
        }
    }
}
