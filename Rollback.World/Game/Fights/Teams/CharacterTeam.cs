using Rollback.Protocol.Enums;
using Rollback.World.CustomEnums;
using Rollback.World.Game.World.Maps;

namespace Rollback.World.Game.Fights.Teams
{
    public sealed class CharacterTeam : Team
    {
        public override TeamType Type =>
            TeamType.Player;

        public CharacterTeam(Cell[] placementCells) : base(placementCells) { }

        public CharacterTeam(Cell[] placementCells, AlignmentSideEnum alignmentSide) : base(placementCells, alignmentSide) { }
    }
}
