using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Libraries
{
    public class ProcessJobManager : IJobManager<Command>
    {
        private readonly Action<object> _outputDataAction;
        private readonly Action<object> _outputErrAction;
        private readonly Action<Command> _beforeProcess;
        private readonly Action<Command> _afterProcess;
        private readonly string _app;

        public ProcessJobManager(string commandExecutorApplicationName, Action<Command> beforeProcess = null, Action<Command> afterProcess = null, Action<object> outputErrAction = null, Action<object> outputDataAction = null)
        {
            _outputDataAction = outputDataAction;
            _outputErrAction = outputErrAction;
            _beforeProcess = beforeProcess;
            _afterProcess = afterProcess;
            _app = commandExecutorApplicationName;
        }

        public bool Run(Command command)
        {
            try
            {
                var code = Task.Run(async () =>
                {
                    _beforeProcess?.Invoke(command);
                    var processCode = await RunProcessAsync(_app, command.Cmd);
                    _afterProcess?.Invoke(command);
                    return processCode;

                }).Result;

                return code == 0;
            }
            catch (Exception ex)
            {
                throw new ProcessJobManagerException("ProcessJobManager throws exception", ex);
            }
        }

        public async Task<int> RunProcessAsync(string fileName, string args)
        {
            using (var process = new Process
            {
                StartInfo =
                {   FileName = fileName, Arguments = "/c " + args,
                    UseShellExecute = false, CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true },
                EnableRaisingEvents = true
            })
            {
                return await RunProcessAsync(process).ConfigureAwait(false);
            }
        }


        private Task<int> RunProcessAsync(Process process)
        {
            var tcs = new TaskCompletionSource<int>();

            process.Exited += (s, ea) => tcs.SetResult(process.ExitCode);
            if (_outputDataAction != null)
            {
                process.OutputDataReceived += (s, ea) => _outputDataAction(ea.Data);
            }

            if (_outputErrAction != null)
            {
                process.ErrorDataReceived += (s, ea) => _outputErrAction(ea.Data);
            }

            bool started = process.Start();
            if (!started)
            {
                throw new InvalidOperationException("Could not start process: " + process);
            }

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            return tcs.Task;
        }
    }
}
