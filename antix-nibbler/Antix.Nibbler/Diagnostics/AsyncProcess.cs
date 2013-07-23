using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Antix.Nibbler.Diagnostics
{
    public class AsyncProcess : IDisposable
    {
        readonly ProcessStartInfo _processStartInfo;
        readonly AsyncProcessProgress _progress;
        readonly Process _process;

        public AsyncProcess(
            ProcessStartInfo processStartInfo,
            AsyncProcessProgress progress)
        {
            _processStartInfo = processStartInfo;
            _progress = progress;

            _processStartInfo.UseShellExecute = false;
            _processStartInfo.CreateNoWindow = true;
            _processStartInfo.RedirectStandardOutput = true;
            _processStartInfo.RedirectStandardError = true;

            _process = new Process
                {
                    StartInfo = _processStartInfo,
                    EnableRaisingEvents = true
                };

            _process.OutputDataReceived += (sender, args) =>
                {
                    if (args.Data != null)
                    {
                        _progress.Output.Add(args.Data);
                    }
                };

            _process.ErrorDataReceived += (sender, args) =>
                {
                    if (args.Data != null)
                    {
                        _progress.Error.Add(args.Data);
                    }
                };
        }

        public async Task<AsyncProcessResults> RunAsync()
        {
            var tcs = new TaskCompletionSource<AsyncProcessResults>();

            _process.Exited +=
                (sender, args) => tcs.TrySetResult(
                    new AsyncProcessResults(_process, _progress));

            _progress.RegisterCancel(() =>
                {
                    tcs.TrySetCanceled();
                    if (!_process.HasExited)
                        _process.CloseMainWindow();
                });

            if (!_process.Start())
            {
                tcs.TrySetException(
                    new InvalidOperationException("Process didn't start"));
            }

            _process.BeginOutputReadLine();
            _process.BeginErrorReadLine();

            return await tcs.Task;
        }

        public void Dispose()
        {
            _progress.Cancel();
        }
    }
}