using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.Game.Criterion.Enums;
using Rollback.World.Game.Effects.Types;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Criterion.Types.Custom
{
    [Identifier("IDW")]
    public sealed class ItemDateWaitCriteria : BaseCriteria
    {
        private TimeSpan? _time;
        private short? _itemId;

        public ItemDateWaitCriteria(string identifier, Comparator comparator, string value, Operator op)
            : base(identifier, comparator, value, op) { }

        public override bool Eval(Character character)
        {
            if (_itemId is null || _time is null)
            {
                var valueSplitted = Value.Split(',');
                if (valueSplitted.Length > 1)
                {
                    if (short.TryParse(valueSplitted[0], out var itemId) && long.TryParse(valueSplitted[1], out var secondsToWait))
                    {
                        _itemId = itemId;
                        _time = TimeSpan.FromSeconds(secondsToWait);
                    }
                    else
                        character.ReplyError($"Can not parse value of {nameof(ItemDateWaitCriteria)}...");
                }
                else
                    character.ReplyError($"Not enough parameters in value of {nameof(ItemDateWaitCriteria)}...");
            }

            var now = DateTime.Now;

            var items = character.Inventory.GetItems(x => x.Id == _itemId);

            return _itemId.HasValue && _time.HasValue && (items.Length is 0 || items.Any(x =>
                    x.Effects.OfType<EffectDate>().Any(effect => effect.Date + _time <= now)
                )) == (Comparator is Comparator.Equal);
        }
    }
}
