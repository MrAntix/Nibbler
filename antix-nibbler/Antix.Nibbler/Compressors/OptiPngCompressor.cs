using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Antix.Nibbler.Diagnostics;

namespace Antix.Nibbler.Tools
{
    public class OptiPngCompressor : ICompressor
    {
        readonly string _toolsDir;

        public OptiPngCompressor(string toolsDir)
        {
            _toolsDir = toolsDir;
        }

        public async Task CompressAsync(
            string fileFrom, string fileTo,
            CompressorProgress progress)
        {
            var pngOutPath = Path
                .GetFullPath(Path.Combine(_toolsDir, @"optipng.exe"));

            var processStart = new ProcessStartInfo(
                pngOutPath, string.Format("\"{0}\" -out \"{1}\" -clobber -verbose", fileFrom, fileTo));

            var processProgress = new AsyncProcessProgress();
            var progressPercent = 0;
            double progressPercentInc = 0;
            if (progress != null)
            {
                processProgress.Error.CollectionChanged
                    += (s, e) =>
                        {
                            progress.Change(
                                string.Format("Compressing {0}", Path.GetFileName(fileFrom)),
                                progressPercent);

                            if (e.NewItems.Cast<string>()
                                 .Any(m => m.Contains("Trying:")))
                            {
                                progressPercentInc = 15;
                            }

                            // just so the user sees some progress
                            progressPercent += (int) Math.Round(progressPercentInc);
                            progressPercentInc /= 1.2;
                        };
                progress.Cancelled += (s, e) => processProgress.Cancel();
            }

            using (var process = new AsyncProcess(processStart, processProgress))
            {
                await process.RunAsync();
            }

            if (progress != null) progress.Change("Done.", 100);
        }
    }
}