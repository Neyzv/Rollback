using Rollback.Protocol.Messages;
using Rollback.World.Game.Jobs;
using Rollback.World.Network;

namespace Rollback.World.Handlers.Jobs
{
    public static class JobHandler
    {
        public static void SendJobDescriptionMessage(WorldClient client) =>
            client.Send(new JobDescriptionMessage(client.Account!.Character!.Jobs.Values.Select(x => x.JobDescription).ToArray()));

        public static void SendJobExperienceMultiUpdateMessage(WorldClient client) =>
            client.Send(new JobExperienceMultiUpdateMessage(client.Account!.Character!.Jobs.Values.Select(x => x.JobExperience).ToArray()));

        public static void SendJobCrafterDirectorySettingsMessage(WorldClient client) =>
            client.Send(new JobCrafterDirectorySettingsMessage(client.Account!.Character!.Jobs.Values.Select(x => x.JobCrafterDirectorySettings).ToArray()));

        public static void SendJobExperienceUpdateMessage(WorldClient client, Job job) =>
            client.Send(new JobExperienceUpdateMessage(job.JobExperience));

        public static void SendObjectJobAddedMessage(WorldClient client, Job job) =>
            client.Send(new ObjectJobAddedMessage((sbyte)job.Id));

        public static void SendJobListedUpdateMessage(WorldClient client, Job job) =>
            client.Send(new JobListedUpdateMessage(client.Account!.Character!.Jobs.ContainsKey(job.Id), (sbyte)job.Id));

        public static void SendJobLevelUpMessage(WorldClient client, Job job) =>
            client.Send(new JobLevelUpMessage(job.Level, job.JobDescription));
    }
}
