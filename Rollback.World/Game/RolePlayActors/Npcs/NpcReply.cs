using Rollback.Common.DesignPattern.Instance;
using Rollback.World.Database.Npcs;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.RolePlayActors.Npcs
{
    public abstract class NpcReply : ParameterContainer
    {
        private readonly NpcReplyRecord _record;

        public int Priority =>
            _record.Priority;

        public short MessageId =>
            _record.MessageId;

        public short ReplyId =>
            _record.ReplyId;

        public NpcReply(NpcReplyRecord record) : base(record.Parameters.Split(';')) =>
            _record = record;

        public virtual bool CanExecute(Character character) =>
            _record.Criterion is null || _record.Criterion.Eval(character);

        public abstract bool Execute(Npc npc, Character character);
    }
}
