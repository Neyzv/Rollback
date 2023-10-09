using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.Database.Npcs;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.RolePlayActors.Npcs.Replies
{
    [Identifier("StartQuest")]
    public sealed class StartQuestReply : NpcReply
    {
        private short? _questId;
        public short QuestId =>
            _questId ??= GetParameterValue<short>(0);

        public StartQuestReply(NpcReplyRecord record) : base(record) { }

        public override bool Execute(Npc npc, Character character)
        {
            character.StartQuest(QuestId);

            return true;
        }
    }
}
