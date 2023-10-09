using Rollback.World.Game.Fights;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Handlers.Fights;

namespace Rollback.World.Game.Interactions.Requests.Fights
{
    public sealed class FightRequest : Request
    {
        public FightRequest(Character sender, Character receiver) : base(sender, receiver) { }


        protected override void InternalOpen()
        {
            FightHandler.SendGameRolePlayPlayerFightFriendlyRequestedMessage(Sender.Client, Sender, Receiver);
            FightHandler.SendGameRolePlayPlayerFightFriendlyRequestedMessage(Receiver.Client, Sender, Receiver);
        }

        protected override void InternalAccept()
        {
            if (Sender.MapInstance == Receiver.MapInstance)
            {
                FightHandler.SendGameRolePlayPlayerFightFriendlyAnsweredMessage(Sender.Client, Sender, Receiver, true);
                FightManager.CreateDuel(Sender.MapInstance, Sender, Receiver)?.StartPlacement();
            }
            else
                FightHandler.SendGameRolePlayPlayerFightFriendlyAnsweredMessage(Sender.Client, Sender, Receiver, false);
        }

        protected override void InternalClose()
        {
            FightHandler.SendGameRolePlayPlayerFightFriendlyAnsweredMessage(Sender.Client, Sender, Receiver, false);
            FightHandler.SendGameRolePlayPlayerFightFriendlyAnsweredMessage(Receiver.Client, Sender, Receiver, false);
        }
    }
}
