using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.CustomEnums;
using Rollback.World.Database.Quests;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Quests.Objectives
{
    [Identifier(QuestObjectiveType.Escort)]
    public sealed class EscortQuestObjective : QuestObjective
    {
        private int? _mapId;
        public int MapId =>
            _mapId ??= GetParameterValue<int>(0);

        private short? _followerItemId;
        public short FollowerItemId =>
            _followerItemId ??= GetParameterValue<short>(1);

        public EscortQuestObjective(Quest quest, QuestObjectiveRecord record, Character owner, int progression) : base(quest, record, owner, progression) { }

        protected override void EnableObjective()
        {
            throw new NotImplementedException();
        }

        protected override void DisableObjective()
        {
            throw new NotImplementedException();
        }
    }
}
