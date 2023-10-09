using Rollback.Common.ORM;
using Rollback.World.Game.Criterion;

namespace Rollback.World.Database.Npcs
{
    public static class NpcReplyRelator
    {
        public const string GetReplies = "SELECT * FROM npcs_replies";
    }

    [Table("npcs_replies")]
    public sealed record NpcReplyRecord
    {
        public NpcReplyRecord() =>
            (Action, _stringCriterion, Parameters) = (string.Empty, string.Empty, string.Empty);

        [Key]
        public int Id { get; set; }

        public short MessageId { get; set; }

        public short ReplyId { get; set; }

        public string Action { get; set; }

        private string _stringCriterion;
        public string StringCriterion
        {
            get => _stringCriterion;
            set
            {
                _stringCriterion = value;

                if (!string.IsNullOrEmpty(value))
                    Criterion = CriterionManager.Instance.Parse(value);
            }
        }

        [Ignore]
        public CriterionExpression? Criterion { get; private set; }

        public short Priority { get; set; }

        public string Parameters { get; set; }
    }
}
