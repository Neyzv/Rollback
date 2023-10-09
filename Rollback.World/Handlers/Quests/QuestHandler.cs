using Rollback.Protocol.Messages;
using Rollback.World.Game.Quests;
using Rollback.World.Game.RolePlayActors.Npcs;
using Rollback.World.Network;
using Rollback.World.Network.Handler;

namespace Rollback.World.Handlers.Quests
{
    public static class QuestHandler
    {
        [WorldHandler(QuestListRequestMessage.Id)]
        public static void HandleQuestListRequestMessage(WorldClient client, QuestListRequestMessage message) =>
            SendQuestListMessage(client);

        [WorldHandler(QuestStepInfoRequestMessage.Id)]
        public static void HandleQuestStepInfoRequestMessage(WorldClient client, QuestStepInfoRequestMessage message)
        {
            var questId = (short)message.questId;
            var quest = client.Account!.Character!.GetQuest(questId);

            if (quest is not null)
                SendQuestStepInfoMessage(client, quest);
            else
                SendQuestStepNoInfoMessage(client, questId);
        }

        public static void SendQuestListMessage(WorldClient client)
        {
            var finishedQuests = new List<short>();
            var activeQuests = new List<short>();

            foreach (var quest in client.Account!.Character!.GetQuests())
            {
                if (quest.IsFinished)
                    finishedQuests.Add(quest.Id);
                else
                    activeQuests.Add(quest.Id);
            }

            client.Send(new QuestListMessage(finishedQuests.ToArray(), activeQuests.ToArray()));
        }

        public static void SendQuestStepInfoMessage(WorldClient client, Quest quest)
        {
            var objectiveIds = new List<short>();
            var progressions = new List<bool>();

            foreach (var objective in quest.GetObjectives())
            {
                objectiveIds.Add(objective.Id);
                progressions.Add(objective.IsInProgress);
            }

            client.Send(new QuestStepInfoMessage(quest.Id, quest.CurrentStepId, objectiveIds.ToArray(), progressions.ToArray()));
        }

        public static void SendQuestStepNoInfoMessage(WorldClient client, short questId) =>
            client.Send(new QuestStepNoInfoMessage(questId));

        public static void SendQuestStepStartedMessage(WorldClient client, Quest quest) =>
            client.Send(new QuestStepStartedMessage((ushort)quest.Id, (ushort)quest.CurrentStepId));

        public static void SendMapNpcsQuestStatusUpdateMessage(WorldClient client)
        {
            var npcCanGiveQuest = new List<int>();
            var npcCanNotGiveQuest = new List<int>();

            foreach (var npc in client.Account!.Character!.MapInstance!.GetActors<Npc>())
            {
                if (npc.CanGiveQuest(client.Account.Character))
                    npcCanGiveQuest.Add(npc.Id);
                else
                    npcCanNotGiveQuest.Add(npc.Id);
            }

            client.Send(new MapNpcsQuestStatusUpdateMessage(client.Account!.Character!.MapInstance.Map.Record.Id, npcCanGiveQuest.ToArray(), npcCanNotGiveQuest.ToArray()));
        }


        public static void SendQuestStepValidatedMessage(WorldClient client, Quest quest) =>
            client.Send(new QuestStepValidatedMessage((ushort)quest.Id, (ushort)quest.CurrentStepId));

        public static void SendQuestObjectiveValidatedMessage(WorldClient client, Quest quest, QuestObjective objective) =>
            client.Send(new QuestObjectiveValidatedMessage((ushort)quest.Id, (ushort)objective.Id));
    }
}
