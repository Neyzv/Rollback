using System.Text.RegularExpressions;
using Rollback.Common.ORM;
using Rollback.World.Database.Interactives;

namespace MapTools.Tasks
{
    internal static class InteractiveWorkerManager
    {
        private const string _getInteractiveSkillById = "SELECT * FROM interactives_skills WHERE Id = {0}";
        private const string _getInteractiveByElementId = "SELECT * FROM interactives_spawns WHERE ElementId = {0}";
        private const string _idChecker = "(?:;|^){0}(?:;|$)";

        public static async void AssignSkillToElementId(DatabaseAccessor db, int elementId, int skillId)
        {
            if (db.SelectSingle<InteractiveSkillRecord>(string.Format(_getInteractiveSkillById, skillId)) is not null)
            {
                await Parallel.ForEachAsync(db.Select<InteractiveSpawnRecord>(string.Format(_getInteractiveByElementId, elementId)),
                    (spawn, _) =>
                    {
                        if (!new Regex(string.Format(_idChecker, skillId), RegexOptions.Compiled).IsMatch(spawn.SkillsIdsCSV))
                        {
                            spawn.SkillsIdsCSV += spawn.SkillsIdsCSV.Length is 0 ? skillId : $",{skillId}";
                            db.Update(spawn);

                            Console.WriteLine($"Updated interactive {spawn.Id} successfully !");
                        }

                        return ValueTask.CompletedTask;
                    });
            }
            else
                Console.WriteLine($"Unknown skill {skillId}...");
        }
    }
}
