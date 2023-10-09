using Rollback.Common.Initialization;
using Rollback.World.Network;

InitializableManager.Initialize();

if (WorldServer.Instance.Config.DebugMod)
{
    Console.WriteLine("Press any key to exit...");
    Console.ReadKey();
}