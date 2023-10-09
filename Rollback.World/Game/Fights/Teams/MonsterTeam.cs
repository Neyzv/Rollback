using Rollback.Protocol.Enums;
using Rollback.World.CustomEnums;
using Rollback.World.Game.World.Maps;

namespace Rollback.World.Game.Fights.Teams
{
    public sealed class MonsterTeam : Team
    {
        public override TeamType Type =>
            TeamType.Monster;

        public int? GroupId { get; set; }

        public int? DungeonId { get; set; }

        public MonsterTeam(Cell[] placementCells) : base(placementCells) { }

        public MonsterTeam(Cell[] placementCells, AlignmentSideEnum alignmentSide) : base(placementCells, alignmentSide) { }
    }
}
