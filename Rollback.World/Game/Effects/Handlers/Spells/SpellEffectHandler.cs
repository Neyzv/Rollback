using Rollback.Protocol.Enums;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Effects.Types;
using Rollback.World.Game.Fights;
using Rollback.World.Game.Fights.Buffs.Types;
using Rollback.World.Game.Fights.Fighters;
using Rollback.World.Game.World.Maps.CellsZone;
using Rollback.World.Handlers.Fights;

namespace Rollback.World.Game.Effects.Handlers.Spells
{
    public abstract class SpellEffectHandler : TargetedEffectHandler<List<FightActor>>
    {
        public const short InfiniteDuration = -1000;

        public SpellCast Cast { get; }

        public Zone Zone { get; }

        public FightDispellable Dispellable { get; set; }

        public SpellEffectHandler(EffectBase effect, List<FightActor> target, SpellCast cast, Zone zone)
            : base(effect, target)
        {
            Cast = cast;
            Zone = zone;
        }

        protected abstract void InternalApply(FightActor fighter);

        public virtual bool CanSeeCast(CharacterFighter fighter) =>
            true;

        public sealed override void Apply()
        {
            if (Cast.Caster.Team!.Fight.Started)
            {
                if (Cast.Caster.Visibility is GameActionFightInvisibilityStateEnum.INVISIBLE && Effect.Duration is 0 && EffectManager.EffectDispellInvisibility(Effect.Id))
                    Cast.Caster.SetVisibleState(GameActionFightInvisibilityStateEnum.VISIBLE, Cast.Caster);

                foreach (var fighter in Target)
                {
                    if (fighter.Alive)
                    {
                        var target = fighter;

                        if (EffectManager.IsEffectReflectable(Effect) && fighter.GetBuffs<SpellReflectionBuff>().OrderByDescending(x => (x.Handler.Effect as EffectDice)?.DiceFace).FirstOrDefault()?.Reflect(Cast.Spell) == true)
                        {
                            target = Cast.Caster;
                            Cast.Caster.Team!.Fight.Send(FightHandler.SendGameActionFightReflectSpellMessage, new object[] { Cast.Caster, fighter });
                        }

                        InternalApply(target);
                    }
                }
            }
        }
    }
}
