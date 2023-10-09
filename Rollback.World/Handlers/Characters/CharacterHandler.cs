using Rollback.Protocol.Messages;
using Rollback.World.CustomEnums;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Network;
using Rollback.World.Network.Handler;

namespace Rollback.World.Handlers.Characters
{
    public static class CharacterHandler
    {
        [WorldHandler(StatsUpgradeRequestMessage.Id)]
        public static void HandleStatsUpgradeRequestMessage(WorldClient client, StatsUpgradeRequestMessage message)
        {
            if (Enum.IsDefined(typeof(StatBoost), message.statId) && client.Account!.Character!.Fighter is null)
                client.Account.Character.BoostStats((StatBoost)message.statId, message.boostPoint);
        }

        public static void SendCharacterStatsListMessage(WorldClient client) =>
            client.Send(new CharacterStatsListMessage(client.Account!.Character!.CharacterCharacteristicsInformations));

        public static void SendCharacterLevelUpMessage(WorldClient client, byte level) =>
            client.Send(new CharacterLevelUpMessage(level));

        public static void SendGameRolePlayPlayerLifeStatusMessage(WorldClient client) =>
            client.Send(new GameRolePlayPlayerLifeStatusMessage((sbyte)client.Account!.Character!.LifeState));

        public static void SendSetCharacterRestrictionsMessage(WorldClient client) =>
            client.Send(new SetCharacterRestrictionsMessage(client.Account!.Character!.ActorRestrictionsInformations));

        public static void SendCharacterLevelUpInformationMessage(WorldClient client, Character character) => // TO DO
            client.Send(new CharacterLevelUpInformationMessage(character.Level, character.Name, character.Id, 0));

        public static void SendStatsUpgradeResultMessage(WorldClient client, short amount) =>
            client.Send(new StatsUpgradeResultMessage(amount));

        public static void SendLifePointsRegenBeginMessage(WorldClient client, byte regenSpeed) =>
            client.Send(new LifePointsRegenBeginMessage(regenSpeed));

        public static void SendLifePointsRegenEndMessage(WorldClient client, int regainedLife) =>
            client.Send(new LifePointsRegenEndMessage(client.Account!.Character!.Stats.Health.Actual, client.Account!.Character!.Stats.Health.ActualMax, regainedLife));
    }
}
