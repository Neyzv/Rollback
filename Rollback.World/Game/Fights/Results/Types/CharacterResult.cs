using Rollback.Protocol.Enums;
using Rollback.Protocol.Types;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Experiences;
using Rollback.World.Game.Fights.Fighters;
using Rollback.World.Game.Fights.Results.Datas;
using Rollback.World.Game.Guilds;

namespace Rollback.World.Game.Fights.Results.Types
{
    public sealed class CharacterResult : FightResult<CharacterFighter>, IExperienceFightResult
    {
        private FightExperienceData? _experienceData;
        private FightPvPData? _pvpData;

        public override bool CanDrop =>
            true;

        public override short Wisdom =>
            Looter.Stats[Stat.Wisdom].Total;

        public override short Prospecting =>
            Looter.Stats[Stat.Prospecting].Total;

        public CharacterResult(CharacterFighter fighter) : base(fighter) { }

        private FightResultAdditionalData[] GetFightResultAdditionalDatas()
        {
            var res = new List<FightResultAdditionalData>();

            if (_experienceData is not null)
                res.Add(_experienceData.FightResultAdditionalData);

            if (_pvpData is not null)
                res.Add(_pvpData.FightResultAdditionalData);

            return res.ToArray();
        }

        public void AddEarnedExperience(int amount)
        {
            var experience = amount;

            _experienceData ??= new(Looter.Character)
            {
                ShowExperience = true,
                ShowExperienceFightDelta = true
            };

            if (Looter.Character.EquipedMount is not null && Looter.Character.MountXpPercent > 0)
            {
                var xp = (int)Math.Floor(experience * Looter.Character.MountXpPercent * 0.01);
                var mountXp = (int)Math.Floor(xp * ExperienceManager.GetXpGap((short)(Looter.Character.Level - Looter.Character.EquipedMount.Level * 2)) * 0.01);
                
                experience -= xp;

                if (mountXp > 0)
                {
                    _experienceData.ShowExperienceForMount = true;
                    _experienceData.ExperienceForMount += mountXp;
                }
            }

            if (Looter.Character.GuildMember?.GivenXPPercent > 0)
            {
                var xp = (int)Math.Floor(experience * Looter.Character.GuildMember.GivenXPPercent * 0.01);
                var guildXp = (int)Math.Floor(xp * ExperienceManager.GetXpGap((short)(Looter.Character.Level - Looter.Character.GuildMember.Guild.Level)) * 0.01);

                experience -= xp;

                if (guildXp > GuildManager.MaxGuildXP)
                    guildXp = GuildManager.MaxGuildXP;

                if (guildXp > 0)
                {
                    _experienceData.ShowExperienceForGuild = true;
                    _experienceData.ExperienceForGuild += guildXp;
                    Looter.Character.GuildMember.ChangeExperience(guildXp);
                }
            }

            _experienceData.ExperienceFightDelta = experience < 0 ? _experienceData.ExperienceFightDelta : _experienceData.ExperienceFightDelta + experience;
        }

        public override void Apply()
        {
            _experienceData?.Apply();

            // TO DO PvP

            Looter.Character.ChangeKamas(Kamas, false);

            foreach (var droppedItem in _itemsToLoot)
                Looter.Character.Inventory.AddItem(droppedItem.Key, droppedItem.Value, send: false);
        }

        public override FightResultFighterListEntry GetResult(FightOutcomeEnum outcome) =>
            new FightResultPlayerListEntry((short)outcome, Loot, Looter.Id, Looter.Alive, Looter.Character.Level, GetFightResultAdditionalDatas());
    }
}
