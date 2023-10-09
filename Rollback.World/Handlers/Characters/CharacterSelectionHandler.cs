using Rollback.Common.Logging;
using Rollback.Protocol.Enums;
using Rollback.Protocol.Messages;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Game.World;
using Rollback.World.Game.World.Maps;
using Rollback.World.Handlers.Alignment;
using Rollback.World.Handlers.Basics;
using Rollback.World.Handlers.Chat;
using Rollback.World.Handlers.Guilds;
using Rollback.World.Handlers.Inventory;
using Rollback.World.Handlers.Mounts;
using Rollback.World.Handlers.Social;
using Rollback.World.Network;
using Rollback.World.Network.Handler;

namespace Rollback.World.Handlers.Characters
{
    public static class CharacterSelectionHandler
    {
        [WorldHandler(CharacterSelectionMessage.Id, false)]
        public static void HandleCharacterSelectionMessage(WorldClient client, CharacterSelectionMessage message)
        {
            if (client.Account!.Characters.ContainsKey(message.id))
            {
                try
                {
                    var record = client.Account.Characters[message.id];

                    var map = WorldManager.Instance.GetMapById(record.MapId);
                    if (map is not null && Cell.CellIdValid(record.CellId))
                    {
                        client.Account.Character = new Character(client, record, map.GetBestInstance(), map.Record.Cells[record.CellId], record.Direction);
                        SendCharacterSelectedSuccessMessage(client);

                        ChatHandler.SendEmoteListMessage(client, new sbyte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 19, 21, 22, 23, 24 });
                        ChatHandler.SendEnabledChannelsMessage(client, new sbyte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 }, Array.Empty<sbyte>());

                        InventoryHandler.SendInventoryContentMessage(client);
                        InventoryHandler.SendInventoryWeightMessage(client);

                        InventoryHandler.SendSpellListMessage(client);

                        AlignmentHandler.SendAlignmentRankUpdateMessage(client);
                        AlignmentHandler.SendAlignmentSubAreasListMessage(client);

                        CharacterHandler.SendSetCharacterRestrictionsMessage(client);
                        CharacterHandler.SendGameRolePlayPlayerLifeStatusMessage(client);

                        client.Account.Character.RefreshJobs();

                        if (client.Account.Character.GuildMember is not null)
                        {
                            GuildHandler.SendGuildMembershipMessage(client);
                            GuildHandler.SendGuildInformationsMembersMessage(client);
                            client.Account.Character.GuildMember.Refresh();
                        }

                        if (client.Account.Character.EquipedMount is not null)
                        {
                            client.Account.Character.EquipedMount.SetOwner(client.Account.Character);
                            
                            MountHandler.SendMountSetMessage(client);
                            MountHandler.SendMountXpRatioMessage(client);

                            if (client.Account.Character.IsRiding)
                            {
                                record.IsRiding = false;
                                client.Account.Character.RideMount();
                            }
                        }

                        SocialHandler.SendFriendWarnOnConnectionStateMessage(client);
                        SocialHandler.SendFriendWarnOnLevelGainStateMessage(client);

                        client.Account.Character.StartRegen();
                        client.Account.Character.LoadSpouse();

                        BasicHandler.SendTextInformationMessage(client, TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 89);
                        if (client.Account!.LastConnection.HasValue && !string.IsNullOrEmpty(client.Account.LastIP))
                        {
                            BasicHandler.SendTextInformationMessage(client, TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 152,
                                    $"{client.Account.LastConnection.Value.Year}", $"{client.Account.LastConnection.Value.Month}",
                                    $"{client.Account.LastConnection.Value.Day}", $"{client.Account.LastConnection.Value.Hour}",
                                    $"{client.Account.LastConnection.Value.Minute}", client.Account.LastIP
                                );
                        }
                        BasicHandler.SendTextInformationMessage(client, TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 153, client.IP!);
                        client.Account.Character.SendServerMessage($"<b>{GeneralConfiguration.Instance.HelloWorldMessage}</b>", GeneralConfiguration.Instance.HelloWorldColor);
                        BasicHandler.SendTextInformationMessage(client, TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 225);
                    }
                    else
                    {
                        Logger.Instance.LogWarn($"Force disconnection of client {client} : Can not use character's map informations to log in...");
                        client.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    Logger.Instance.LogWarn($"Force disconnection of client {client} : {ex.Message}");
                    client.Dispose();
                }
            }
            else
                SendCharacterSelectedErrorMessage(client);
        }

        [WorldHandler(CharacterFirstSelectionMessage.Id, false)]
        public static void HandleCharacterFirstSelectionMessage(WorldClient client, CharacterFirstSelectionMessage message) =>
            HandleCharacterSelectionMessage(client, message);

        public static void SendCharacterSelectedErrorMessage(WorldClient client) =>
            client.Send(new CharacterSelectedErrorMessage());

        public static void SendCharacterSelectedSuccessMessage(WorldClient client) =>
            client.Send(new CharacterSelectedSuccessMessage(client.Account!.Character!.CharacterBaseInformations));
    }
}
