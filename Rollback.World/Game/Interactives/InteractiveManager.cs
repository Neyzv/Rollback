using System.Reflection;
using Rollback.Common.DesignPattern.Assemblies;
using Rollback.Common.DesignPattern.Attributes;
using Rollback.Common.DesignPattern.Instance;
using Rollback.Common.Initialization;
using Rollback.Common.Logging;
using Rollback.Common.ORM;
using Rollback.World.Database.Interactives;
using Rollback.World.Game.Interactives.Skills;
using Rollback.World.Game.World;

namespace Rollback.World.Game.Interactives
{
    public sealed class InteractiveManager : Singleton<InteractiveManager>
    {
        public const byte MaxDistanceToInteract = 2;

        private readonly Dictionary<short, InteractiveSkillTemplateRecord> _skillTemplates;
        private readonly Dictionary<int, InteractiveSkillRecord> _skills;
        private readonly Dictionary<int, Func<InteractiveObject, InteractiveSkillRecord, Skill>> _skillActions;

        public InteractiveManager()
        {
            _skillTemplates = new();
            _skills = new();
            _skillActions = new();
        }

        [Initializable(InitializationPriority.LowLevelDatasManager, "Interactives")]
        public void Initialize()
        {
            Logger.Instance.Log("\tLoad skills templates...");
            foreach (var skillTemplate in DatabaseAccessor.Instance.Select<InteractiveSkillTemplateRecord>(InteractiveSkillTemplateRelator.GetInteractiveSkillTemplates))
                _skillTemplates[skillTemplate.Id] = skillTemplate;

            var skillType = typeof(Skill);
            var interactiveSkillRecordType = typeof(InteractiveSkillRecord);
            var interactiveObjectType = typeof(InteractiveObject);
            foreach (var (type, attributes) in from assembly in AssemblyManager.Instance.Assemblies
                                               from type in assembly.GetTypes()
                                               let attributes = type.GetCustomAttributes<IdentifierAttribute>()
                                               where attributes.Any() && !type.IsAbstract && type.IsSubclassOf(skillType)
                                               select (type, attributes))
            {
                var constructor = type.GetConstructor(new[] { interactiveObjectType, interactiveSkillRecordType });
                if (constructor is not null)
                {
                    var skillFactory = (InteractiveObject interactive, InteractiveSkillRecord record) =>
                        (Skill)constructor.Invoke(new object[] { interactive, record });

                    foreach (var attribute in attributes)
                        if (attribute.Identifier is int id)
                            _skillActions[id] = skillFactory;
                }
                else
                    Logger.Instance.LogError(msg: $"Can not find a valid constructor for type {type.Name}...");
            }

            Logger.Instance.Log("\tLoad skills...");
            foreach (var skill in DatabaseAccessor.Instance.Select<InteractiveSkillRecord>(InteractiveSkillRelator.GetSkills))
                _skills[skill.Id] = skill;

            Logger.Instance.Log("\tSpawning Interactives...");
            foreach (var spawn in DatabaseAccessor.Instance.Select<InteractiveSpawnRecord>(InteractiveSpawnRelator.GetSpawns))
                WorldManager.Instance.GetMapById(spawn.MapId)?.AddInteractive(spawn);
        }

        public Skill? CreateSkill(InteractiveObject interactive, int id)
        {
            Skill? skill = default;

            if (GetInteractiveSkillById(id) is { } record)
            {
                if (record.Template?.GatheredRessourceItem > 0)
                    skill = new HarvestSkill(interactive, record);
                else if (record.Template?.CraftableItemIds.Count is not 0)
                    skill = new CraftSkill(interactive, record);
                else if (_skillActions.ContainsKey(record.SkillTemplateId))
                    skill = _skillActions[record.SkillTemplateId](interactive, record);
            }

            return skill;
        }

        public InteractiveSkillTemplateRecord? GetInteractiveSkillTemplateById(short id) =>
            _skillTemplates.ContainsKey(id) ? _skillTemplates[id] : default;

        public InteractiveSkillTemplateRecord[] GetInteractiveSkillTemplates(Predicate<InteractiveSkillTemplateRecord>? p = default) =>
            (p is null ? _skillTemplates.Values : _skillTemplates.Values.Where(x => p(x))).ToArray();

        public InteractiveSkillRecord? GetInteractiveSkillById(int id) =>
            _skills.ContainsKey(id) ? _skills[id] : default;
    }
}
