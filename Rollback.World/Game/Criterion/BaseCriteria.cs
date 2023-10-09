using Rollback.World.Game.Criterion.Enums;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Criterion
{
    public class BaseCriteria : Criteria
    {
        public string Identifier { get; }

        public Comparator Comparator { get; }

        public string Value { get; }

        public BaseCriteria(string identifier, Comparator comparator, string value, Operator op) : base(op)
        {
            Identifier = identifier;
            Comparator = comparator;
            Value = value;
        }

        protected bool Compare<T>(T obj, T comparand)
            where T : IComparable
        {
            var comparaison = obj.CompareTo(comparand);

            return Comparator switch
            {
                Comparator.Equal => comparaison is 0,
                Comparator.Inequal => comparaison is not 0,
                Comparator.Inferior => comparaison < 0,
                Comparator.Superior => comparaison > 0,
                _ => true
            };
        }

        protected bool Compare(string str, string comparand) =>
            Comparator switch
            {
                Comparator.Equal => str == comparand,
                Comparator.Inequal => str != comparand,
                Comparator.Like => str.Equals(comparand, StringComparison.InvariantCultureIgnoreCase),
                Comparator.StartWith => str.StartsWith(comparand),
                Comparator.StartWithLike => str.StartsWith(comparand, StringComparison.InvariantCultureIgnoreCase),
                Comparator.EndWith => str.EndsWith(comparand),
                Comparator.EndWithLike => str.EndsWith(comparand, StringComparison.InvariantCultureIgnoreCase),
                _ => true
            };

        public override bool Eval(Character character) =>
            true;
    }
}
