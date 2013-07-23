using System;
using System.Collections.Generic;
using System.IO;
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

                foreach (var file in files)
                {
                    var fileExt = Path.GetExtension(file);
                    var fileName = file.Substring(0, file.Length - fileExt.Length);

                    var compressProgress = new CompressProgress();

                    switch (fileExt)
                    {
                        case ".png":
                            tasks.Add(
                                _pngCompressor.CompressAsync(
                                    file,
                                    string.Format(pngPattern, fileName, fileExt),
                                    compressProgress
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