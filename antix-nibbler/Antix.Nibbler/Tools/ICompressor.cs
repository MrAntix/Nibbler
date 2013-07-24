using System.Threading.Tasks;

namespace Antix.Nibbler.Tools
{
    public interface ICompressor
    {
        Task CompressAsync(
            string fileFrom, string fileTo,
            CompressorProgress progress);
    }
}