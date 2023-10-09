using Rollback.Common.DesignPattern.Attributes;
using Rollback.Common.Extensions;
using Rollback.World.Game.Criterion.Enums;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Game.World;

namespace Rollback.World.Game.Criterion.Types.Custom
{
    [Identifier("CO")]
    public sealed class CellOccupedCriteria : BaseCriteria
    {
        private string? _infos;

        public string? Infos =>
            _infos ??= Value.ChangeType<string>();

        public CellOccupedCriteria(string identifier, Comparator comparator, string value, Operator op) : base(identifier, comparator, value, op) { }

        public override bool Eval(Character character)
        {
            var result = false;

            if (Infos is not null && Infos.Split(',') is { } infosSplitted && !string.IsNullOrEmpty(infosSplitted.First()))
            {
                int mapId = -1;
                short cellId = -1;

                if (infosSplitted.Length > 1 ? int.TryParse(infosSplitted[0], out mapId) && short.TryParse(infosSplitted[1], out cellId) :
                    short.TryParse(infosSplitted[0], out cellId))
                {
                    var mapToTest = mapId >= 0 && WorldManager.Instance.GetMapById(mapId) is { } map ? map : character.MapInstance.Map;

                    result = mapToTest.GetInstances().Any(x => Comparator is Comparator.Equal ? x.GetActor<Character>(x => x.Cell.Id == cellId) is not null :
                        x.GetActors<Character>(x => x.Cell.Id == cellId).Length is 0);
                }
            }

            return result;
        }
    }
}
