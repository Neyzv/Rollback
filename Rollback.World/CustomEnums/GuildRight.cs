namespace Rollback.World.CustomEnums
{
    public enum GuildRight : uint
    {
        Boss = 1,
        ManageBoost = 2,
        ManageRights = 4,
        InviteMembers = 8,
        BanMembers = 16,
        ManageMembersXP = 32,
        ManageRanks = 64,
        ManageOwnXP = 128,
        SummonTaxCollector = 256,
        CollectTaxCollectors = 512,
        UsePaddocks = 4096,
        ManagePaddocks = 8192,
        ManageMounts = 16384,
    }
}
