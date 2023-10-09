namespace Rollback.World.Game.Effects.Handlers
{
    public abstract class TargetedEffectHandler<TTarget> : EffectHandler
        where TTarget : class
    {
        public TTarget Target { get; protected set; }

        public TargetedEffectHandler(EffectBase effect, TTarget target)
            : base(effect) =>
            Target = target;
    }
}
