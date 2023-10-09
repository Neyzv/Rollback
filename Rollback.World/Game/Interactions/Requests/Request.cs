using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Interactions.Requests
{
    public abstract class Request : IInteraction
    {
        public Character Sender { get; }

        public Character Receiver { get; }

        public Request(Character sender, Character receiver)
        {
            Sender = sender;
            Receiver = receiver;
        }

        protected abstract void InternalOpen();

        public void Open()
        {
            Sender.Interaction = this;
            Receiver.Interaction = this;
            InternalOpen();
        }

        protected abstract void InternalAccept();

        public void Accept()
        {
            Sender.Interaction = default;
            Sender.OnLeaveInteraction(this);

            Receiver.Interaction = default;
            Receiver.OnLeaveInteraction(this);

            InternalAccept();
        }

        protected abstract void InternalClose();

        public void Close()
        {
            InternalClose();

            if (Sender.Interaction == this)
                Sender.Interaction = default;

            Sender.OnLeaveInteraction(this);

            if (Receiver.Interaction == this)
                Receiver.Interaction = default;

            Receiver.OnLeaveInteraction(this);
        }
    }
}
