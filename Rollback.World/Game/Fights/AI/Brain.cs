namespace Rollback.World.Game.Fights.AI
{
    public class Brain
    {
        protected readonly AIFighter _fighter;

        public Brain(AIFighter fighter) =>
            _fighter = fighter;

        public void Play()
        {
            var target = _fighter.Team!.OpposedTeam.GetFighters<FightActor>(x => x.Alive).OrderBy(x => x.Cell.Point.ManhattanDistanceTo(_fighter.Cell.Point)).FirstOrDefault();
            if (target != null)
            {
                new Actions.Movements.MoveNearAction(_fighter, target.Cell).Apply();

                foreach (var spell in _fighter.GetSpells())
                    _fighter.CastSpell(spell, target.Cell);
            }

            _fighter.EndTurn();
        }
    }
}
