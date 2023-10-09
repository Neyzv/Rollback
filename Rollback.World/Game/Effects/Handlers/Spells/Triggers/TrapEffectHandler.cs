using System.Drawing;
using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Effects.Types;
using Rollback.World.Game.Fights;
using Rollback.World.Game.Fights.Fighters;
using Rollback.World.Game.Spells;
using Rollback.World.Game.World.Maps.CellsZone;

namespace Rollback.World.Game.Effects.Handlers.Spells.Triggers
{
    [Identifier(EffectId.EffectTrap)]
    public sealed class TrapEffectHandler : SpellEffectHandler
    {
        public TrapEffectHandler(EffectBase effect, List<FightActor> target, SpellCast cast, Zone zone) : base(effect, target, cast, zone) =>
            Target = new() { Cast.Caster };

        protected override void InternalApply(FightActor fighter)
        {
            if (Effect is EffectDice effectDice && Zone is not null)
            {
                var trapSpellTemplate = SpellManager.Instance.GetSpellTemplateById(effectDice.DiceNum);

                if (trapSpellTemplate is not null && effectDice.DiceFace > 0 && trapSpellTemplate.SpellLevels.Length >= effectDice.DiceFace)
                    fighter.Team!.Fight.AddTrap(Zone, GetTrapColor(Cast.Spell.Id), fighter, new Spell(trapSpellTemplate, (sbyte)effectDice.DiceFace));
            }
        }

        public override bool CanSeeCast(CharacterFighter fighter) =>
            fighter.IsFriendlyWith(Cast.Caster);

        private static Color GetTrapColor(int trapSpellId) =>
            trapSpellId switch
            {
                65 or 79 or 2072 => Color.FromArgb(153, 101, 70), // Sournois / Masse / Masse doppeul
                80 => Color.FromArgb(102, 67, 46), // Mortel
                71 or 2068 => Color.FromArgb(13, 184, 104), // Empoisonné / Empoirsonné doppeul
                73 => Color.White, // Répulsif
                77 or 2071 => Color.FromArgb(89, 57, 128), // Silence / Silence doppeul
                69 => Color.Cyan, // Imobilisation
                _ => Color.Brown,
            };
    }
}
