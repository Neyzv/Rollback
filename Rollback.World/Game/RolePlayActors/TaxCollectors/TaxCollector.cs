using System.Drawing;
using Rollback.Common.Extensions;
using Rollback.Common.ORM;
using Rollback.Protocol.Types;
using Rollback.World.CustomEnums;
using Rollback.World.Database.TaxCollectors;
using Rollback.World.Game.Fights;
using Rollback.World.Game.Fights.Fighters;
using Rollback.World.Game.Fights.Types;
using Rollback.World.Game.Guilds;
using Rollback.World.Game.Items.Storages;
using Rollback.World.Game.Looks;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Game.Spells;
using Rollback.World.Game.World.Maps;
using GuildMember = Rollback.World.Game.Guilds.GuildMember;

namespace Rollback.World.Game.RolePlayActors.TaxCollectors
{
    public sealed class TaxCollector
    {
        private readonly TaxCollectorRecord _record;
        private readonly ReaderWriterLockSlim _lock;

        public int Id =>
            _record.Id;

        public short FirstNameId =>
            _record.FirstNameId;

        public short LastNameId =>
            _record.LastNameId;

        public int MapId =>
            _record.MapId;

        public GuildMember Hirer =>
            _record.Hirer!;

        public int GatheredExperience
        {
            get
            {
                _lock.EnterReadLock();
                var result = _record.GatheredExperience;
                _lock.ExitReadLock();

                return result;
            }
            private set
            {
                _lock.EnterWriteLock();
                _record.GatheredExperience = value;
                _lock.ExitWriteLock();
            }
        }

        public int GatheredKamas
        {
            get
            {
                _lock.EnterReadLock();
                var result = _record.GatheredKamas;
                _lock.ExitReadLock();

                return result;
            }
            private set
            {
                _lock.EnterWriteLock();
                _record.GatheredKamas = value;
                _lock.ExitWriteLock();
            }
        }

        public Map Map { get; }

        public Guild Guild =>
            _record.Hirer!.Guild;

        private ActorLook? _look;
        public ActorLook Look
        {
            get
            {
                if (_look is null)
                    RefreshLook();

                return _look!;
            }
        }

        public TaxCollectorBag Bag { get; }

        private Character? _dialoger;
        public Character? Dialoger
        {
            get
            {
                _lock.EnterReadLock();
                var result = _dialoger;
                _lock.ExitReadLock();

                return result;
            }
            set
            {
                _lock.EnterWriteLock();
                _dialoger = value;
                _lock.ExitWriteLock();
            }
        }

        private TaxCollectorFighter? _fighter;
        public TaxCollectorFighter? Fighter
        {
            get
            {
                _lock.EnterReadLock();
                var result = _fighter;
                _lock.ExitReadLock();

                return result;
            }
            private set
            {
                _lock.EnterWriteLock();
                _fighter = value;
                _lock.ExitWriteLock();
            }
        }

        public bool IsBusy =>
            Fighter is not null || Dialoger is not null;

        public AdditionalTaxCollectorInformations AdditionalTaxCollectorInformations =>
            new(_record.Hirer!.Name, _record.HiredDate.GetUnixTimeStamp());

        public TaxCollectorInformations TaxCollectorInformations
        {
            get
            {
                return Fighter?.Team?.Fight is not FightPvT fightPvT || fightPvT.State is not FightState.Placement ?
                        new TaxCollectorInformations(Id,
                            FirstNameId,
                            LastNameId,
                            AdditionalTaxCollectorInformations,
                            Map.Record.X,
                            Map.Record.Y,
                            Map.SubArea?.Id ?? 0,
                            (sbyte)(Fighter is null ? TaxCollectorState.Normal : TaxCollectorState.Fight),
                            Look.GetEntityLook())
                    :
                        new TaxCollectorInformationsInWaitForHelpState(Id,
                            FirstNameId,
                            LastNameId,
                            AdditionalTaxCollectorInformations,
                            Map.Record.X,
                            Map.Record.Y,
                            Map.SubArea?.Id ?? 0,
                            (sbyte)TaxCollectorState.FightPlacement,
                            Look.GetEntityLook(),
                            new(fightPvT.GetChallengerPlacementTimeLeft() / 100, FightConfig.Instance.PvTChallengersPlacementTime / 100,
                                fightPvT.DefendersFreeSlotCount));
            }
        }

        public TaxCollectorBasicInformations BasicInformations =>
            new(FirstNameId,
                LastNameId,
                Map.Record.Id);

        public TaxCollector(TaxCollectorRecord record, Map map)
        {
            _record = record;
            _lock = new();
            Map = map;
            Bag = new(this);
        }

        public void RefreshLook()
        {
            _look = new(GuildManager.TaxCollectorsBones, new(), new(), new(), new());
            _look.AddColor(2, Color.FromArgb(Guild.SymbolColor));
            _look.AddColor(4, Color.FromArgb(Guild.BackgroundColor));

            foreach (var instance in Map.GetInstances())
                instance.TaxCollector?.Refresh();
        }

        public void ChangeGatheredExperience(int amount)
        {
            GatheredExperience += amount;

            if (GatheredExperience < 0)
                GatheredExperience = 0;
            else if (GatheredExperience > TaxCollectorManager.MaxGatheredXP)
                GatheredExperience = TaxCollectorManager.MaxGatheredXP;
        }

        public void ChangeGatheredKamas(int amount)
        {
            GatheredKamas += amount;

            if (GatheredKamas < 0)
                GatheredKamas = 0;
            else if (GatheredKamas > Inventory.MaxKamasInInventory)
                GatheredKamas = Inventory.MaxKamasInInventory;
        }

        public void JoinTeam(Team team, Cell cell)
        {
            if (Fighter is null && !team.Full)
            {
                foreach (var instance in Map.GetInstances())
                    instance.TaxCollector?.ChangeVisibility(false);

                var spells = new Dictionary<short, Spell>();

                foreach (var spellInfos in Guild.SpellsLevels)
                    if (spellInfos.Value > 0 && SpellManager.Instance.GetSpellTemplateById(spellInfos.Key) is { } template)
                        spells.Add(spellInfos.Key, new(template, spellInfos.Value));

                Fighter = new TaxCollectorFighter(team.Fight.GetFreeContextualId(), this, cell, spells);
                team.AddFighter(Fighter);
            }
        }

        public void QuitFight()
        {
            if (Fighter is not null)
            {
                foreach (var instance in Map.GetInstances())
                    instance.TaxCollector?.ChangeVisibility(true);

                Fighter = default;
            }
        }

        public void Save()
        {
            DatabaseAccessor.Instance.InsertOrUpdate(_record);
            Bag.Save();
        }

        public void Delete()
        {
            DatabaseAccessor.Instance.Delete(_record);
            DatabaseAccessor.Instance.ExecuteNonQuery(string.Format(TaxCollectorItemRelator.DeleteByOwnerId, Id));
        }
    }
}
