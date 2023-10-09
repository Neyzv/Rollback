using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.CustomEnums;
using Rollback.World.Database.Npcs;
using Rollback.World.Game.Interactions.Dialogs.Exchanges.Trades.Traders;
using Rollback.World.Game.Interactions.Dialogs.Exchanges.Trades.Types;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.RolePlayActors.Npcs.Actions
{
    [Identifier("Chanil")]
    internal class ChanilAction : NpcAction
    {
        public override NpcActionType NpcActionType =>
            NpcActionType.DropCollectPet;

        public ChanilAction(NpcActionRecord record)
            : base(record) { }

        public override void Execute(Npc npc, Character character) =>
            new ChanilTrade(new PlayerTrader(character), new NpcTrader(npc)).Open();
    }
}
