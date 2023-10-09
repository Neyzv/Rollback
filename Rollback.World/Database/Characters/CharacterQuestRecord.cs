using Rollback.Common.Logging;
using Rollback.Common.ORM;
using Rollback.World.Database.Quests;
using Rollback.World.Game.Quests;

namespace Rollback.World.Database.Characters
{
    public static class CharacterQuestRelator
    {
        public const string GetCharacterQuestsByCharacterId = "SELECT * FROM characters_quests WHERE CharacterId = {0}";
    }

    [Table("characters_quests")]
    public sealed record CharacterQuestRecord
    {
        public CharacterQuestRecord()
        {
            _objectivesCSV = string.Empty;
            Objectives = new();
        }

        [Key]
        public int CharacterId { get; set; }

        private short _questId;
        [Key]
        public short QuestId
        {
            get => _questId;
            set
            {
                _questId = value;
                Template = QuestManager.Instance.GetQuestRecordById(value);
            }
        }

        [Ignore]
        public QuestRecord? Template { get; private set; }

        public short CurrentStepId { get; set; }

        private string _objectivesCSV;
        public string ObjectivesCSV
        {
            get => _objectivesCSV;
            set
            {
                _objectivesCSV = value;

                Objectives.Clear();

                if (!string.IsNullOrEmpty(value))
                    foreach (var objectiveInfos in value.Split(';'))
                    {
                        var splittedValue = objectiveInfos.Split(',');
                        if (short.TryParse(splittedValue[0], out var objectiveId))
                        {
                            var progression = 0;
                            if (splittedValue.Length > 1 && !int.TryParse(splittedValue[1], out progression))
                                Logger.Instance.LogWarn($"Can not convert progression of quest {QuestId} for the objective {objectiveId}...");

                            Objectives[objectiveId] = progression;
                        }
                        else
                            Logger.Instance.LogWarn($"Can not convert quest objective informations {objectiveInfos}...");
                    }
            }
        }

        [Ignore]
        public Dictionary<short, int> Objectives { get; private set; }

        public bool IsFinished { get; set; }
    }
}
