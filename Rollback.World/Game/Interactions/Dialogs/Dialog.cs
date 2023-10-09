using Rollback.World.CustomEnums;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Interactions.Dialogs
{
    public abstract class Dialog : IInteraction
    {
        public Character Character { get; }

        public abstract DialogType DialogType { get; }

        public Dialog(Character character) =>
            Character = character;

        protected abstract void InternalOpen();

        public void Open()
        {
            Character.Interaction = this;
            InternalOpen();
        }

        protected virtual void InternalClose() { }

        public void Close()
        {
            InternalClose();

            if (Character.Interaction == this)
                Character.Interaction = null;

            Character.OnLeaveInteraction(this);
        }
    }

    public abstract class Dialog<T> : Dialog
        where T : class
    {
        public T Dialoger { get; }

        public Dialog(Character character, T dialoger) : base(character) =>
            Dialoger = dialoger;
    }
}
