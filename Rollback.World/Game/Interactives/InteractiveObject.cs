using Rollback.Common.DesignPattern.Threading.Schedul;
using Rollback.Common.Logging;
using Rollback.Protocol.Types;
using Rollback.World.CustomEnums;
using Rollback.World.Database.Interactives;
using Rollback.World.Game.Interactives.Skills;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Game.World.Maps;
using Rollback.World.Handlers.Interactives;

namespace Rollback.World.Game.Interactives
{
    public sealed class InteractiveObject : WorldObject
    {
        private readonly InteractiveSpawnRecord _spawnRecord;

        public int Id =>
            _spawnRecord.Id;

        public int ElementId =>
            _spawnRecord.ElementId;

        public bool Animated =>
            _spawnRecord.Animated || _skills.Values.Any(x => x is HarvestSkill);

        public InteractiveState State { get; private set; }

        private readonly Dictionary<short, Skill> _skills;
        public IReadOnlyDictionary<short, Skill> Skills =>
            _skills;

        public StatedElement StatedElement =>
            new(Id, Cell.Id, (int)State);

        public InteractiveObject(MapInstance mapInstance, InteractiveSpawnRecord spawnRecord)
            : base(mapInstance, mapInstance.Map.Record.Cells[spawnRecord.CellId])
        {
            _spawnRecord = spawnRecord;
            _skills = new();
            LoadSkills();
        }

        private void LoadSkills()
        {
            foreach (var skillId in _spawnRecord.SkillIds)
                if (InteractiveManager.Instance.CreateSkill(this, skillId) is { } skill &&
                    skill.TemplateId.HasValue)
                {
                    if (!_skills.TryAdd(skill.TemplateId!.Value, skill))
                        Logger.Instance.LogError(msg: $"Can not add skill {skillId} because of a duplicate template id...");
                }
                else
                    Logger.Instance.LogError(msg: $"Can not create skill {skillId}...");
        }

        private void EndUse(Character character, Skill skill)
        {
            InteractiveHandler.SendInteractiveUseEndedMessage(character.Client, this, skill);

            skill.Execute(character);

            character.InteractiveObject = default;
        }

        public void ChangeState(InteractiveState state)
        {
            if (Animated)
            {
                State = state;
                MapInstance.Send(InteractiveHandler.SendStatedElementUpdatedMessage, new[] { this });
            }
        }

        public void Use(Character character, short skillId)
        {
            if (Cell.Point.ManhattanDistanceTo(character.Cell.Point) <= InteractiveManager.MaxDistanceToInteract &&
                Skills.ContainsKey(skillId))
            {
                if (Skills[skillId].CanExecute(character))
                {
                    if (Skills[skillId].BeforeExecute(character) && Skills[skillId].CanBeUsed(character))
                    {
                        character.InteractiveObject = this;

                        MapInstance.Send(InteractiveHandler.SendInteractiveUsedMessage, new object[] { character.Id, this, Skills[skillId] });

                        if (Skills[skillId].Duration > 0)
                            Scheduler.Instance.ExecuteDelayed(() => EndUse(character, Skills[skillId]))
                                .WithTime(TimeSpan.FromMilliseconds(Skills[skillId].Duration));
                        else
                            EndUse(character, Skills[skillId]);
                    }
                }
                else
                    //Certaines conditions ne sont pas satisfaites.
                    character.SendInformationMessage(Protocol.Enums.TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 34);
            }
        }

        public InteractiveElement GetInteractiveElement(Character character)
        {
            var enabledSkills = new List<short>();
            var disabledSkills = new List<short>();

            foreach (var skill in Skills.Values.Where(x => x.CanExecute(character)))
            {
                if (skill.TemplateId.HasValue)
                {
                    if (skill.CanBeUsed(character))
                        enabledSkills.Add(skill.TemplateId.Value);
                    else
                        disabledSkills.Add(skill.TemplateId.Value);
                }
            }

            return new(Id, enabledSkills.ToArray(), disabledSkills.ToArray());
        }

        public void Refresh() =>
            MapInstance.Send(InteractiveHandler.SendInteractiveElementUpdatedMessage, new[] { this });
    }
}
