using System.Threading.Tasks;
using SharpShell.SharpContextMenu;

namespace Antix.Nibbler.Shell
{
    public abstract class CompressContextMenu : SharpContextMenu
    {
        readonly NibblerService _nibblerService = new NibblerService();

        public async Task CompressFilesToSingleAsync()
        {
            await _nibblerService.CompressAsync(SelectedItemPaths, new NibblerCompressOptions
                {
                    CompressedFile = "{0}.min{1}"
                });
        }

        public async Task CompressFilesToMinAsync()
        {
            await _nibblerService.CompressAsync(SelectedItemPaths, new NibblerCompressOptions
                {
                    CompressedFilesPattern = "{0}.min{1}"
                });
        }

        public async Task CompressFilesAsync()
        {
           await  _nibblerService.CompressAsync(SelectedItemPaths, new NibblerCompressOptions());
        }
    }
}