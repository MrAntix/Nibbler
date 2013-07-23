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
            CompressProgress progress)
        {
            var pngOutPath = Path
                .GetFullPath(Path.Combine(_toolsDir, @"optipng.exe"));

            var processStart = new ProcessStartInfo(
                pngOutPath, string.Format("\"{0}\" -out \"{1}\" -clobber", fileFrom, fileTo));

            var processProgress = new AsyncProcessProgress();
            if (progress != null)
            {
                processProgress.Output.CollectionChanged += (s, e) =>
                    {
                        var last = e.NewItems
                                    .OfType<string>().Last()
                                    .Replace(NumberFormatInfo.CurrentInfo.PercentSymbol, "");
                        int percentage;
                        if (int.TryParse(last, NumberStyles.Any, NumberFormatInfo.CurrentInfo, out percentage))
                        {
                            progress.Percentage = percentage;
                        }

                        Console.WriteLine(last);
                    };
            }

            using (var process = new AsyncProcess(processStart, processProgress))
            {
                await process.RunAsync();
            }
        }
    }
}