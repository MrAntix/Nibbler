using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Antix.Nibbler.Tools;

namespace Antix.Nibbler
{
    public class NibblerService
    {
        readonly string _rootDir;
        readonly IPngCompressor _pngCompressor;

        public NibblerService()
        {
            _rootDir = new Uri(Path.GetDirectoryName(GetType().Assembly.CodeBase)).LocalPath;
            _pngCompressor = new OptiPngCompressor(Path.Combine(_rootDir, "Tools"));
        }

        public async Task CompressAsync(
            IEnumerable<string> files, NibblerCompressOptions options)
        {
            if (options.CompressedFile != null)
            {
                // files must be same type
                throw new InvalidOperationException("Files must be same type");
            }
            else
            {
                var pattern = options.CompressedFilesPattern ?? "{0}.min{1}";
                var pngPattern = options.CompressedFilesPattern ?? "{0}{1}";

                var tasks = new List<Task>();
                var filesArray = files as string[] ?? files.ToArray();
                var filesCount = filesArray.Count();
                var fileIndex = 0;

                var percentages = new int[filesCount];

                foreach (var file in filesArray)
                {
                    var fileExt = Path.GetExtension(file);
                    if (fileExt == null) continue;

                    var fileName = file.Substring(0, file.Length - fileExt.Length);

                    CompressorProgress progress = null;
                    if (options.Progress != null)
                    {
                        progress = GetProgressObject(
                            filesArray, fileIndex++,
                            percentages,
                            options.Progress
                            );
                    }

                    switch (fileExt)
                    {
                        case ".png":

                            tasks.Add(
                                _pngCompressor.CompressAsync(
                                    file,
                                    string.Format(pngPattern, fileName, fileExt),
                                    progress
                                    )
                                );

                            break;

                        default:
                            throw new NotSupportedException(fileExt);
                    }
                }

                try
                {
                    await Task.Factory.ContinueWhenAll(tasks.ToArray(), _ => { });
                }
                catch (TaskCanceledException)
                {
                    if (options.Progress != null)
                        options.Progress("Cancelled", 100);
                }
            }
        }

        static CompressorProgress GetProgressObject(
            ICollection<string> filesArray, int fileIndex,
            IList<int> percentages,
            Func<string, int, bool> optionsProgress)
        {
            var progress = new CompressorProgress();

            progress.Change = (m, p) =>
                {
                    percentages[fileIndex] = p;

                    var message =
                        string.Format("Compressing\n{0}",
                                      string.Join(", ",
                                                  filesArray
                                                      .Where((f, fi) => percentages[fi] != 100)
                                                      .Select(Path.GetFileName)));

                    if (!optionsProgress(
                        message, percentages.Sum()/filesArray.Count))
                    {
                        progress.Cancel();
                    }
                };

            return progress;
        }
    }
}