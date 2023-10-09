using System.Text.RegularExpressions;

namespace MapTools.Tasks
{
    internal static class ClientManager
    {
        private const string _dofusExeName = "Dofus.exe";
        private static readonly Lazy<Regex> _pathRegex;

        static ClientManager() =>
            _pathRegex = new(() => new("^[A-Z]:(?:[\\\\\\/][^\\\\\\/]+)*[\\\\\\/]?$", RegexOptions.Compiled),
                LazyThreadSafetyMode.ExecutionAndPublication);

        public static bool CheckClientPath(string clientPath) =>
            _pathRegex.Value.IsMatch(clientPath) && File.Exists($"{clientPath}/{_dofusExeName}");
    }
}
