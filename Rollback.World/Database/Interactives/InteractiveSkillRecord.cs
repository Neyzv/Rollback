using Rollback.Common.ORM;
using Rollback.World.Game.Criterion;
using Rollback.World.Game.Interactives;

namespace Rollback.World.Database.Interactives
{
    public static class InteractiveSkillRelator
    {
        public const string GetSkills = "SELECT * FROM interactives_skills";
    }

    [Table("interactives_skills")]
    public sealed record InteractiveSkillRecord
    {
        public InteractiveSkillRecord() =>
            (ParametersCSV, _stringCriterion) = (string.Empty, string.Empty);

        [Key]
        public int Id { get; set; }

        private short _skillTemplateId;
        public short SkillTemplateId
        {
            get => _skillTemplateId;
            set
            {
                _skillTemplateId = value;
                Template = InteractiveManager.Instance.GetInteractiveSkillTemplateById(value);
            }
        }

        [Ignore]
        public InteractiveSkillTemplateRecord? Template { get; private set; }

        public string ParametersCSV { get; set; }

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
    }
}
