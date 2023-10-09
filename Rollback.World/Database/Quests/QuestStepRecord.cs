using Rollback.Common.Logging;
using Rollback.Common.ORM;

namespace Rollback.World.Database.Quests
{
    public static class QuestStepRelator
    {
        public const string GetQuestStepById = "SELECT * FROM quests_steps WHERE Id = {0}";
    }

    [Table("quests_steps")]
    public sealed record QuestStepRecord
    {
        public QuestStepRecord()
        {
            _itemsRewardCSV = string.Empty;
            ItemsReward = new();
            EmotesRewardCSV = string.Empty;
            JobsRewardCSV = string.Empty;
            SpellsRewardCSV = string.Empty;
            ObjectivesCSV = string.Empty;
            Objectives = Array.Empty<QuestObjectiveRecord>();
        }

        [Key]
        public short Id { get; set; }

        public uint ExperienceReward { get; set; }

        public int KamasReward { get; set; }

        private string _itemsRewardCSV;
        public string ItemsRewardCSV
        {
            get => _itemsRewardCSV;
            set
            {
                _itemsRewardCSV = value;

                ItemsReward.Clear();

                if (!string.IsNullOrEmpty(value))
                {
                    foreach (var itemInfos in value.Split(';'))
                    {
                        var splittedItemInfos = itemInfos.Split(',');
                        if (splittedItemInfos.Length > 1 && short.TryParse(splittedItemInfos[0], out var itemId) && int.TryParse(splittedItemInfos[1], out var quantity))
                            ItemsReward[itemId] = quantity;
                        else
                            Logger.Instance.LogError(msg: $"Can not parse quest step items reward {itemInfos}, for quest step {Id}...");
                    }
                }
            }
        }

        [Ignore]
        public Dictionary<short, int> ItemsReward { get; private set; }

        public string EmotesRewardCSV { get; set; }

        public string JobsRewardCSV { get; set; }

        public string SpellsRewardCSV { get; set; }

        public string ObjectivesCSV { get; set; }

        [Ignore]
        public QuestObjectiveRecord[] Objectives { get; set; }
    }
}
