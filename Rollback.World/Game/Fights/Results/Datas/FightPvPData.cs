using Rollback.Protocol.Types;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Fights.Results.Datas
{
    public sealed class FightPvPData : FightAdditionalDatas
    {
        public short Honor { get; set; }

        public short Dishonor { get; set; }

        public override FightResultAdditionalData FightResultAdditionalData =>
            throw new NotImplementedException();//new FightResultPvpData(Character.AlignmentGrade, ); TO DO

        public FightPvPData(Character character) : base(character)
        {
        }

        public override void Apply()
        {

        }
    }
}
