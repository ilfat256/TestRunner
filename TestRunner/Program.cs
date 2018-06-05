using Libraries;
using Libraries.Log;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestRunnerCli
{
    class Program
    {
        private static int _runnerCount = Environment.ProcessorCount;
        private static string _jobFile = string.Empty;
        private static readonly ILogger _log = ConcurrentLogAccumulator.GetLogger;

        static void Main(string[] args)
        {
            ParseArgs(args);
            Console.WriteLine("Runnners count : {0}", _runnerCount);

            try
            {
                var tests = TestLoader.FromCSV(_jobFile);
                var processManager = new ProcessJobManager("cmd", DoBeforeProcess, DoAfterProcess);

                var runner = new MultiThreadRunner<Command>(_runnerCount, processManager);
                runner.SetCommands(tests);
                var results = runner.Start();

                Console.WriteLine("Execution complete!");
            }
            catch (Exception ex)
            {
                _log.Log($"{nameof(Main)} throws", ex);
            }
            finally
            {
                var logs = _log.Release();
                if (logs.Count > 0)
                {
                    File.WriteAllLines($"log_{DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")}.txt", logs);
                }
            }

        }

        private static void ParseArgs(string[] args)
        {
            PrintHelpMessage(args);
            ParseOptions(args);
        }

        private static void PrintHelpMessage(string[] args)
        {
            if (args.Count() == 0 || args[0] == "help")
            {
                Console.WriteLine("Usage: runtests [-r count] [-j fileName]");
                Console.WriteLine("Options: ");
                Console.WriteLine("-r count       runners count, buy default, it is calculated as number of logical cores on the system. The maximum allowed number should be twice as the default one");
                Console.WriteLine("-j fileName    a CSV file that includes transcription of all the tests");

                Environment.Exit(0);
            }
        }

        private static void ParseOptions(string[] args)
        {
            string jobFile = string.Empty;
            for (int i = 0; i < args.Count(); i = i + 2)
            {
                TrySetRunnersCount(args[i], args[i + 1]);
                TrySetJobFileName(args[i], args[i + 1]);
            }
        }

        private static void TrySetRunnersCount(string key, string value)
        {
            if (key == "-r")
            {
                var runnersCountOption = Int32.Parse(value);
                if (runnersCountOption != 0)
                {
                    if (runnersCountOption > _runnerCount * 2)
                    {
                        _runnerCount = _runnerCount * 2;
                    }
                    else
                    {
                        _runnerCount = runnersCountOption;
                    }
                }
            }
        }

        private static void TrySetJobFileName(string key, string value)
        {
            if (key == "-j")
            {
                _jobFile = value;
            }
        }


        private static void DoBeforeProcess(Command command)
        {
            Console.WriteLine("{0}: Test Id: {1} Taken", DateTime.Now, command.Id);
        }

        private static void DoAfterProcess(Command command)
        {
            Console.WriteLine("{0}: Test Id: {1} Finished", DateTime.Now, command.Id);
        }
    }
}
