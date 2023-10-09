using System.Drawing;
using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Effects.Types;
using Rollback.World.Game.Fights;
using Rollback.World.Game.Spells;
using Rollback.World.Game.World.Maps.CellsZone;

namespace Rollback.World.Game.Effects.Handlers.Spells.Triggers
{
    [Identifier(EffectId.EffectGlyph), Identifier(EffectId.EffectGlyph402)]
    public sealed class GlyphEffectHandler : SpellEffectHandler
    {
        public GlyphEffectHandler(EffectBase effect, List<FightActor> target, SpellCast cast, Zone zone) : base(effect, target, cast, zone) =>
            Target = new() { Cast.Caster };

        protected override void InternalApply(FightActor fighter)
        {
            if (Effect is EffectDice effectDice && Zone is not null)
            {
                var glyphSpellTemplate = SpellManager.Instance.GetSpellTemplateById(effectDice.DiceNum);

                if (glyphSpellTemplate is not null && effectDice.DiceFace > 0 && glyphSpellTemplate.SpellLevels.Length >= effectDice.DiceFace)
                    Cast.Caster.Team!.Fight.AddGlyph(Effect.Id is EffectId.EffectGlyph402 ? TriggerType.TurnEnd : TriggerType.TurnBegin,
                        Zone, GetGlyphColor(Cast.Spell.Id), Cast.Caster, new Spell(glyphSpellTemplate, (sbyte)effectDice.DiceFace), Effect.Duration);
            }
        }

        private static Color GetGlyphColor(int glyphSpellId) =>
            glyphSpellId switch
            {
                10 or 2033 => Color.FromArgb(201, 9, 20), // enflammé / enflammé doppeul
                17 => Color.FromArgb(180, 95, 4), // aggressif
                15 => Color.FromArgb(35, 123, 59), // Immobilisation
                12 or 2034 => Color.FromArgb(162, 110, 13), // Aveuglement / Aveuglement doppeul
                13 or 2035 => Color.FromArgb(8, 41, 138), // Silence / Silence doppeul
                367 or 949 => Color.White, // Cawotte / Kaskargo
                _ => Color.FromArgb(198, 9, 20),
            };
    }
}
