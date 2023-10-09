using Rollback.Common.DesignPattern.Attributes;
using Rollback.Common.Logging;
using Rollback.World.Database.Monsters;
using Rollback.World.Database.Npcs;
using Rollback.World.Game.Fights;
using Rollback.World.Game.Interactions;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Game.RolePlayActors.Monsters;

namespace Rollback.World.Game.RolePlayActors.Npcs.Replies
{
    [Identifier("MonsterFight")]
    public sealed class MonsterFightReply : NpcReply
    {
        private List<(MonsterRecord, sbyte)>? _monstersInformations;
        public List<(MonsterRecord, sbyte)> MonstersInformations =>
            _monstersInformations ??= ParseMonstersInformations();

        private bool? _activateBlades;
        public bool? ActivateBlades =>
            _activateBlades ??= GetParameterValue<bool>(1);

        public MonsterFightReply(NpcReplyRecord record) : base(record) { }

        private List<(MonsterRecord, sbyte)> ParseMonstersInformations()
        {
            var result = new List<(MonsterRecord, sbyte)>();

            foreach (var monsterInfo in GetParameterValue<string>(0)!
                .Split('|'))
            {
                var monsterInfoSplitted = monsterInfo.Split(',');

                if (!string.IsNullOrEmpty(monsterInfoSplitted[0]))
                {
                    if (short.TryParse(monsterInfoSplitted[0], out var monsterId))
                    {
                        sbyte gradeId = -1;

                        if (monsterInfoSplitted.Length > 1 && !sbyte.TryParse(monsterInfoSplitted[1], out gradeId))
                            Logger.Instance.LogError(msg: $"Incorrect monster grade {monsterInfoSplitted[1]} isn't a correct number...");
                        else if (MonsterManager.Instance.GetMonsterRecordById(monsterId) is { } monsterTemplate)
                        {
                            if (gradeId <= monsterTemplate.Grades.Count)
                                result.Add((monsterTemplate, gradeId));
                            else
                                Logger.Instance.LogError(msg: $"Incorrect monster informations, grade {gradeId} isn't correct...");
                        }
                        else
                            Logger.Instance.LogError(msg: $"Incorrect monster informations, can not find monster {monsterId}...");
                    }
                    else
                        Logger.Instance.LogError(msg: "Can not parse monster informations...");
                }
                else
                    Logger.Instance.LogError(msg: "Can not identify monster informations...");
            }

            return result;
        }

        private void OnLeaveInteraction(Character character, IInteraction interaction)
        {
            character.LeaveInteraction -= OnLeaveInteraction;

            if (MonstersInformations is not null && ActivateBlades.HasValue)
            {
                var monsters = new List<Monster>();

                foreach (var (monsterTemplate, gradeId) in MonstersInformations)
                    monsters.Add(new Monster(monsterTemplate, gradeId < 1 ? (sbyte)Random.Shared.Next(1, monsterTemplate.Grades.Count + 1) : gradeId));

                if (monsters.Count is not 0)
                    FightManager.CreatePvM(character.MapInstance, character, monsters, ActivateBlades.Value)?.StartPlacement();
            }
        }

        public override bool Execute(Npc npc, Character character)
        {
            character.LeaveInteraction += OnLeaveInteraction;

            return true;
        }
    }
}
