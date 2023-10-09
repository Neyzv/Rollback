using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.Database.Quests;
using Rollback.World.Game.Fights;
using Rollback.World.Game.Interactions;
using Rollback.World.Game.Interactions.Dialogs.Npcs;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Game.RolePlayActors.Monsters;

namespace Rollback.World.Game.Quests.Objectives.Events.Types
{
    [Identifier("MonsterFight")]
    public sealed class MonsterFightQuestObjectiveEvent : QuestObjectiveEvent
    {
        private short? _npcId;
        public short NpcId =>
            _npcId ??= GetParameterValue<short>(0);

        private string? _monstersInformations;
        public string MonstersInformations =>
            _monstersInformations ??= GetParameterValue<string>(1)!;

        private bool? _activateBlades;
        public bool ActivateBlades =>
            _activateBlades ??= GetParameterValue<bool>(2);

        public MonsterFightQuestObjectiveEvent(Character owner, QuestObjective objective, QuestObjectiveEventRecord record) : base(owner, objective, record) { }

        public override void Trigger() =>
            _owner.LeaveInteraction += OnLeaveInteraction;

        public override void UnTrigger() =>
            _owner.LeaveInteraction -= OnLeaveInteraction;

        private void OnLeaveInteraction(Character character, IInteraction interaction)
        {
            if (interaction is NpcDialog dialog && dialog.Dialoger.Record.Id == NpcId)
            {
                var monsters = new List<Monster>();

                foreach (var monsterInfo in MonstersInformations.Split('|'))
                {
                    var monsterInfoSplitted = monsterInfo.Split(',');

                    if (!string.IsNullOrEmpty(monsterInfoSplitted[0]))
                    {
                        if (short.TryParse(monsterInfoSplitted[0], out var monsterId))
                        {
                            sbyte gradeId = -1;

                            if (monsterInfoSplitted.Length > 1 && !sbyte.TryParse(monsterInfoSplitted[1], out gradeId))
                                _owner.SendServerMessage($"Incorrect monster grade {monsterInfoSplitted[1]} isn't a correct number...");
                            else if (MonsterManager.Instance.GetMonsterRecordById(monsterId) is { } monsterTemplate)
                            {
                                if (gradeId is -1)
                                    gradeId = (sbyte)Random.Shared.Next(1, monsterTemplate.Grades.Count + 1);

                                if (gradeId <= monsterTemplate.Grades.Count)
                                    monsters.Add(new(monsterTemplate, gradeId));
                                else
                                    _owner.SendServerMessage($"Incorrect monster informations, grade {gradeId} isn't correct...");
                            }
                        }
                        else
                            _owner.SendServerMessage($"Can not parse monster informations...");
                    }
                    else
                        _owner.SendServerMessage($"Can not identify monster informations...");
                }

                if (monsters.Count is not 0)
                    FightManager.CreatePvM(_owner.MapInstance, _owner, monsters, ActivateBlades)?.StartPlacement();
            }
        }
    }
}
