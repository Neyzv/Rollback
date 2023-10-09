using Rollback.Protocol.Enums;
using Rollback.Protocol.Messages;
using Rollback.Protocol.Types;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Fights;
using Rollback.World.Game.Fights.Types;
using Rollback.World.Game.Guilds;
using Rollback.World.Game.Interactions.Dialogs.Exchanges.TaxCollectors;
using Rollback.World.Game.Interactions.Requests.Guilds;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Game.RolePlayActors.TaxCollectors;
using Rollback.World.Handlers.Fights;
using Rollback.World.Network;
using Rollback.World.Network.Handler;
using GuildMember = Rollback.World.Game.Guilds.GuildMember;

namespace Rollback.World.Handlers.Guilds
{
    public static class GuildHandler
    {
        [WorldHandler(GuildCreationValidMessage.Id)]
        public static void HandleGuildCreationValidMessage(WorldClient client, GuildCreationValidMessage message)
        {
            if (client.Account!.Character!.Fighter is null)
            {
                var result = GuildCreationResultEnum.GUILD_CREATE_OK;

                if (GuildManager.IsNameCorrect(message.guildName))
                {
                    if (GuildManager.Instance.IsEmblemCorrect(message.guildEmblem))
                    {
                        if (GuildManager.Instance.GetGuild(x => x.Name == message.guildName) is null)
                        {
                            if (client.Account!.Character!.Inventory.GetItem(x => x.Id == GuildManager.GuildalogemmeId) is { } guildalogemme &&
                                GuildManager.Instance.CreateGuild(message.guildName, message.guildEmblem) is { } guild &&
                                guild.AddMember(client.Account.Character))
                            {
                                client.Account.Character.Inventory.RemoveItem(guildalogemme, 1);

                                if (client.Account!.Character!.Interaction is not null)
                                    client.Account.Character.Interaction.Close();
                            }
                            else
                                result = GuildCreationResultEnum.GUILD_CREATE_ERROR_REQUIREMENT_UNMET;
                        }
                        else
                            result = GuildCreationResultEnum.GUILD_CREATE_ERROR_NAME_ALREADY_EXISTS;
                    }
                    else
                        result = GuildCreationResultEnum.GUILD_CREATE_ERROR_EMBLEM_ALREADY_EXISTS;
                }
                else
                    result = GuildCreationResultEnum.GUILD_CREATE_ERROR_NAME_INVALID;

                SendGuildCreationResultMessage(client, result);
            }
        }

        [WorldHandler(GuildGetInformationsMessage.Id)]
        public static void HandleGuildGetInformationsMessage(WorldClient client, GuildGetInformationsMessage message)
        {
            if (client.Account!.Character!.GuildMember is not null)
            {
                switch ((GuildInformationsType)message.infoType)
                {
                    case GuildInformationsType.General:
                        SendGuildInformationsGeneralMessage(client);
                        break;

                    case GuildInformationsType.Members:
                        SendGuildInformationsMembersMessage(client);
                        break;

                    case GuildInformationsType.Upgrade:
                        SendGuildInfosUpgradeMessage(client);
                        break;

                    case GuildInformationsType.Paddock:
                        SendGuildInformationsPaddocksMessage(client);
                        break;

                    case GuildInformationsType.Houses:
                        SendGuildHousesInformationMessage(client);
                        break;

                    case GuildInformationsType.TaxCollectors:
                        SendTaxCollectorListMessage(client);
                        break;
                }
            }
        }

        [WorldHandler(GuildInvitationMessage.Id)]
        public static void HandleGuildInvitationMessage(WorldClient client, GuildInvitationMessage message)
        {
            if (client.Account!.Character!.GuildMember is not null && !client.Account.Character.IsBusy)
            {
                if (client.Account.Character.GuildMember.HasRight(GuildRight.InviteMembers))
                {
                    if (WorldServer.Instance.GetClient(x => x.Account?.Character?.Id == message.targetId)?.Account!.Character is { } target)
                    {
                        if (target.GuildMember is null)
                        {
                            if (!target.IsBusy)
                            {
                                if (client.Account.Character.GuildMember.Guild.CanAddMembers)
                                    new GuildInvitationRequest(client.Account.Character, target).Open();
                                else
                                    // Le niveau de votre guilde ne lui permet pas d\'avoir plus de %1 membres.
                                    client.Account.Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 55,
                                        client.Account.Character.GuildMember.Guild.MaxMembers);
                            }
                            else
                                // Ce joueur est occupé. Impossible de l\'inviter.
                                client.Account.Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 209);
                        }
                        else
                            // Impossible, ce joueur est déjà dans une guilde
                            client.Account.Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 206);
                    }
                    else
                        // Impossible d\'inviter, ce joueur est inconnu ou non connecté.
                        client.Account.Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 208);
                }
                else
                    // Vous n\'avez pas le droit requis pour inviter des joueurs dans votre guilde.
                    client.Account.Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 207);
            }
        }

        [WorldHandler(GuildInvitationAnswerMessage.Id)]
        public static void HandleGuildInvitationAnswerMessage(WorldClient client, GuildInvitationAnswerMessage message)
        {
            if (client.Account!.Character!.Interaction is GuildInvitationRequest request)
            {
                if (message.accept)
                    request.Accept();
                else
                    request.Close();
            }
        }

        [WorldHandler(GuildChangeMemberParametersMessage.Id)]
        public static void HandleGuildChangeMemberParametersMessage(WorldClient client, GuildChangeMemberParametersMessage message)
        {
            if (Enum.IsDefined(typeof(GuildRank), message.rank))
            {
                var rights = new List<GuildRight>();
                var newRights = (GuildRight)message.rights;

                foreach (var right in Enum.GetValues<GuildRight>())
                    if (newRights.HasFlag(right))
                        rights.Add(right);

                client.Account!.Character!.GuildMember?.Guild.ManageMemberOptions(client.Account.Character.Id,
                    message.memberId, (GuildRank)message.rank, message.experienceGivenPercent, rights.ToArray());
            }
        }

        [WorldHandler(GuildKickRequestMessage.Id)]
        public static void HandleGuildKickRequestMessage(WorldClient client, GuildKickRequestMessage message) =>
            client.Account!.Character!.GuildMember?.Guild.KickMember(client.Account.Character.Id, message.kickedId);

        [WorldHandler(TaxCollectorHireRequestMessage.Id)]
        public static void HandleTaxCollectorHireRequestMessage(WorldClient client, TaxCollectorHireRequestMessage message)
        {
            if (client.Account!.Character!.Fighter is null)
            {
                if (client.Account!.Character!.GuildMember?.HasRight(GuildRight.SummonTaxCollector) == true)
                {
                    if (client.Account.Character.Kamas >= client.Account.Character.GuildMember.Guild.HireCost)
                    {
                        if (client.Account.Character.GuildMember.Hire())
                            client.Account.Character.ChangeKamas(-client.Account.Character.GuildMember.Guild.HireCost);
                    }
                    else
                        SendTaxCollectorErrorMessage(client, TaxCollectorErrorReasonEnum.TAX_COLLECTOR_NOT_ENOUGH_KAMAS);
                }
                else
                    SendTaxCollectorErrorMessage(client, TaxCollectorErrorReasonEnum.TAX_COLLECTOR_NO_RIGHTS);
            }
        }

        [WorldHandler(GuildCharacsUpgradeRequestMessage.Id)]
        public static void HandleGuildCharacsUpgradeRequestMessage(WorldClient client, GuildCharacsUpgradeRequestMessage message)
        {
            if (client.Account!.Character!.GuildMember?.HasRight(GuildRight.ManageBoost) == true)
                client.Account.Character.GuildMember.Guild.Boost((GuildBoostType)message.charaTypeTarget);
        }

        [WorldHandler(GuildSpellUpgradeRequestMessage.Id)]
        public static void HandleGuildSpellUpgradeRequestMessage(WorldClient client, GuildSpellUpgradeRequestMessage message)
        {
            if (client.Account!.Character!.GuildMember?.HasRight(GuildRight.ManageBoost) == true)
                client.Account.Character.GuildMember.Guild.BoostSpell((short)message.spellId);
        }

        [WorldHandler(TaxCollectorFireRequestMessage.Id)]
        public static void HandleTaxCollectorFireRequestMessage(WorldClient client, TaxCollectorFireRequestMessage message)
        {
            if (client.Account!.Character!.Fighter is null)
                if (client.Account!.Character!.MapInstance.GetActor<TaxCollectorNpc>(message.collectorId) is { } taxCollector)
                    if (client.Account.Character.GuildMember?.HasRight(GuildRight.CollectTaxCollectors) == true)
                        client.Account.Character.GuildMember.Fire(taxCollector.TaxCollector.Id);
                    else
                        SendTaxCollectorErrorMessage(client, TaxCollectorErrorReasonEnum.TAX_COLLECTOR_NO_RIGHTS);
        }

        [WorldHandler(ExchangeRequestOnTaxCollectorMessage.Id)]
        public static void HandleExchangeRequestOnTaxCollectorMessage(WorldClient client, ExchangeRequestOnTaxCollectorMessage message)
        {
            if (client.Account!.Character!.Fighter is null &&
                client.Account.Character.MapInstance.GetActor<TaxCollectorNpc>(message.taxCollectorId) is { } taxCollector)
            {
                if (client.Account.Character.GuildMember is not null &&
                    client.Account.Character.GuildMember.Guild.Id == taxCollector.TaxCollector.Guild.Id)
                {
                    if (client.Account.Character.GuildMember.HasRight(GuildRight.CollectTaxCollectors))
                    {
                        if (taxCollector.TaxCollector.IsBusy)
                            SendTaxCollectorErrorMessage(client, TaxCollectorErrorReasonEnum.TAX_COLLECTOR_ERROR_UNKNOWN);
                        else
                            new TaxCollectorExchange(client.Account.Character, taxCollector.TaxCollector).Open();
                    }
                    else
                        SendTaxCollectorErrorMessage(client, TaxCollectorErrorReasonEnum.TAX_COLLECTOR_NO_RIGHTS);
                }
                else
                    SendTaxCollectorErrorMessage(client, TaxCollectorErrorReasonEnum.TAX_COLLECTOR_NOT_OWNED);
            }
        }

        [WorldHandler(GameRolePlayTaxCollectorFightRequestMessage.Id)]
        public static void HandleGameRolePlayTaxCollectorFightRequestMessage(WorldClient client, GameRolePlayTaxCollectorFightRequestMessage message)
        {
            if (client.Account!.Character!.Fighter is null &&
                client.Account.Character.MapInstance.GetActor<TaxCollectorNpc>(message.taxCollectorId) is { } taxCollector)
            {
                var fighterRefusedReason = client.Account.Character.CanRequestFight(taxCollector);
                if (fighterRefusedReason is FighterRefusedReasonEnum.FIGHTER_ACCEPTED)
                {
                    FightManager.CreatePvT(client.Account.Character.MapInstance, client.Account.Character, taxCollector.TaxCollector)?
                        .StartPlacement();
                }
                else
                    FightHandler.SendChallengeFightJoinRefusedMessage(client, fighterRefusedReason);
            }
        }

        [WorldHandler(GuildFightJoinRequestMessage.Id)]
        public static void HandleGuildFightJoinRequestMessage(WorldClient client, GuildFightJoinRequestMessage message)
        {
            if (client.Account!.Character!.Fighter is null &&
                client.Account.Character.GuildMember?.Guild.GetTaxCollectorById(message.taxCollectorId) is { } taxCollector &&
                taxCollector.Fighter?.Team!.Fight is FightPvT fight)
            {
                var fighterRefusedReason = fight.AddWaitingDefender(client.Account.Character);

                if (fighterRefusedReason is not FighterRefusedReasonEnum.FIGHTER_ACCEPTED)
                    FightHandler.SendChallengeFightJoinRefusedMessage(client, fighterRefusedReason);
            }
        }

        [WorldHandler(GuildFightLeaveRequestMessage.Id)]
        public static void HandleGuildFightLeaveRequestMessage(WorldClient client, GuildFightLeaveRequestMessage message)
        {
            if (client.Account!.Character!.GuildMember?.Guild.GetTaxCollectorById(message.taxCollectorId) is { } taxCollector &&
                taxCollector.Fighter?.Team!.Fight is FightPvT fight)
                fight.RemoveWaitingDefender(client.Account.Character.Id);
        }

        public static void SendGuildCreationResultMessage(WorldClient client, GuildCreationResultEnum reason) =>
            client.Send(new GuildCreationResultMessage((sbyte)reason));

        public static void SendGuildCreationStartedMessage(WorldClient client) =>
            client.Send(new GuildCreationStartedMessage());

        public static void SendGuildJoinedMessage(WorldClient client) =>
            client.Send(new GuildJoinedMessage(client.Account!.Character!.GuildMember!.Guild.Name, client.Account.Character.GuildMember!.Guild.Emblem,
                (uint)client.Account.Character.GuildMember.Rights));

        public static void SendGuildInformationsMembersMessage(WorldClient client) =>
            client.Send(new GuildInformationsMembersMessage(client.Account!.Character!.GuildMember!.Guild.MembersInformations));

        public static void SendGuildMembershipMessage(WorldClient client) =>
            client.Send(new GuildMembershipMessage(client.Account!.Character!.GuildMember!.Guild.Name,
                client.Account.Character.GuildMember.Guild.Emblem, (uint)client.Account.Character.GuildMember.Rights));

        public static void SendGuildInformationsGeneralMessage(WorldClient client) =>
            client.Send(new GuildInformationsGeneralMessage(true, client.Account!.Character!.GuildMember!.Guild.Level,
                client.Account.Character.GuildMember.Guild.LowerExperienceLevelFloor, client.Account.Character.GuildMember.Guild.Experience,
                client.Account.Character.GuildMember.Guild.UpperExperienceLevelFloor));

        public static void SendGuildInformationsMemberUpdateMessage(WorldClient client, GuildMember member) =>
            client.Send(new GuildInformationsMemberUpdateMessage(member.MemberInformations));

        public static void SendGuildMemberOnlineStatusMessage(WorldClient client, GuildMember member) =>
            client.Send(new GuildMemberOnlineStatusMessage(member.MemberId, member.Character is not null));

        public static void SendGuildInfosUpgradeMessage(WorldClient client) =>
            client.Send(new GuildInfosUpgradeMessage(client.Account!.Character!.GuildMember!.Guild.MaxTaxCollector,
                0, client.Account.Character.GuildMember.Guild.TaxCollectorHealth, client.Account.Character.GuildMember.Guild.TaxCollectorDamageBonuses,
                client.Account.Character.GuildMember.Guild.TaxCollectorPods, client.Account.Character.GuildMember.Guild.TaxCollectorProspecting,
                client.Account.Character.GuildMember.Guild.TaxCollectorWisdom, client.Account.Character.GuildMember.Guild.BoostPoints,
                client.Account.Character.GuildMember.Guild.SpellsLevels.Keys.ToArray(), client.Account.Character.GuildMember.Guild.SpellsLevels.Values.ToArray()));

        public static void SendGuildInformationsPaddocksMessage(WorldClient client) =>
            client.Send(new GuildInformationsPaddocksMessage(1, Array.Empty<PaddockContentInformations>())); // TO DO

        public static void SendGuildHousesInformationMessage(WorldClient client) =>
            client.Send(new GuildHousesInformationMessage(Array.Empty<HouseInformationsForGuild>())); // TO DO

        public static void SendGuildInvitationStateRecruterMessage(WorldClient client, Character target, GuildInvitationStateEnum state) =>
            client.Send(new GuildInvitationStateRecruterMessage(target.Name, (sbyte)state));

        public static void SendGuildInvitationStateRecrutedMessage(WorldClient client, GuildInvitationStateEnum state) =>
            client.Send(new GuildInvitationStateRecrutedMessage((sbyte)state));

        public static void SendGuildInvitedMessage(WorldClient client, Character sender, Character receiver) =>
            client.Send(new GuildInvitedMessage(sender.Id, sender.Name, sender.GuildMember!.Guild.Name));

        public static void SendGuildLeftMessage(WorldClient client) =>
            client.Send(new GuildLeftMessage());

        public static void SendGuildMemberLeavingMessage(WorldClient client, bool kicked, GuildMember member) =>
            client.Send(new GuildMemberLeavingMessage(kicked, member.MemberId));

        public static void SendGuildLevelUpMessage(WorldClient client) =>
            client.Send(new GuildLevelUpMessage(client.Account!.Character!.GuildMember!.Guild.Level));

        public static void SendTaxCollectorListMessage(WorldClient client)
        {
            var taxCollectors = new List<TaxCollectorInformations>();
            var taxCollectorsInFight = new List<TaxCollectorFightersInformation>();

            foreach (var taxCollector in client.Account!.Character!.GuildMember!.Guild.GetTaxCollectors())
            {
                if (taxCollector.Fighter is not null)
                    taxCollectorsInFight.Add(taxCollector.Fighter.TaxCollectorFightersInformation);

                taxCollectors.Add(taxCollector.TaxCollectorInformations);
            }

            client.Send(new TaxCollectorListMessage(client.Account.Character.GuildMember.Guild.MaxTaxCollector,
                client.Account.Character.GuildMember.Guild.HireCost,
                taxCollectors.ToArray(),
                taxCollectorsInFight.ToArray()));
        }

        public static void SendTaxCollectorErrorMessage(WorldClient client, TaxCollectorErrorReasonEnum reason) =>
            client.Send(new TaxCollectorErrorMessage((sbyte)reason));

        public static void SendTaxCollectorMovementMessage(WorldClient client, bool added, TaxCollector taxCollector, GuildMember member) =>
            client.Send(new TaxCollectorMovementMessage(added, taxCollector.BasicInformations, member.Character!.Name));

        public static void SendTaxCollectorMovementAddMessage(WorldClient client, TaxCollector taxCollector) =>
            client.Send(new TaxCollectorMovementAddMessage(taxCollector.TaxCollectorInformations));

        public static void SendTaxCollectorMovementRemoveMessage(WorldClient client, TaxCollector taxCollector) =>
            client.Send(new TaxCollectorMovementRemoveMessage(taxCollector.Id));

        public static void SendTaxCollectorAttackedMessage(WorldClient client, TaxCollector taxCollector) =>
            client.Send(new TaxCollectorAttackedMessage(taxCollector.FirstNameId, taxCollector.LastNameId, taxCollector.Map.Record.X,
                taxCollector.Map.Record.Y, taxCollector.Map.Record.Id));

        public static void SendGuildFightPlayersHelpersJoinMessage(WorldClient client, Character defender, TaxCollector taxCollector) =>
            client.Send(new GuildFightPlayersHelpersJoinMessage(taxCollector.Id, defender.CharacterMinimalPlusLookInformations));

        public static void SendGuildFightPlayersHelpersLeaveMessage(WorldClient client, Character defender, TaxCollector taxCollector) =>
            client.Send(new GuildFightPlayersHelpersLeaveMessage(taxCollector.Id, defender.Id));

        public static void SendTaxCollectorAttackedResultMessage(WorldClient client, bool isDead, TaxCollector taxCollector) =>
            client.Send(new TaxCollectorAttackedResultMessage(isDead, taxCollector.BasicInformations));
    }
}
