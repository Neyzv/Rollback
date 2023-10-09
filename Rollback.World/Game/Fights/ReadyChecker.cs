using System.Collections.Concurrent;
using Rollback.World.Game.Fights.Fighters;
using Rollback.World.Handlers.Fights;

namespace Rollback.World.Game.Fights
{
    public sealed class ReadyChecker
    {
        /// <summary>
        /// Delay in ms before a fighter is declared as lagger.
        /// </summary>
        private const int LagTimeOut = 5000;

        private readonly Action _success;
        private readonly Action<CharacterFighter[]> _failure;
        private readonly ConcurrentDictionary<int, CharacterFighter> _laggers;
        private Timer? _timer;

        public bool Started =>
            _timer is not null;

        public ReadyChecker(Action success, Action<CharacterFighter[]> failure)
        {
            _success = success;
            _failure = failure;
            _laggers = new();
        }

        private void OnTimerElapsed(object? state)
        {
            Stop();
            _failure(_laggers.Values.ToArray());
        }

        private void Stop()
        {
            _timer?.Dispose();
            _timer = null;
        }

        public void Start(CharacterFighter[] fighters)
        {
            if (!Started)
            {
                _timer = new Timer(new(OnTimerElapsed), default, LagTimeOut, LagTimeOut);
                _laggers.Clear();

                if (fighters.Length is not 0)
                {

                    foreach (var fighter in fighters)
                    {
                        _laggers.TryAdd(fighter.Id, fighter);
                        FightHandler.SendGameFightTurnReadyRequestMessage(fighter.Character.Client, fighter.Team!.Fight!);
                    }
                }
                else
                    _success();
            }
        }

        public void ToggleReady(CharacterFighter fighter)
        {
            if (Started)
            {
                if (_laggers.TryRemove(fighter.Id, out _) && _laggers.Count is 0)
                {
                    Stop();
                    _success();
                }
            }
        }
    }
}
