using MapTools.Tasks;
using Rollback.Common.ORM;

var db = new DatabaseAccessor(new()
{
    Host = "127.0.0.1",
    User = "root",
    Password = "",
    DatabaseName = "rollback_world",
});
var clientPath = "D:\\Code\\dofus\\2.0\\client\\app";

if (!Directory.Exists("outputs"))
    Directory.CreateDirectory("outputs");

if (!Directory.Exists("inputs"))
    Directory.CreateDirectory("inputs");

if (db.TestConnection())
{
    while (true)
    {
        Console.WriteLine("Choose an action :\n1 : Set client path" +
            "\n2 : Sync maps\n3 : Sync interacives\n4 : Assign skill to an element id");

        switch (Console.ReadLine())
        {
            case "1":
                Console.WriteLine("Please write your path :");
                var path = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(path) && ClientManager.CheckClientPath(path))
                    clientPath = path;
                else
                    Console.WriteLine("Wrong dofus path...");
                break;

            case "2":
                Console.WriteLine("1 : From DB to Client");
                switch (Console.ReadLine())
                {
                    case "1":
                        Console.WriteLine("1 : Sync all maps");
                        Console.WriteLine("2 : Sync one map");
                        switch (Console.ReadLine())
                        {
                            case "1":
                                MapWorkerManager.UpdateAllMaps(db, clientPath);
                                break;

                            case "2":
                                Console.Write("Id of the map : ");
                                MapWorkerManager.UpdateMap(db, clientPath, int.Parse(Console.ReadLine()!));
                                break;
                        }
                        break;
                }
                break;

            case "4":
                Console.WriteLine("Please enter element id :");
                if (int.TryParse(Console.ReadLine(), out var elementId))
                {
                    Console.WriteLine("Please enter skill id :");
                    if (int.TryParse(Console.ReadLine(), out var skillId))
                        InteractiveWorkerManager.AssignSkillToElementId(db, elementId, skillId);
                    else
                        Console.WriteLine("Incorrect numeric value...");
                }
                else
                    Console.WriteLine("Incorrect numeric value...");
                break;

            default:
                Console.WriteLine("Incorrect choice...");
                break;
        }
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
        Console.Clear();
    }
}
else
    Console.WriteLine("Error while connecting to the database, please check that your mysql provider is on or your identification logs...");