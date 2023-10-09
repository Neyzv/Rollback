using Rollback.Common.DesignPattern.Instance;
using Rollback.Common.Initialization;
using Rollback.Common.Logging;
using Rollback.Common.ORM;
using Rollback.World.Database.Spells;

namespace Rollback.World.Game.Spells
{
    public sealed class SpellManager : Singleton<SpellManager>
    {
        private readonly Dictionary<int, SpellTemplateRecord> _templates;

        public SpellManager() =>
            _templates = new();

        [Initializable(InitializationPriority.DatasManager, "Spells")]
        public void Initialize()
        {
            foreach (var template in DatabaseAccessor.Instance.Select<SpellTemplateRecord>(SpellTemplateRelator.GetSpellTemplates))
            {
                if (!string.IsNullOrEmpty(template.SpellLevelsCSV))
                {
                    var splittedValue = template.SpellLevelsCSV.Split(',');

                    template.SpellLevels = new SpellLevelRecord[splittedValue.Length];

                    for (int i = 0; i < splittedValue.Length; i++)
                        if (int.TryParse(splittedValue[i], out var spellLevelId))
                        {
                            var level = DatabaseAccessor.Instance.SelectSingle<SpellLevelRecord>(string.Format(SpellLevelRelator.GetSpellLevelById, spellLevelId));

                            if (level is null)
                                Logger.Instance.LogError(msg: $"Couldn't find spell level {spellLevelId}, for spell {template.Id}...");
                            else
                                template.SpellLevels[i] = level;
                        }
                        else
                            Logger.Instance.LogError(msg: $"Error while parsing spell levels of spell {template.Id}...");
                }

                _templates[template.Id] = template;
            }
        }

        public SpellTemplateRecord? GetSpellTemplateById(int id) =>
            _templates.ContainsKey(id) ? _templates[id] : default;
    }
}
