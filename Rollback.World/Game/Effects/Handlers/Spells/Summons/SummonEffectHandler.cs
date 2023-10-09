using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Effects.Types;
using Rollback.World.Game.Fights;
using Rollback.World.Game.Fights.Fighters;
using Rollback.World.Game.RolePlayActors.Monsters;
using Rollback.World.Game.World.Maps.CellsZone;

namespace Rollback.World.Game.Effects.Handlers.Spells.Summons
{
    [Identifier(EffectId.EffectSummon), Identifier(EffectId.EffectSummonStatic)]
    public sealed class SummonEffectHandler : SpellEffectHandler
    {
        public SummonEffectHandler(EffectBase effect, List<FightActor> target, SpellCast cast, Zone zone) : base(effect, target, cast, zone) =>
            Target = new() { Cast.Caster };

        protected override void InternalApply(FightActor fighter)
        {
            if (fighter.CanSummon && Effect is EffectDice effectDice)
            {
                var monsterTemplate = MonsterManager.Instance.GetMonsterRecordById(effectDice.DiceNum);

                if (monsterTemplate is not null && monsterTemplate.Grades.Count >= effectDice.DiceFace)
                {
                    var cell = Cast.TargetedCell;
                    if (!fighter.Team!.Fight.IsCellFreeToWalkOn(cell.Id))
                        cell = fighter.Team.Fight.GetFirstFreeCellNear(cell.Point);

                    if (cell is not null)
                        fighter.AddSummon(Effect.Id is EffectId.EffectSummonStatic ? new SummonedStaticMonster(new Monster(monsterTemplate, (sbyte)effectDice.DiceFace), cell, fighter.Direction, fighter) :
                            new SummonedMonster(new Monster(monsterTemplate, (sbyte)effectDice.DiceFace), cell, fighter.Direction, fighter));
                }
            }
        }
    }
}
