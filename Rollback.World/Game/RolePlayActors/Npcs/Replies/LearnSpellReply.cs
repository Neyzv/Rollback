using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.Database.Npcs;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.RolePlayActors.Npcs.Replies
{
    [Identifier("LearnSpell")]
    public sealed class LearnSpellReply : NpcReply
    {
        private short? _spellId;
        public short SpellId =>
            _spellId ??= GetParameterValue<short>(0);

        private sbyte? _level;
        public sbyte Level =>
            _level ??= GetParameterValue<sbyte?>(1) ?? 1;

        public LearnSpellReply(NpcReplyRecord record)
            : base(record) { }

        public override bool Execute(Npc npc, Character character)
        {
            character.LearnSpell(SpellId, Level);

            return true;
        }
    }
}
