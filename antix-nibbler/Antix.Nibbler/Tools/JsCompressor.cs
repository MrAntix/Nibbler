using System.IO;
using System.Threading.Tasks;
using Yahoo.Yui.Compressor;

namespace Antix.Nibbler.Tools
{
    public class JsCompressor : ICompressor
    {
        readonly JavaScriptCompressor _compressor = new JavaScriptCompressor();

        public async Task CompressAsync(
            string fileFrom, string fileTo,
            CompressorProgress progress)
        {
            var fromContent = File.ReadAllText(fileFrom);

            var toContent = _compressor
                .Compress(fromContent);

            File.WriteAllText(fileTo, toContent);

            if (progress != null) progress.Change("Done.", 100);
        }
    }
}