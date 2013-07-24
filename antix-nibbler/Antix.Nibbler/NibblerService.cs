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
        readonly ICompressor _pngCompressor;
        readonly ICompressor _jsCompressor;
        readonly ICompressor _cssCompressor;

        public NibblerService()
        {
            var codebaseDir = Path.GetDirectoryName(GetType().Assembly.CodeBase);
            if (codebaseDir == null) throw new InvalidOperationException("Cannot get codebase dir for " + GetType().Assembly.FullName);

            _rootDir = new Uri(codebaseDir).LocalPath;
            _pngCompressor = new OptiPngCompressor(Path.Combine(_rootDir, "Tools"));
            _jsCompressor = new JsCompressor();
            _cssCompressor = new CssCompressor();
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

                    fileExt = fileExt.ToLower();
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
                        case ".css":
                            tasks.Add(
                                _cssCompressor.CompressAsync(
                                    file,
                                    string.Format(pattern, fileName, fileExt),
                                    progress
                                    ));

                            break;
                        case ".js":
                            tasks.Add(
                                _jsCompressor.CompressAsync(
                                    file,
                                    string.Format(pattern, fileName, fileExt),
                                    progress
                                    ));

                            break;
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