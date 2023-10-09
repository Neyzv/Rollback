using Rollback.World.CustomEnums;
using Rollback.World.Handlers.Fights;

namespace Rollback.World.Game.Fights
{
    public sealed class TimeLine
    {
        private readonly IFight _fight;
        private short _pointer;
        private FightActor[]? _actors;

        public short RoundNumber { get; private set; }

        public TimeLine(IFight fight)
        {
            _fight = fight;
            _pointer = -1;
            RoundNumber = 1;
        }

        public void SortFighters()
        {
            var challengers = _fight.Challengers.GetFighters<FightActor>(x => x.Alive).ToArray();
            var defenders = _fight.Defenders.GetFighters<FightActor>(x => x.Alive).ToArray();

            _actors = new FightActor[challengers.Length + defenders.Length];

            var challInitiative = challengers.Sum(x => x.Stats[Stat.Initiative].Total);
            var defInitiative = defenders.Sum(x => x.Stats[Stat.Initiative].Total);

            var team = challInitiative >= defInitiative ? challengers : defenders;

            for (var i = 0; i < _actors.Length; i++)
            {
                var availableTeamFighters = team.Where(x => !_actors.Contains(x)).ToArray();
                if (availableTeamFighters.Length is 0)
                    availableTeamFighters = (team == challengers ? defenders : challengers).Where(x => !_actors.Contains(x)).ToArray();

                _actors[i] = availableTeamFighters.First(x => availableTeamFighters.All(y => y.Stats[Stat.Initiative].Total <= x.Stats[Stat.Initiative].Total));

                team = team == challengers ? defenders : challengers;
            }
        }

        public void RemoveFighter(FightActor fighter)
        {
            if (_actors!.Contains(fighter))
            {
                var newTimeLine = new FightActor[_actors!.Length - 1];

                var i = 0;
                foreach (var actor in _actors)
                    if (actor.Id != fighter.Id)
                        newTimeLine[i++] = actor;

                _actors = newTimeLine;
            }
        }

        public void AddSummon(SummonedFighter summon)
        {
            var fighterAdded = false;
            var newTimeLine = new FightActor[_actors!.Length + 1];

            var i = 0;
            foreach (var actor in _actors)
            {
                newTimeLine[i++] = actor;

                if (summon.Summoner.Id == actor.Id)
                {
                    newTimeLine[i++] = summon;
                    fighterAdded = true;
                }
            }

            if (!fighterAdded)
                newTimeLine[^1] = summon;

            _actors = newTimeLine;
        }

        public FightActor GetNextFighter()
        {
            FightActor fighter;

            do
            {
                _pointer++;

                if (_pointer >= _actors!.Length)
                {
                    _pointer = 0;
                    RoundNumber++;
                }

                fighter = _actors[_pointer];
            }
            while (!fighter!.Alive);

            return fighter;
        }

        public void Refresh() =>
            _fight.Send(FightHandler.SendGameFightTurnListMessage, new object[] { _actors! });
    }
}
