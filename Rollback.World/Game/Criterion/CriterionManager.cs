using System.Reflection;
using System.Text.RegularExpressions;
using Rollback.Common.DesignPattern.Assemblies;
using Rollback.Common.DesignPattern.Attributes;
using Rollback.Common.DesignPattern.Instance;
using Rollback.Common.Initialization;
using Rollback.Common.Logging;
using Rollback.World.Game.Criterion.Enums;

namespace Rollback.World.Game.Criterion
{
    public sealed class CriterionManager : Singleton<CriterionManager>
    {
        private const string ExpressionLabel = "expression";
        private const string OperatorLabel = "operator";
        private const string IdentifierLabel = "identifier";
        private const string ComparatorLabel = "comparator";
        private const string ValueLabel = "value";

        private readonly Dictionary<string, Func<string, Comparator, string, Operator, BaseCriteria>> _criterions;

        private static readonly Regex _splitRegex;
        private static readonly Regex _splitExpressionRegex;

        private static readonly Dictionary<string, Comparator> _charToComparator = new()
        {
            ["="] = Comparator.Equal,
            ["!"] = Comparator.Inequal,
            [">"] = Comparator.Superior,
            ["<"] = Comparator.Inferior,
            ["~"] = Comparator.Like,
            ["s"] = Comparator.StartWith,
            ["S"] = Comparator.StartWithLike,
            ["e"] = Comparator.EndWith,
            ["E"] = Comparator.EndWithLike,
            ["i"] = Comparator.Invalid,
            ["v"] = Comparator.Valid,
            ["#"] = Comparator.Unknown1,
            ["/"] = Comparator.Unknown2,
            ["X"] = Comparator.Unknown3,
        };

        private static readonly Dictionary<string, Operator> _charToOperator = new()
        {
            ["&"] = Operator.And,
            ["|"] = Operator.Or,
        };

        static CriterionManager()
        {
            _splitRegex = new($"(?<{ExpressionLabel}>\\(.+?\\)(?![^()]*(?:\\([^()]*\\)[^()]*\\))|(?:[^()]*\\)))|.+?)(?:(?<{OperatorLabel}>[{string.Join(default(string?), _charToOperator.Keys)}])|$)", RegexOptions.Compiled);
            _splitExpressionRegex = new($"^(?<{IdentifierLabel}>.{{2,3}}?)(?<{ComparatorLabel}>[{string.Join(default(string?), _charToComparator.Keys)}])(?<{ValueLabel}>.+)$", RegexOptions.Compiled);
        }

        public CriterionManager() =>
            _criterions = new();

        [Initializable(InitializationPriority.DatasManager, "Criterion")]
        public void Initialize()
        {
            var baseCriteriaType = typeof(BaseCriteria);

            foreach (var (type, attributes) in from assembly in AssemblyManager.Instance.Assemblies
                                               from type in assembly.GetTypes()
                                               let attributes = type.GetCustomAttributes<IdentifierAttribute>()
                                               where attributes.Any() && !type.IsAbstract && type.IsClass && type.IsSubclassOf(baseCriteriaType)
                                               select (type, attributes))
            {
                var constructor = type.GetConstructor(new[] { typeof(string), typeof(Comparator), typeof(string), typeof(Operator) });

                if (constructor is not null)
                {
                    var criterionFactory = (string identifier, Comparator comparator, string value, Operator op) =>
                                    (BaseCriteria)constructor.Invoke(new object[] { identifier, comparator, value, op! });

                    foreach (var attribute in attributes)
                    {
                        if (attribute.Identifier is string identifier)
                            if (!_criterions.TryAdd(identifier, criterionFactory))
                                Logger.Instance.LogError(msg: $"Found two criterions with alias {identifier}");
                    }
                }
                else
                    Logger.Instance.LogError(msg: $"Can not find a valid constructor for type {type.Name}...");
            }
        }

        private BaseCriteria CreateCriteria(string identifier, Comparator comparator, string value, Operator op)
        {
            if (!_criterions.TryGetValue(identifier, out var criteriaFactory))
                Logger.Instance.LogWarn($"Unknown criteria with identifier {identifier}, it's not implemented.");

            return criteriaFactory is not null ? criteriaFactory(identifier, comparator, value, op) : new(identifier, comparator, value, op);
        }

        private CriterionExpression? Parse(string input, Operator resOpe = Operator.None)
        {
            var criterions = new List<Criteria>();
            var matches = _splitRegex.Matches(input);

            for (var i = 0; i < matches.Count; i++)
            {
                if (matches[i].Groups.ContainsKey(ExpressionLabel) && matches[i].Groups.ContainsKey(OperatorLabel))
                {
                    if (matches[i].Groups[ExpressionLabel].Value != string.Empty &&
                        matches[i].Groups[OperatorLabel].Value != string.Empty || i == matches.Count - 1)
                    {
                        Operator op = Operator.None;

                        if (matches[i].Groups[OperatorLabel].Value != string.Empty &&
                            !_charToOperator.TryGetValue(matches[i].Groups[OperatorLabel].Value, out op))
                            Logger.Instance.LogError(msg: "Regex pattern error...");

                        if (matches[i].Groups[ExpressionLabel].Value[0] is '(' || matches[i].Groups[ExpressionLabel].Value[^1] is ')')
                        {
                            if (matches[i].Groups[ExpressionLabel].Value[0] is not '(' || matches[i].Groups[ExpressionLabel].Value[^1] is not ')')
                                Logger.Instance.LogError(msg: "Malformed criterion expression, missing an opened or closed parantheses...");

                            criterions.Add(Parse(matches[i].Groups[ExpressionLabel].Value[1..^1], op)!);
                        }
                        else
                        {
                            var splittedExpression = _splitExpressionRegex.Match(matches[i].Groups[ExpressionLabel].Value);

                            if (!splittedExpression.Groups.ContainsKey(IdentifierLabel) ||
                                !splittedExpression.Groups.ContainsKey(ComparatorLabel) ||
                                !splittedExpression.Groups.ContainsKey(ValueLabel))
                                Logger.Instance.LogError(msg: "Regex pattern error, missing some group of capture for sub splitting...");
                            else
                            {
                                if (!_charToComparator.TryGetValue(splittedExpression.Groups[ComparatorLabel].Value, out var comparator))
                                    Logger.Instance.LogError(msg: "Regex pattern error...");

                                criterions.Add(CreateCriteria(splittedExpression.Groups[IdentifierLabel].Value, comparator,
                                    splittedExpression.Groups[ValueLabel]!.Value, op));
                            }
                        }
                    }
                    else
                        Logger.Instance.LogError(msg: "Malformed criterion expression...");
                }
                else
                    Logger.Instance.LogError(msg: "Regex pattern error, missing some group of capture...");
            }

            return criterions.Count > 0 ? new(criterions, resOpe) : default;
        }

        public CriterionExpression? Parse(string input) =>
            Parse(input, default);
    }
}
