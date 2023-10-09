using Rollback.Protocol.Enums;
using Rollback.World.Database.Interactives;
using Rollback.World.Game.Interactions.Dialogs.Exchanges.Trades.Traders;
using Rollback.World.Game.Interactions.Dialogs.Exchanges.Trades.Types;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Interactives.Skills
{
    public sealed class CraftSkill : Skill
    {
        public CraftSkill(InteractiveObject interactive, InteractiveSkillRecord record)
            : base(interactive, record) { }

        public override bool CanBeUsed(Character character) =>
            ParentJobId.HasValue && character.HaveJob(ParentJobId.Value);

        public override bool BeforeExecute(Character character)
        {
            var result = true;

            if (!character.HaveJob(ParentJobId!.Value))
            {
                //Tu n\'exerces pas le métier nécessaire.
                character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 2);
                result = false;
            }

            return result;
        }

        public override void Execute(Character character) =>
            new CraftTrade(new KamasDisabledTrader(character), new EmptyTrader(), this).Open();
    }
}
