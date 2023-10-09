using Rollback.Common.Commands;
using Rollback.Common.Extensions;
using Rollback.Protocol.Enums;
using Rollback.Protocol.Messages;
using Rollback.World.Game.RolePlayActors;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Game.World;
using Rollback.World.Network;
using Rollback.World.Network.Handler;

namespace Rollback.World.Handlers.Chat
{
    public static class ChatHandler
    {
        [WorldHandler(ChatClientMultiMessage.Id)]
        public static void HandleChatClientMultiMessage(WorldClient client, ChatClientMultiMessage message) // TO DO
        {
            if (message.content != string.Empty)
            {
                if (message.content[0] is '.')
                    CommandManager.Instance.HandleCommand(message.content[1..], client.Account!.Character!);
                else
                {
                    var now = DateTime.Now;
                    switch (message.channel)
                    {
                        case (sbyte)ChatChannelsMultiEnum.CHANNEL_TEAM:
                            /*if (client.Character.Team != null)
                            {
                            }*/
                            break;
                        case (sbyte)ChatChannelsMultiEnum.CHANNEL_GUILD:
                            /*if (client.Character.Guild != null)
                            {
                            }*/
                            break;
                        case (sbyte)ChatChannelsMultiEnum.CHANNEL_ALIGN:
                            if (client.Account!.Character!.AlignmentSide is not AlignmentSideEnum.ALIGNMENT_NEUTRAL)
                                SendAlignChatServerMessage(client.Account.Character, now, message.content);
                            break;
                        case (sbyte)ChatChannelsMultiEnum.CHANNEL_PARTY:
                            SendPartyChatServerMessage(client.Account!.Character!, now, message.content);
                            break;
                        case (sbyte)ChatChannelsMultiEnum.CHANNEL_SALES:
                            var cooldownSales = TimeSpan.FromMilliseconds(GeneralConfiguration.Instance.TimeBetweenSalesMessage);

                            if (client.Account!.Character!.LastSalesMessage is not null && client.Account!.Character!.LastSalesMessage - now <= cooldownSales)
                                // Ce canal est restreint pour améliorer sa lisibilité. Vous pourrez envoyer un nouveau message dans %1 secondes. Ceci ne vous autorise cependant pas pour autant à surcharger ce canal.
                                client.Account!.Character!.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 115, Math.Ceiling((client.Account!.Character!.LastSalesMessage + cooldownSales - now).Value.TotalSeconds));
                            else
                            {
                                client.Account.Character.LastSalesMessage = now;
                                SendSalesChatServerMessage(client.Account!.Character!, now, message.content);
                            }
                            break;
                        case (sbyte)ChatChannelsMultiEnum.CHANNEL_SEEK:
                            var cooldownSeek = TimeSpan.FromMilliseconds(GeneralConfiguration.Instance.TimeBetweenSeekMessage);

                            if (client.Account!.Character!.LastSeekMessage is not null && client.Account!.Character!.LastSeekMessage - now <= cooldownSeek)
                                // Ce canal est restreint pour améliorer sa lisibilité. Vous pourrez envoyer un nouveau message dans %1 secondes. Ceci ne vous autorise cependant pas pour autant à surcharger ce canal.
                                client.Account!.Character!.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 115, Math.Ceiling((client.Account!.Character!.LastSeekMessage + cooldownSeek - now).Value.TotalSeconds));
                            else
                            {
                                client.Account.Character.LastSeekMessage = now;
                                SendSeekChatServerMessage(client.Account!.Character!, now, message.content);
                            }
                            break;
                        case (sbyte)ChatChannelsMultiEnum.CHANNEL_NOOB:
                            break;
                        case (sbyte)ChatChannelsMultiEnum.CHANNEL_ADMIN:
                            SendAdminChatServerMessage(client.Account!.Character!, now, message.content);
                            break;
                        default:
                            SendBasicChatServerMessage(client.Account!.Character!, now, message.content);
                            break;
                    }
                }
            }
        }

        [WorldHandler(ChatClientPrivateMessage.Id)]
        public static void HandleChatClientPrivateMessage(WorldClient client, ChatClientPrivateMessage message)
        {
            if (message.content != string.Empty)
            {
                var target = WorldServer.Instance.GetClient(x => x.Account?.Character?.Name == message.receiver)?.Account!.Character;
                if (target is not null)
                {
                    //TO DO Ignored
                    if (target.Id != client.Account!.Character!.Id)
                    {
                        var now = DateTime.Now;

                        SendChatServerMessage(target.Client, client.Account.Character, ChatActivableChannelsEnum.PSEUDO_CHANNEL_PRIVATE, now, message.content);
                        SendChatServerCopyMessage(client, ChatActivableChannelsEnum.PSEUDO_CHANNEL_PRIVATE, message.content, target);
                    }
                    else
                        SendChatErrorMessage(client, ChatErrorEnum.CHAT_ERROR_INTERIOR_MONOLOGUE);
                }
                else
                    SendChatErrorMessage(client, ChatErrorEnum.CHAT_ERROR_RECEIVER_NOT_FOUND);
            }
        }

        [WorldHandler(AdminQuietCommandMessage.Id)]
        public static void HandleAdminQuietCommandMessage(WorldClient client, AdminQuietCommandMessage message)
        {
            if (message.content.Contains("move") && client.Account!.Role is GameHierarchyEnum.ADMIN)
            {
                var pos = message.content[7..].Split(',');
                var map = WorldManager.Instance.GetMapIdFromCoord(client.Account!.Character!.MapInstance?.Map.SubArea?.Area.SuperArea.Id is null ? (short)0 : client.Account!.Character!.MapInstance.Map.SubArea!.Area.SuperArea.Id, sbyte.Parse(pos[0]), sbyte.Parse(pos[1]));

                if (map is not null)
                    client.Account!.Character!.Teleport(map);
            }
        }

        private static void SendChatServerMessage(WorldClient client, RolePlayActor sender, ChatActivableChannelsEnum channel, DateTime time, string content) =>
            client.Send(new ChatServerMessage((sbyte)channel, content, time.GetUnixTimeStamp(), string.Empty, sender.Id, sender is Character character ? character.Name : string.Empty));

        public static void SendChatServerCopyMessage(WorldClient client, ChatActivableChannelsEnum channel, string content, Character target)
        {
            if (content != string.Empty)
                client.Send(new ChatServerCopyMessage((sbyte)channel, content, TimeExtensions.GetUnixTimeStamp(DateTime.Now), "", target.Id, target.Name));
        }

        public static void SendBasicChatServerMessage(RolePlayActor sender, DateTime time, string content) =>
            sender.MapInstance.Send(SendChatServerMessage, new object[] { sender, ChatChannelsMultiEnum.CHANNEL_GLOBAL, time, content });

        public static void SendAlignChatServerMessage(Character sender, DateTime time, string content) =>
            WorldServer.Instance.Send(x => x.Account?.Character?.AlignmentSide == sender.AlignmentSide, SendChatServerMessage,
                new object[] { sender, ChatActivableChannelsEnum.CHANNEL_ALIGN, time, content });

        public static void SendSalesChatServerMessage(Character sender, DateTime time, string content) =>
            WorldServer.Instance.Send(x => x.Account?.Character is not null, SendChatServerMessage,
                new object[] { sender, ChatActivableChannelsEnum.CHANNEL_SALES, time, content });

        public static void SendSeekChatServerMessage(Character sender, DateTime time, string content) =>
            WorldServer.Instance.Send(x => x.Account?.Character is not null, SendChatServerMessage,
                new object[] { sender, ChatActivableChannelsEnum.CHANNEL_SEEK, time, content });

        public static void SendAdminChatServerMessage(Character sender, DateTime time, string content) =>
            WorldServer.Instance.Send(x => x.Account?.Character is not null && x.Account.Role > GameHierarchyEnum.PLAYER, SendChatServerMessage,
                new object[] { sender, ChatActivableChannelsEnum.CHANNEL_ADMIN, time, content });

        public static void SendPartyChatServerMessage(Character sender, DateTime time, string content)
        {
            if (sender.Party is not null)
                sender.Party.Send(SendChatServerMessage, new object[] { sender, ChatActivableChannelsEnum.CHANNEL_PARTY, time, content });
            else
                SendChatErrorMessage(sender.Client, ChatErrorEnum.CHAT_ERROR_NO_PARTY);
        }

        public static void SendEmoteListMessage(WorldClient client, sbyte[] emotesIds) =>
            client.Send(new EmoteListMessage(emotesIds));

        public static void SendEnabledChannelsMessage(WorldClient client, sbyte[] channelIds, sbyte[] disabledChannelsIds) =>
            client.Send(new EnabledChannelsMessage(channelIds, disabledChannelsIds));

        public static void SendChatErrorMessage(WorldClient client, ChatErrorEnum reason) =>
            client.Send(new ChatErrorMessage((sbyte)reason));
    }
}
