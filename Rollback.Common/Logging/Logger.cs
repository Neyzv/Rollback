using System.Diagnostics;
using System.Reflection;
using Rollback.Common.DesignPattern.Instance;
using Rollback.Common.Initialization;

namespace Rollback.Common.Logging
{
    public class Logger : Singleton<Logger>
    {
        private readonly object _lock = new();

        private static readonly string[] _consoleLogo = new string[]
        {
            @" _____      _ _ _                _    ",
            @"| ___ \    | | | |              | |   ",
            @"| |_/ /___ | | | |__   __ _  ___| | __",
            @"|    // _ \| | | '_ \ / _` |/ __| |/ /",
            @"| |\ \ (_) | | | |_) | (_| | (__|   < ",
            @"\_| \_\___/|_|_|_.__/ \__,_|\___|_|\_\",
            "",
            ""
        };

        private static readonly ConsoleColor[] _logoColors =
        {
            ConsoleColor.DarkCyan,
            ConsoleColor.DarkRed,
            ConsoleColor.DarkGray,
            ConsoleColor.DarkGreen,
            ConsoleColor.DarkYellow,
            ConsoleColor.Green,
            ConsoleColor.Red,
            ConsoleColor.White,
        };

        public event Action? Error;

        [Initializable(InitializationPriority.Console)]
        public static void Initialize()
        {
            var consoleName = Assembly.GetEntryAssembly()?.GetName().Name?.Replace('.', ' ');
            Console.Title = $"{consoleName} - Dofus 2.0.0 by Neyzu";
            System.Console.ForegroundColor = _logoColors.ElementAt(Random.Shared.Next(_logoColors.Length));

            foreach (string line in _consoleLogo)
            {
                int pad = (Console.BufferWidth + line.Length) / 2;

                Console.WriteLine(line.PadLeft(pad));
            }

            Console.WriteLine($" {consoleName}");
            Console.WriteLine();
        }

        private void BaseLog(string? prefix, string msg, ConsoleColor color)
        {
            lock (_lock)
            {
                Console.ForegroundColor = color;
                Console.Write($"{(prefix is not null ? $"[{prefix}]\t" : string.Empty)}");
                Console.WriteLine(msg);
                Console.ResetColor();
            }
        }

        public void LogError(Exception? e = default, string? msg = default, params object[] parameters)
        {
            var finalMsg = string.Format(msg is not null ? msg! : (e is not null ? e.Message : string.Empty), parameters);
            if (e is not null)
            {
                var stackTrace = new StackTrace(e, true);
                var frames = stackTrace.GetFrames();
                for (var i = 1; i < frames.Length; i++)
                    finalMsg += $"\n{frames[i].GetFileName()} on {frames[i].GetMethod()} line {frames[i].GetFileLineNumber()}";
            }

            BaseLog("ERROR", $"Error : {finalMsg}", ConsoleColor.Red);

            Error?.Invoke();
        }

        public void LogWarn(string? msg = default, Exception? e = default, params object[] parameters)
        {
            var finalMsg = string.Format(msg is not null ? msg! : (e is not null ? e.Message : string.Empty), parameters);
            if (e is not null)
            {
                var stackTrace = new StackTrace(e, true);
                var frames = stackTrace.GetFrames();
                for (var i = 1; i < frames.Length; i++)
                    finalMsg += $"\n{frames[i].GetFileName()} on {frames[i].GetMethod()} line {frames[i].GetFileLineNumber()}";
            }

            BaseLog("WARN", $"Warning : {finalMsg}", ConsoleColor.Yellow);
        }

        public void LogInfo(string msg, params object[] parameters) =>
            BaseLog("INFO", $"Information : {string.Format(msg, parameters)}", ConsoleColor.Green);

        public void LogInit(string msg, params object[] parameters) =>
            BaseLog("INIT", $"Initialize : {string.Format(msg, parameters)}", ConsoleColor.White);

        public void Log(string msg, params object[] parameters) =>
            BaseLog(default, string.Format(msg, parameters), ConsoleColor.Gray);

        public void LogSend(object client, object message, bool details = false) =>
            BaseLog("SEND", $"Send : {(details ? message : message.GetType().Name)} to {client}", ConsoleColor.Cyan);

        public void LogReceive(object client, object message, bool details = false) =>
            BaseLog("RECV", $"Receive : {(details ? message : message.GetType().Name)} from {client}", ConsoleColor.Magenta);
    }
}
