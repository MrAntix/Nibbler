using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Antix.Nibbler.Diagnostics;

namespace Antix.Nibbler.Tools
{
    public class OptiPngCompressor : IPngCompressor
    {
        readonly string _toolsDir;

        public OptiPngCompressor(string toolsDir)
        {
            _toolsDir = toolsDir;
        }

        public async Task CompressAsync(
            string fileFrom, string fileTo,
            Action<string, int> progress)
        {
            var pngOutPath = Path
                .GetFullPath(Path.Combine(_toolsDir, @"optipng.exe"));

            var processStart = new ProcessStartInfo(
                pngOutPath, string.Format("\"{0}\" -out \"{1}\" -clobber -verbose", fileFrom, fileTo));

            var processProgress = new AsyncProcessProgress();
            //if (progress != null)
            //{
            //    processProgress.Error.CollectionChanged += (s, e) =>
            //        {
            //            var last = e.NewItems
            //                        .OfType<string>().Last();

            //            progress(last, 0);
            //        };
            //}

            if (progress != null)
                progress(
                    string.Format("Compressing {0}", Path.GetFileName(fileFrom)),
                    0);

            using (var process = new AsyncProcess(processStart, processProgress))
            {
                await process.RunAsync();
            }

            if (progress != null) progress("Done.", 100);
        }
    }
}