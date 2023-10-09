using Rollback.Auth.Network;
using Rollback.Common.Initialization;

InitializableManager.Initialize();

if (AuthServer.Instance.Config.DebugMod)
{
    Console.WriteLine("Press any key to exit...");
    Console.ReadKey();
}