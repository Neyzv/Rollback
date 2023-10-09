using Rollback.Common.DesignPattern.Attributes;
using Rollback.Common.Extensions;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Criterion.Enums;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Criterion.Types
{
    [Identifier("CA"), Identifier("CC"), Identifier("CS"), Identifier("CI"), Identifier("CW"), Identifier("CV"), Identifier("CM"), Identifier("CP"),
        Identifier("Ca"), Identifier("Cc"), Identifier("Cs"), Identifier("Ci"), Identifier("Cw"), Identifier("Cv")]
    public sealed class StatCriteria : BaseCriteria
    {
        private static readonly Dictionary<string, Stat?> _criterionStatRelation = new()
        {
            ["CA"] = Stat.Agility,
            ["CC"] = Stat.Chance,
            ["CS"] = Stat.Strength,
            ["CI"] = Stat.Intelligence,
            ["CW"] = Stat.Wisdom,
            ["CV"] = Stat.Vitality,
            ["CM"] = Stat.MovementPoints,
            ["CP"] = Stat.ActionPoints,
        };

        private static readonly Dictionary<string, Stat?> _criterionBaseStatRelation = new()
        {
            ["Ca"] = Stat.Agility,
            ["Cc"] = Stat.Chance,
            ["Cs"] = Stat.Strength,
            ["Ci"] = Stat.Intelligence,
            ["Cw"] = Stat.Wisdom,
            ["Cv"] = Stat.Vitality,
        };

        private short? _statAmount;
        public short StatAmount =>
            _statAmount ??= Value.ChangeType<short>();

        public StatCriteria(string identifier, Comparator comparator, string value, Operator op) : base(identifier, comparator, value, op) { }

        public override bool Eval(Character character)
        {
            if (!_criterionStatRelation.TryGetValue(Identifier, out var stat))
                _criterionBaseStatRelation.TryGetValue(Identifier, out stat);
            else
                return Compare(character.Stats[stat!.Value].Total, StatAmount);

            if (stat is null)
                character.ReplyError($"Can not find a stat associated to identifier {Identifier} for stat criteria...");

            return stat is not null && Compare(character.Stats[stat.Value].Additional, StatAmount);
        }
    }
}
