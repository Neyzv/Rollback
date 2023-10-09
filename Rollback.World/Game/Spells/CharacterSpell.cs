using Rollback.Common.ORM;
using Rollback.Protocol.Types;
using Rollback.World.Database.Characters;
using Rollback.World.Database.Spells;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Spells
{
    public sealed class CharacterSpell : Spell
    {
        private readonly CharacterSpellRecord _record;
        private readonly Character _owner;

        /// <summary>
        /// 63 is non equiped / more is for shortcut
        /// </summary>
        public byte Position
        {
            get => _record.Position;
            set => _record.Position = value;
        }

        public SpellItem SpellItem =>
            new(Position, Id, Level);

        public CharacterSpell(SpellTemplateRecord template, sbyte level, Character owner, byte position)
            : base(template, level)
        {
            _owner = owner;
            _record = new()
            {
                OwnerId = _owner.Id,
                Position = position,
                SpellId = template.Id,
                SpellLevel = level
            };
        }

        public CharacterSpell(CharacterSpellRecord record, Character owner) :
            base(SpellManager.Instance.GetSpellTemplateById(record.SpellId)!, record.SpellLevel) =>
            (_record, _owner) = (record, owner);

        public override bool Upgrade() =>
            base.Upgrade() && (MinPlayerLevel <= _owner.Level || !DownGrade());

        public void Save()
        {
            _record.SpellLevel = Level;
            DatabaseAccessor.Instance.InsertOrUpdate(_record);
        }

        public void Delete() =>
            DatabaseAccessor.Instance.Delete(_record);
    }
}
