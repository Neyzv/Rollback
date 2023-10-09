using Rollback.Protocol.Enums;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Fights.Fighters;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Game.World.Maps;

namespace Rollback.World.Game.Fights.Teams
{
    public sealed class TaxCollectorAttackerTeam : Team
    {
        public override TeamType Type =>
            TeamType.Player;

        public TaxCollectorAttackerTeam(Cell[] placementCells) : base(placementCells) { }

        public override FighterRefusedReasonEnum CanJoin(Character character)
        {
            FighterRefusedReasonEnum result;

            if (OpposedTeam.GetFighters<TaxCollectorFighter>().FirstOrDefault() is { } taxCollectorFighter
                && taxCollectorFighter.TaxCollector.Guild.Id == character.GuildMember?.Guild.Id)
                result = FighterRefusedReasonEnum.WRONG_GUILD;
            else
                result = base.CanJoin(character);

            return result;
        }
    }
}
