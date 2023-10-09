using Rollback.World.CustomEnums;
using Rollback.World.Game.Effects.Types;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Effects.Handlers.Items.Stats
{
    public sealed class StatsEffectHandler : ItemEffectHandler
    {
        public StatsEffectHandler(EffectBase effect, Character target) : base(effect, target) { }

        public override void Apply()
        {
            if (Effect.Record is not null && Effect is EffectInteger integer)
            {
                var stat = EffectManager.GetStatByCharacteristic((Characteristic)Effect.Record.Characteristic);
                if (stat is not null)
                    Target.Stats[stat.Value].Equipments += (short)(Effect.Record.Operator is "-" ? -integer.Value : integer.Value);
            }
        }

        public override void UnApply()
        {
            if (Effect.Record is not null && Effect is EffectInteger integer)
            {
                var stat = EffectManager.GetStatByCharacteristic((Characteristic)Effect.Record.Characteristic);
                if (stat is not null)
                    Target.Stats[stat.Value].Equipments -= (short)(Effect.Record.Operator is "-" ? -integer.Value : integer.Value);
            }
        }
    }
}
