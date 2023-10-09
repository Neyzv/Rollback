using Rollback.Common.Logging;
using Rollback.Common.ORM;
using Rollback.World.Database.Characters;
using Rollback.World.Database.Quests;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Handlers.Quests;

namespace Rollback.World.Game.Quests
{
    public sealed class Quest
    {
        private readonly CharacterQuestRecord _characterQuestRecord;
        private readonly Character _owner;
        private readonly List<QuestObjective> _objectives;
        private QuestStepRecord? _currentStepRecord;
        private QuestRecord Record =>
            _characterQuestRecord.Template!;

        public short Id =>
            Record.Id;

        public bool IsRepeatable =>
            Record.IsRepeatable;

        public short CurrentStepId
        {
            get => _characterQuestRecord.CurrentStepId;
            private set => _characterQuestRecord.CurrentStepId = value;
        }

        public bool IsFinished
        {
            get => _characterQuestRecord.IsFinished;
            private set => _characterQuestRecord.IsFinished = value;
        }

        public Quest(Character owner, CharacterQuestRecord characterQuestRecord)
        {
            _owner = owner;
            _characterQuestRecord = characterQuestRecord;
            _objectives = new();

            RefreshQuestInfos();
        }

        private void OnObjectiveCompleted(QuestObjective objective)
        {
            QuestHandler.SendMapNpcsQuestStatusUpdateMessage(_owner.Client);
            QuestHandler.SendQuestObjectiveValidatedMessage(_owner.Client, this, objective);

            if (_objectives.TrueForAll(x => !x.IsInProgress))
            {
                QuestHandler.SendQuestStepValidatedMessage(_owner.Client, this);

                GetCurrentQuestStepRewards();

                var i = 0;
                for (; i < Record.Steps.Length && Record.Steps[i].Id != CurrentStepId; i++) { }

                if (i < Record.Steps.Length - 1)
                {
                    CurrentStepId = Record.Steps[i + 1].Id;
                    RefreshQuestInfos();

                    QuestHandler.SendQuestStepStartedMessage(_owner.Client, this);
                }
                else
                    IsFinished = true;
            }

            if (!IsFinished)
                // Quête mise à jour : <b>$quest%1</b>
                _owner.SendInformationMessage(Protocol.Enums.TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 55, Id);
            else
                // Quête terminée : <b>$quest%1</b>
                _owner.SendInformationMessage(Protocol.Enums.TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 56, Id);

            objective.Completed -= OnObjectiveCompleted;
        }

        private void RefreshQuestInfos()
        {
            _currentStepRecord = Record.Steps.FirstOrDefault(x => x.Id == CurrentStepId);

            if (_currentStepRecord is null)
            {
                Logger.Instance.LogWarn($"Force disconnection of client {_owner.Client} : Can not find quest step record {CurrentStepId} for quest {Id}...");
                _owner.Client.Dispose();
            }
            else
            {
                foreach (var objective in _objectives)
                    objective.Completed -= OnObjectiveCompleted;

                _objectives.Clear();

                foreach (var objective in QuestManager.Instance.GetQuestStepInfos(this, _characterQuestRecord, _currentStepRecord, _owner))
                {
                    _objectives.Add(objective);

                    if (objective.IsInProgress)
                        if (IsFinished)
                            objective.Complete();
                        else
                            objective.Completed += OnObjectiveCompleted;
                }
            }
        }

        private void GetCurrentQuestStepRewards()
        {
            if (_currentStepRecord is not null && !IsFinished)
            {
                if (_currentStepRecord.ExperienceReward > 0)
                    _owner.ChangeExperience(_currentStepRecord.ExperienceReward);

                if (_currentStepRecord.KamasReward > 0)
                    _owner.ChangeKamas(_currentStepRecord.KamasReward);

                foreach (var itemInfo in _currentStepRecord.ItemsReward)
                    _owner.Inventory.AddItem(itemInfo.Key, itemInfo.Value);

                // TO DO others rewards
            }
        }

        public QuestObjective[] GetObjectives(Predicate<QuestObjective>? p = default) =>
            (p is null ? _objectives : _objectives.Where(x => p(x))).ToArray();

        public QuestObjective? GetObjective(Predicate<QuestObjective> p) =>
            _objectives.FirstOrDefault(x => p(x));

        public void Save()
        {
            _characterQuestRecord.ObjectivesCSV = string.Join(';', _objectives.Select(x => $"{x.Id},{x.Progression}"));
            DatabaseAccessor.Instance.InsertOrUpdate(_characterQuestRecord);
        }
    }
}
