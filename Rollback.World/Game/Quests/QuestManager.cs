using System.Reflection;
using Rollback.Common.DesignPattern.Assemblies;
using Rollback.Common.DesignPattern.Attributes;
using Rollback.Common.DesignPattern.Instance;
using Rollback.Common.Initialization;
using Rollback.Common.Logging;
using Rollback.Common.ORM;
using Rollback.World.CustomEnums;
using Rollback.World.Database.Characters;
using Rollback.World.Database.Quests;
using Rollback.World.Game.Quests.Objectives;
using Rollback.World.Game.Quests.Objectives.Events;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Quests
{
    public sealed class QuestManager : Singleton<QuestManager>
    {
        private readonly Dictionary<short, QuestRecord> _quests;
        private readonly Dictionary<QuestObjectiveType, Func<Quest, QuestObjectiveRecord, Character, int, QuestObjective>> _questObjectivesFactories;
        private readonly Dictionary<short, List<QuestObjectiveEventRecord>> _questObjectivesEvents;
        private readonly Dictionary<string, Func<Character, QuestObjective, QuestObjectiveEventRecord, QuestObjectiveEvent>> _questObjectivesEventsFactories;

        public QuestManager()
        {
            _quests = new();
            _questObjectivesFactories = new();
            _questObjectivesEvents = new();
            _questObjectivesEventsFactories = new();
        }

        [Initializable(InitializationPriority.DependantDatasManager, "Quests")]
        public void Initialize()
        {
            Logger.Instance.Log("\tLoading objectives...");
            var questObjectiveType = typeof(QuestObjective);
            var questObjectiveEventType = typeof(QuestObjectiveEvent);

            foreach (var (type, attributes) in from assembly in AssemblyManager.Instance.Assemblies
                                               from type in assembly.GetTypes()
                                               let attributes = type.GetCustomAttributes<IdentifierAttribute>(false)
                                               where attributes.Any() && type.IsClass && !type.IsAbstract
                                               select (type, attributes))
            {
                if (type.IsSubclassOf(questObjectiveType))
                {
                    var constructor = type.GetConstructor(new[] { typeof(Quest), typeof(QuestObjectiveRecord), typeof(Character), typeof(int) });

                    if (constructor is not null)
                    {
                        var questObjectiveFactory = (Quest quest, QuestObjectiveRecord record, Character owner, int progression) =>
                            (QuestObjective)constructor.Invoke(new object[] { quest, record, owner, progression });

                        foreach (var attribute in attributes)
                            if (attribute.Identifier is QuestObjectiveType identifier)
                                if (!_questObjectivesFactories.TryAdd(identifier, questObjectiveFactory))
                                    Logger.Instance.LogError(msg: $"Found two quest objectives with alias {identifier}...");
                    }
                    else
                        Logger.Instance.LogError(msg: $"Can not find a valid constructor for {type.Name}...");
                }
                else if (type.IsSubclassOf(questObjectiveEventType))
                {
                    var constructor = type.GetConstructor(new[] { typeof(Character), typeof(QuestObjective), typeof(QuestObjectiveEventRecord) });

                    if (constructor is not null)
                    {
                        var questObjectiveEventFactory = (Character owner, QuestObjective objective, QuestObjectiveEventRecord record) =>
                            (QuestObjectiveEvent)constructor.Invoke(new object[] { owner, objective, record });

                        foreach (var attribute in attributes)
                            if (attribute.Identifier is string identifier)
                                if (!_questObjectivesEventsFactories.TryAdd(identifier, questObjectiveEventFactory))
                                    Logger.Instance.LogError(msg: $"Found two quest objectives with alias {identifier}...");
                    }
                    else
                        Logger.Instance.LogError(msg: $"Can not find a valid constructor for {type.Name}...");
                }
            }

            foreach (var questObjectiveEventRecord in DatabaseAccessor.Instance.Select<QuestObjectiveEventRecord>(QuestObjectiveEventRelator.GetObjectiveEvent))
            {
                if (_questObjectivesEvents.ContainsKey(questObjectiveEventRecord.QuestObjectiveId))
                    _questObjectivesEvents[questObjectiveEventRecord.QuestObjectiveId].Add(questObjectiveEventRecord);
                else
                    _questObjectivesEvents.Add(questObjectiveEventRecord.QuestObjectiveId, new() { questObjectiveEventRecord });
            }

            Logger.Instance.Log("\tLoading records...");
            foreach (var questRecord in DatabaseAccessor.Instance.Select<QuestRecord>(QuestRelator.GetQuests))
            {
                if (!string.IsNullOrEmpty(questRecord.StepsCSV))
                {
                    var stepIds = questRecord.StepsCSV.Split(',');
                    questRecord.Steps = new QuestStepRecord[stepIds.Length];

                    for (var i = 0; i < stepIds.Length; i++)
                    {
                        if (short.TryParse(stepIds[i], out var stepId))
                        {
                            var step = DatabaseAccessor.Instance.SelectSingle<QuestStepRecord>(string.Format(QuestStepRelator.GetQuestStepById, stepId));

                            if (step is null)
                                Logger.Instance.LogError(msg: $"Can not find quest step {stepId}...");
                            else
                            {
                                if (!string.IsNullOrEmpty(step.ObjectivesCSV))
                                {
                                    var objectivesIds = step.ObjectivesCSV.Split(',');
                                    step.Objectives = new QuestObjectiveRecord[objectivesIds.Length];

                                    for (var j = 0; j < objectivesIds.Length; j++)
                                    {
                                        if (short.TryParse(objectivesIds[j], out var objectiveId))
                                        {
                                            var objective = DatabaseAccessor.Instance.SelectSingle<QuestObjectiveRecord>(
                                                string.Format(QuestObjectiveRelator.GetQuestObjectiveById, objectiveId));

                                            if (objective is null)
                                                Logger.Instance.LogError(msg: $"Can not find quest objective {objectiveId}...");
                                            else
                                            {
                                                step.Objectives[j] = objective;
                                            }
                                        }
                                        else
                                            Logger.Instance.LogError(msg: $"Can not parse quest objective id {objectivesIds[j]}...");
                                    }
                                }

                                questRecord.Steps[i] = step;
                            }
                        }
                        else
                            Logger.Instance.LogError(msg: $"Can not parse quest step id {stepIds[i]}...");
                    }
                }

                _quests[questRecord.Id] = questRecord;
            }
        }

        public QuestRecord? GetQuestRecordById(short id) =>
            _quests.ContainsKey(id) ? _quests[id] : default;

        public List<QuestObjective> GetQuestStepInfos(Quest quest, CharacterQuestRecord characterQuestRecord, QuestStepRecord stepRecord, Character owner)
        {
            var result = new List<QuestObjective>();

            if (characterQuestRecord.Template is not null)
            {
                foreach (var objectiveRecord in stepRecord.Objectives)
                {
                    if (_questObjectivesFactories.ContainsKey(objectiveRecord.Type))
                        result.Add(_questObjectivesFactories[objectiveRecord.Type](quest, objectiveRecord, owner,
                            characterQuestRecord.Objectives.ContainsKey(objectiveRecord.Id) ? characterQuestRecord.Objectives[objectiveRecord.Id] : default));
                    else
                    {
                        result.Add(new FreeFormQuestObjective(quest, objectiveRecord, owner, characterQuestRecord.Objectives.ContainsKey(objectiveRecord.Id) ? characterQuestRecord.Objectives[objectiveRecord.Id] : default));
                        Logger.Instance.LogWarn($"Can not find a quest objective with identifier {objectiveRecord.Type}...");
                    }
                }
            }
            else
            {
                Logger.Instance.LogWarn($"Force disconnection of client {owner.Client} : Can not find quest record {characterQuestRecord.QuestId}...");
                owner.Client.Dispose();
            }

            return result;
        }

        public Dictionary<QuestObjectiveEventTriggerType, List<QuestObjectiveEvent>> GetObjectiveEvents(Character owner, QuestObjective objective)
        {
            var result = new Dictionary<QuestObjectiveEventTriggerType, List<QuestObjectiveEvent>>();

            if (_questObjectivesEvents.ContainsKey(objective.Id))
                foreach (var questObjectiveEventRecord in _questObjectivesEvents[objective.Id])
                {
                    if (_questObjectivesEventsFactories.ContainsKey(questObjectiveEventRecord.Action))
                    {
                        var objectiveEvent = _questObjectivesEventsFactories[questObjectiveEventRecord.Action](owner, objective, questObjectiveEventRecord);

                        if (result.ContainsKey(questObjectiveEventRecord.TriggerType))
                            result[questObjectiveEventRecord.TriggerType].Add(objectiveEvent);
                        else
                            result.Add(questObjectiveEventRecord.TriggerType, new() { objectiveEvent });
                    }
                    else
                        Logger.Instance.LogWarn($"Unhandled quest objective event action {questObjectiveEventRecord.Action}...");
                }

            return result;
        }
    }
}
