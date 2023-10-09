using Rollback.Protocol.Enums;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Commands.Commands.Beta
{
    public sealed class SpellMaxCommand : InGameCommand
    {
        public override string[] Aliases =>
            new[] { "spellmax" };

        public override string Description =>
            "This command upgrade all your spells to the max level";

        public override byte Role =>
            (byte)GameHierarchyEnum.PLAYER;

        protected override void Execute(Character sender)
        {
            foreach (var spell in sender.GetSpells())
                while (spell.Upgrade()) { }

            sender.RefreshSpells();
        }
    }
}
