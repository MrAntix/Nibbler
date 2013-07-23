using System.Diagnostics;
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
                    CompressedFile = "{0}.min{1}",
                    Progress = (m, p) => Debug.WriteLine("{0} {1}%", m, p)
                });
        }

        public async Task CompressFilesToMinAsync()
        {
            var waitDialog = new ProgressDialog
                {
                    Title = "Antix Nibber"
                };

            waitDialog.ShowDialog();

            await _nibblerService.CompressAsync(SelectedItemPaths, new NibblerCompressOptions
                {
                    CompressedFilesPattern = "{0}.min{1}",
                    Progress = (m, p) =>
                        {
                            var ms = m.Split('\n');
                            waitDialog.Header = ms[0];
                            waitDialog.Message = ms[1];
                            waitDialog.Value = p;
                        }
                });

            waitDialog.CloseDialog();
        }

        public async Task CompressFilesAsync()
        {
            await _nibblerService.CompressAsync(SelectedItemPaths, new NibblerCompressOptions
                {
                    Progress = (m, p) => Debug.WriteLine("{0} {1}%", m, p)
                });
        }
    }
}