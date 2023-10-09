using System.Drawing;
using Rollback.Protocol.Enums;
using Rollback.Protocol.Types;
using Rollback.World.CustomEnums;
using Rollback.World.Game.World.Maps.CellsZone;
using Rollback.World.Game.World.Maps.CellsZone.Shapes;
using Rollback.World.Handlers.Fights;

namespace Rollback.World.Game.Fights.Triggers
{
    public abstract class TriggerMark
    {
        public short Id { get; }

        public TriggerType? TriggerType { get; set; }

        public GameActionFightInvisibilityStateEnum Visibility { get; protected set; }

        public Zone Zone { get; }

        public Color Color { get; }

        public FightActor Caster { get; }

        public GameActionMarkedCell[] GameActionMarkedCell
        {
            get
            {
                if (Zone is Lozenge)
                    return new[] { new GameActionMarkedCell(Zone.CenterCell.Id, (sbyte)Zone.Radius, Color.ToArgb()) };

                var argbColor = Color.ToArgb();
                return Zone.AffectedCells.Values.Select(x => new GameActionMarkedCell(x.Id, 0, argbColor)).ToArray();
            }
        }

        public abstract GameActionMarkTypeEnum Type { get; }

        protected TriggerMark(short id, TriggerType? triggerType, Zone zone, Color color, FightActor caster)
        {
            Id = id;
            TriggerType = triggerType;
            Zone = zone;
            Color = color;
            Caster = caster;
            Visibility = GameActionFightInvisibilityStateEnum.VISIBLE;
        }

        public bool VisibleFor(FightActor fighter) =>
            Visibility is not GameActionFightInvisibilityStateEnum.INVISIBLE || fighter.IsFriendlyWith(Caster);

        public void ChangeVisibility(bool visible)
        {
            if (visible && Visibility is not GameActionFightInvisibilityStateEnum.VISIBLE)
            {
                Visibility = GameActionFightInvisibilityStateEnum.VISIBLE;
                Caster.Team!.Fight.Send(x => VisibleFor(x.Account!.Character!.Fighter!), FightHandler.SendGameActionFightMarkCellsMessage, new object[] { this });
            }
            else if (!visible && Visibility is GameActionFightInvisibilityStateEnum.VISIBLE)
            {
                Visibility = GameActionFightInvisibilityStateEnum.INVISIBLE;
                Caster.Team!.Fight.Send(x => !VisibleFor(x.Account!.Character!.Fighter!), FightHandler.SendGameActionFightUnmarkCellsMessage, new object[] { this });
            }
        }

        public abstract SpellCast Trigger(FightActor target);
    }
}
