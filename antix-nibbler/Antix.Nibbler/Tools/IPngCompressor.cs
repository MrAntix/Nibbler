using System.Threading.Tasks;

namespace Antix.Nibbler.Tools
{
    public interface IPngCompressor
    {
        Task CompressAsync(
            string fileFrom, string fileTo,
            CompressProgress progress);
    }
}