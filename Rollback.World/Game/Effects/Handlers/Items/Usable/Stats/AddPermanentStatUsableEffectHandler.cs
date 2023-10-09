using Rollback.Common.DesignPattern.Attributes;
using Rollback.Protocol.Enums;
using Rollback.World.CustomEnums;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Game.World.Maps;

namespace Rollback.World.Game.Effects.Handlers.Items.Usable.Stats
{
    [Identifier(EffectId.EffectAddPermanentAgility), Identifier(EffectId.EffectAddPermanentChance), Identifier(EffectId.EffectAddPermanentIntelligence),
        Identifier(EffectId.EffectAddPermanentStrength), Identifier(EffectId.EffectAddPermanentVitality), Identifier(EffectId.EffectAddPermanentWisdom)]
    public sealed class AddPermanentStatUsableEffectHandler : UsableEffectHandler
    {
        public const short Limit = 101;

        public AddPermanentStatUsableEffectHandler(EffectBase effect, Character itemOwner, Cell targetedCell) : base(effect, itemOwner, targetedCell) { }

        private static (Stat, short)? GetEffectStatAndMessageId(EffectId effect) =>
            effect switch
            {
                EffectId.EffectAddPermanentAgility => (Stat.Agility, 12),
                EffectId.EffectAddPermanentChance => (Stat.Chance, 11),
                EffectId.EffectAddPermanentIntelligence => (Stat.Intelligence, 14),
                EffectId.EffectAddPermanentStrength => (Stat.Strength, 10),
                EffectId.EffectAddPermanentVitality => (Stat.Vitality, 13),
                EffectId.EffectAddPermanentWisdom => (Stat.Wisdom, 9),
                _ => default
            };

        public override void Apply()
        {
            if (GenerateEffect() is { } effectInteger && GetTarget() is { } target && GetEffectStatAndMessageId(effectInteger.Id) is { } statInfos)
            {
                var amountAdded = effectInteger.Value;
                target.Stats[statInfos.Item1].Additional += amountAdded;

                if (target.Stats[statInfos.Item1].Additional > Limit)
                {
                    amountAdded -= (short)(target.Stats[statInfos.Item1].Additional - Limit);
                    target.Stats[statInfos.Item1].Additional = Limit;
                }

                /*
                9 => "Tu as gagné <b>%1</b> points de sagesse."
                10 => "Tu as gagné <b>%1</b> points de force."
                11 => "Tu as gagné <b>%1</b> points de chance."
                12 => "Tu as gagné <b>%1</b> points d\'agilité."
                13 => "Tu as gagné <b>%1</b> points de vitalité."
                14 => "Tu as gagné <b>%1</b> points d\' intelligence."
                */
                target.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, statInfos.Item2, amountAdded);

                target.RefreshStats();
            }
        }
    }
}
