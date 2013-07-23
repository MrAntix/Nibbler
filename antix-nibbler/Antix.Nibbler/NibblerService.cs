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
                var percentages = new int[filesCount];
                var i = 0;

                foreach (var file in filesArray)
                {
                    var fileExt = Path.GetExtension(file);
                    if (fileExt == null) continue;

                    var fileName = file.Substring(0, file.Length - fileExt.Length);

                    Action<string, int> progress = null;
                    if (options.Progress != null)
                    {
                        var fileIndex = i++;
                        progress =
                            (m, p) =>
                                {
                                    percentages[fileIndex] = p;
                                    options.Progress(
                                        string.Format("Compressing\n{0}",
                                                      string.Join(", ",
                                                                  filesArray
                                                                      .Where((f, fi) => percentages[fi] != 100)
                                                                      .Select(Path.GetFileName))),

                                        percentages.Sum()/filesCount);
                                };
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
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}