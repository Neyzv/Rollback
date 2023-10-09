using System.Drawing;
using Rollback.Protocol.Enums;
using Rollback.Protocol.Types;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Fights.Fighters;
using Rollback.World.Game.Fights.Triggers;
using Rollback.World.Game.Spells;
using Rollback.World.Game.World.Maps;
using Rollback.World.Game.World.Maps.CellsZone;
using Rollback.World.Network;

namespace Rollback.World.Game.Fights
{
    public interface IFight
    {
        short Id { get; }

        FightTypeEnum Type { get; }

        bool CanCancelFight { get; }

        short AgeBonus { get; }

        MapInstance Map { get; }

        Team Challengers { get; }

        Team Defenders { get; }

        IReadOnlyCollection<FightActor> Winners { get; }

        IReadOnlyCollection<FightActor> Losers { get; }

        FightState State { get; }

        bool BladesVisible { get; }

        bool SpectatorClosed { get; }

        short RoundNumber { get; }

        bool Started { get; }

        int Duration { get; }

        bool IsPvP { get; }

        bool IsSequencing { get; }

        FightActor? FighterPlaying { get; }

        ReadyChecker Checker { get; }

        FightResultListEntry[] Results { get; }

        FightCommonInformations FightCommonInformations { get; }

        event Action<FightActor>? FighterAdded;
        void OnFighterAdded(FightActor fighter);

        event Action<FightActor>? FighterRemoved;
        void OnFighterRemoved(FightActor fighter);

        event Action<IFight>? FightEnded;

        event Action<IFight>? WinnersDeterminated;

        int GetFreeContextualId();

        void Send(Delegate d, object[]? parameters = default);

        void Send(Predicate<WorldClient> p, Delegate d, object[]? parameters = default);

        bool CanChangePlacementCell(FightActor fighter, short cellId);

        T? GetFighter<T>(Predicate<T>? p = default) where T : FightActor;

        T[] GetAllFighters<T>(Predicate<T>? p = default) where T : FightActor;

        List<FightActor> GetAlignedFighters(MapPoint startCell, short range, DirectionsEnum direction);

        bool IsCellFreeToWalkOn(short cellId);

        Cell? GetFirstFreeCellNear(MapPoint point, bool ignoreActor = false);

        bool CanSee(FightActor source, MapPoint target, HashSet<short>? additionalBlockedCells = default);

        void RemoveTrigger(TriggerMark trigger);

        void AddGlyph(TriggerType? triggerType, Zone zone, Color color, FightActor caster, Spell spell, short duration);

        void AddTrap(Zone zone, Color color, FightActor caster, Spell spell);

        void DecrementCastedGlyphs(FightActor caster);

        TTrigger[] GetTriggers<TTrigger>(Predicate<TTrigger>? p = default) where TTrigger : TriggerMark;

        TriggerMark[] GetAvailableTriggersFor(FightActor trigger, TriggerType type);

        void NotifyTriggers(TriggerMark[] triggers, FightActor target);

        int GetPlacementTimeLeft();

        void StartTimer(FightTimer timerType);

        void StartPlacement();

        void CheckAllStatus();

        FightSequence? StartSequence(SequenceTypeEnum sequence);

        void AcknowledgeSequence(CharacterFighter sender, int sequenceId);

        bool CheckFightEnd();

        void EndFight();
    }
}
