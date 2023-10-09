using Rollback.Common.ORM;
using Rollback.World.Game.RolePlayActors.Npcs;

namespace Rollback.World.Database.Npcs
{
    public static class NpcRelator
    {
        public const string GetNpcs = "SELECT * FROM npcs_templates";
    }

    [Table("npcs_templates")]
    public sealed record NpcRecord
    {
        public NpcRecord()
        {
            EntityLookString = string.Empty;
            _messagesCSV = string.Empty;
            Messages = new();
            _repliesCSV = string.Empty;
            Replies = new();
            _actionsCSV = string.Empty;
            Actions = Array.Empty<byte>();
            ActionsRecords = new();
            RepliesRecords = new();
        }

        [Key]
        public short Id { get; set; }

        public bool Gender { get; set; }

        public string EntityLookString { get; set; }

        private string _messagesCSV;
        public string MessagesCSV
        {
            get => _messagesCSV;
            set
            {
                _messagesCSV = value;
                Messages = NpcManager.ParseCSVString(value);
            }
        }

        [Ignore]
        public Dictionary<short, int> Messages { get; private set; }

        private string _repliesCSV;
        public string RepliesCSV
        {
            get => _repliesCSV;
            set
            {
                _repliesCSV = value;
                Replies = NpcManager.ParseCSVString(value);
            }
        }

        [Ignore]
        public Dictionary<short, int> Replies { get; private set; }

        private string _actionsCSV;
        public string ActionsCSV
        {
            get => _actionsCSV;
            set
            {
                _actionsCSV = value;

                if (value != string.Empty)
                    Actions = value.Split(",").Select(x => byte.Parse(x)).ToArray();
            }
        }

        [Ignore]
        public byte[] Actions { get; private set; }

        [Ignore]
        public List<NpcActionRecord> ActionsRecords { get; set; }

        [Ignore]
        public List<NpcReplyRecord> RepliesRecords { get; set; }
    }
}
