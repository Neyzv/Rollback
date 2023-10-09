using Rollback.Common.Logging;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Effects.Types;

namespace Rollback.World.Game.Effects.Handlers
{
    public abstract class EffectHandler
    {
        private EffectInteger? _effectInteger;

        public EffectBase Effect { get; set; }

        public EffectHandler(EffectBase effect) =>
            Effect = effect;

        public EffectInteger? GenerateEffect()
        {
            var effect = _effectInteger ??= Effect.GenerateEffect(EffectGenerationType.Normal, EffectGenerationContext.Spell) as EffectInteger;

            if (effect is null)
                Logger.Instance.LogWarn($"Can not generate effect {Effect.Id}, because it doesn't return an EffectInteger...");

            return effect;
        }

        public abstract void Apply();
    }
}
