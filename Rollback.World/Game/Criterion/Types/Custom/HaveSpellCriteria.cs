using Rollback.Common.DesignPattern.Attributes;
using Rollback.Common.Extensions;
using Rollback.World.Game.Criterion.Enums;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Criterion.Types.Custom;

[Identifier("HS")]
public sealed class HaveSpellCriteria : BaseCriteria
{
    private short? _spellId;
    private short SpellId =>
        _spellId ??= Value.ChangeType<short>();

    public HaveSpellCriteria(string identifier, Comparator comparator, string value, Operator op)
        : base(identifier, comparator, value, op) { }

    public override bool Eval(Character character)
    {
        var spell = character.GetSpell(SpellId);

        return Comparator is Comparator.Equal ? spell is not null : spell is null;
    }
}
