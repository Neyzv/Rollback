using Rollback.Protocol.Enums;
using Rollback.World.Game.Fights.Fighters;
using Rollback.World.Handlers.Fights;

namespace Rollback.World.Game.Fights
{
    public sealed class FightSequence : IDisposable
    {
        private bool _acknowledged;

        public int Id { get; }

        public SequenceTypeEnum Sequence { get; }

        public FightActor Sender { get; }

        public bool Disposed { get; private set; }

        public FightSequence(int id, SequenceTypeEnum sequence, FightActor sender)
        {
            Id = id;
            Sequence = sequence;
            Sender = sender;
        }

        public void Acknowledge(CharacterFighter sender)
        {
            if (!_acknowledged && sender.Id == Sender.Id)
            {
                // TO DO Antibot

                _acknowledged = true;
            }
        }

        public void Dispose()
        {
            if (!Disposed)
            {
                if (!_acknowledged)
                    Sender.Team!.Fight.Send(FightHandler.SendSequenceEndMessage, new object[] { this });
                else if (Sender is CharacterFighter characterFighter)
                    characterFighter.Character.Client.SafeBotBan();

                Disposed = true;

                GC.SuppressFinalize(this);
            }
        }
    }
}
