using System.IO;
using System.Threading.Tasks;

namespace Antix.Nibbler.Tools
{
    public class CssCompressor : ICompressor
    {
        readonly Yahoo.Yui.Compressor.CssCompressor _compressor 
            = new Yahoo.Yui.Compressor.CssCompressor();

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