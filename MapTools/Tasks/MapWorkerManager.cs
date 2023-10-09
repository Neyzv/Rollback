using Rollback.Client;
using Rollback.Client.Dlm;
using Rollback.Common.ORM;
using Rollback.World.Database.World;

namespace MapTools.Tasks
{
    internal sealed class MapWorkerManager
    {
        private static void UpdateDlmFile(DatabaseAccessor db, Map map)
        {
            var record = db.Select<MapRecord>("SELECT * FROM world_maps WHERE Id = " + map.Id).FirstOrDefault();
            if (record != null)
            {
                //map.SubAreaId = record.SubAreaId;
                //map.LeftNeighbourId = record.LeftNeighbourId;
                //map.RightNeighbourId = record.RightNeighbourId;
                //map.BottomNeighbourId = record.BottomNeighbourId;
                //map.TopNeighbourId = record.TopNeighbourId;

                using (var _ = File.Create("./outputs/" + map.Id + ".dlm")) { }

                ClientFile.WriteDlm(map, "./outputs/" + map.Id + ".dlm");
                var d = ClientFile.ReadDlm("./outputs/" + map.Id + ".dlm");

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Map " + map.Id + " successfully synchronized !");
                Console.ResetColor();
            }
        }

        public static void UpdateMap(DatabaseAccessor db, string clientPath, int mapId)
        {
            var dlmFile = Directory.GetFiles(clientPath, mapId + ".dlm", SearchOption.AllDirectories).FirstOrDefault();
            if (dlmFile != null)
            {
                var map = ClientFile.ReadDlm(dlmFile);
                UpdateDlmFile(db, map);
            }
        }

        public static void UpdateAllMaps(DatabaseAccessor db, string clientPath)
        {
            foreach (var dlmFile in Directory.GetFiles(clientPath, "*.dlm", SearchOption.AllDirectories))
                UpdateDlmFile(db, ClientFile.ReadDlm(dlmFile));
        }
    }
}
