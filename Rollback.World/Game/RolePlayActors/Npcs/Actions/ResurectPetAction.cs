using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.CustomEnums;
using Rollback.World.Database.Npcs;
using Rollback.World.Game.Interactions.Dialogs.Exchanges.Trades.Traders;
using Rollback.World.Game.Interactions.Dialogs.Exchanges.Trades.Types;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.RolePlayActors.Npcs.Actions
{
    [Identifier("ResurectPet")]
    public sealed class ResurectPetAction : NpcAction
    {
        public override NpcActionType NpcActionType =>
            NpcActionType.ResurrectPet;

        public ResurectPetAction(NpcActionRecord record)
            : base(record) { }

        public override void Execute(Npc npc, Character character) =>
            new PetResurrectionTrade(new KamasDisabledTrader(character), new NpcTrader(npc)).Open();
    }
}
