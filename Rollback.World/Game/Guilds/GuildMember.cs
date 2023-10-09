using Rollback.Common.Logging;
using Rollback.Common.ORM;
using Rollback.Protocol.Enums;
using Rollback.World.CustomEnums;
using Rollback.World.Database.Guilds;
using Rollback.World.Game.Fights.Types;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Game.RolePlayActors.TaxCollectors;
using Rollback.World.Handlers.Guilds;

namespace Rollback.World.Game.Guilds
{
    public sealed class GuildMember
    {
        private readonly GuildMemberRecord _record;

        public int MemberId =>
            _record.MemberId;

        public int GuildId =>
            _record.GuildId;

        private Guild? _guild;
        public Guild Guild =>
            _guild ??= AssignGuild()!;

        public Character? Character { get; private set; }

        public string Name =>
            Character is null ? _record.MemberRecord!.Name : Character.Name;

        public GuildRank Rank
        {
            get => _record.MemberRank;
            set => _record.MemberRank = value;
        }

        public GuildRight Rights
        {
            get => _record.Rights;
            private set => _record.Rights = value;
        }

        public sbyte GivenXPPercent
        {
            get => _record.GivenXPPercent;
            private set => _record.GivenXPPercent = value;
        }

        public long GivenXP
        {
            get => _record.GivenXP;
            private set => _record.GivenXP = value;
        }

        public FightPvT? WaitingFight { get; set; }

        public Protocol.Types.GuildMember MemberInformations =>
            Character is null ? new(MemberId, _record.MemberRecord!.Name, _record.Level, (sbyte)_record.MemberRecord.Breed, _record.MemberRecord.Sex,
                (short)Rank, GivenXP, GivenXPPercent, (uint)Rights, 0, (sbyte)_record.MemberRecord.AlignmentSide, 0) :
                new(MemberId, Character.Name, Character.Level, (sbyte)Character.Breed, Character.Sex, (short)Rank, GivenXP, GivenXPPercent, (uint)Rights,
                    (sbyte)(Character.Fighter is null ? PlayerConnectedStatus.Online : PlayerConnectedStatus.InFight),
                    (sbyte)Character.AlignmentSide, 0);

        public GuildMember(GuildMemberRecord record) =>
            _record = record;

        private Guild? AssignGuild()
        {
            var guild = GuildManager.Instance.GetGuildById(GuildId);

            if (guild is null)
                Logger.Instance.LogError(msg: $"Can not find guild {GuildId}, for member {_record.MemberId}...");

            return guild;
        }

        public bool HasRight(params GuildRight[] rights) =>
            Rights is GuildRight.Boss || rights.All(x => Rights.HasFlag(x));

        public void ChangeRights(params GuildRight[] rights)
        {
            var result = 0u;
            foreach (var right in rights)
                if (Enum.IsDefined(right))
                    result |= (uint)right;

            Rights = (GuildRight)result;
        }

        public void ChangeGivenXPPercent(sbyte givenXPPercent) =>
            GivenXPPercent = givenXPPercent < 0 ? (sbyte)0 : givenXPPercent > GuildManager.MaxGivenXpPercent ? GuildManager.MaxGivenXpPercent : givenXPPercent;

        public void ChangeExperience(long amount)
        {
            if (amount > 0 && (ulong)(GivenXP + amount) > long.MaxValue)
                GivenXP = long.MaxValue;
            else if (GivenXP + amount < 0)
                GivenXP = 0;
            else
                GivenXP += amount;

            Guild.ChangeExperience(amount);
        }

        public bool Hire()
        {
            var result = false;
            if (Character is not null)
            {
                if (Character.MapInstance.AllowTaxCollector)
                {
                    if (Character.MapInstance.Map.CanSpawnTaxCollector)
                    {
                        if (Guild.CanAddMembers)
                        {
                            var taxCollector = TaxCollectorManager.Instance.CreateTaxCollector(this);

                            Guild.AddTaxCollector(taxCollector, Character.Cell);
                            Guild.Send(GuildHandler.SendTaxCollectorMovementMessage, new object[] { true, taxCollector, this });

                            result = true;
                        }
                        else
                            GuildHandler.SendTaxCollectorErrorMessage(Character.Client, TaxCollectorErrorReasonEnum.TAX_COLLECTOR_MAX_REACHED);
                    }
                    else
                        GuildHandler.SendTaxCollectorErrorMessage(Character.Client, TaxCollectorErrorReasonEnum.TAX_COLLECTOR_ALREADY_ONE);
                }
                else
                    GuildHandler.SendTaxCollectorErrorMessage(Character.Client, TaxCollectorErrorReasonEnum.TAX_COLLECTOR_CANT_HIRE_HERE);
            }

            return result;
        }

        public void Fire(int taxCollectorId)
        {
            if (Guild.RemoveTaxCollector(taxCollectorId) is { } taxCollector)
                Guild.Send(GuildHandler.SendTaxCollectorMovementMessage, new object[] { false, taxCollector, this });
        }

        public bool AssignCharacter(Character character)
        {
            var res = false;

            if (Character is null)
            {
                Character = character;

                res = true;
            }

            return res;
        }

        public void UnassignCharacter()
        {
            Character = null;
            WaitingFight?.RemoveWaitingDefender(MemberId);

            Refresh();
        }

        public void Refresh()
        {
            UpdateRecord();

            Guild.Send(GuildHandler.SendGuildInformationsMemberUpdateMessage, new[] { this });
            Guild.Send(GuildHandler.SendGuildMemberOnlineStatusMessage, new[] { this });
        }

        private void UpdateRecord()
        {
            if (Character is not null)
            {
                _record.MemberRecord!.Name = Character.Name;
                _record.Level = Character.Level;
            }
        }

        public void Save()
        {
            if (GuildManager.Instance.GetGuildById(GuildId) is not null)
            {
                UpdateRecord();
                DatabaseAccessor.Instance.InsertOrUpdate(_record);
            }
            else
            {
                Logger.Instance.LogInfo($"Deletion of guild member {MemberId}, guild {GuildId} not found...");
                Delete();
            }
        }

        public void Delete() =>
            DatabaseAccessor.Instance.Delete(_record);
    }
}
