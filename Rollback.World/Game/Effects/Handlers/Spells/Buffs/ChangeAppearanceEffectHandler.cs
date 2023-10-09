using Rollback.Common.DesignPattern.Attributes;
using Rollback.Common.Logging;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Effects.Types;
using Rollback.World.Game.Fights;
using Rollback.World.Game.World.Maps.CellsZone;

namespace Rollback.World.Game.Effects.Handlers.Spells.Buffs
{
    [Identifier(EffectId.EffectChangeAppearance), Identifier(EffectId.EffectChangeAppearance335)]
    public sealed class ChangeAppearanceEffectHandler : SpellEffectHandler
    {
        public ChangeAppearanceEffectHandler(EffectBase effect, List<FightActor> target, SpellCast cast, Zone zone) : base(effect, target, cast, zone) { }

        protected override void InternalApply(FightActor fighter)
        {
            // TO DO riding a mount
            if (Effect is EffectDice effectDice)
            {
                var newLook = fighter.Look.Clone();

                short? bones = default;
                short? scale = default;

                switch (effectDice.Value)
                {
                    case 667: // Picole
                        bones = 44; // 1084
                        break;

                    case 729: // Momification
                        bones = 113; // 1068
                        break;

                    case 874: // Colere de Zatoïshwan
                        bones = 453; // 1202
                        scale = 80; // 60
                        break;

                    case 671: // Puissance sylvestre
                        bones = 893;
                        scale = 80;
                        break;

                    default:
                        Logger.Instance.LogWarn($"Unhandled identifier {effectDice.Value} to change entity appearance...");
                        return;
                }

                if (bones is not null)
                    newLook.Bones = bones.Value;

                if (scale is not null)
                    newLook.Scales = new() { scale.Value };

                fighter.AddSkinBuff(this, newLook);
            }
        }
    }
}
