using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
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
                    Progress = (m, p) =>
                        {
                            Debug.WriteLine("{0} {1}%", m, p);
                            return false;
                        }
                });
        }

        public async Task CompressFilesToMinAsync()
        {
            try
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
                                if (ms.Length > 1) waitDialog.Message = ms[1];

                                waitDialog.Value = p;

                                return !waitDialog.HasUserCancelled;
                            }
                    });

                waitDialog.CloseDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public async Task CompressFilesAsync()
        {
            await _nibblerService.CompressAsync(SelectedItemPaths, new NibblerCompressOptions
                {
                    Progress = (m, p) =>
                        {
                            Debug.WriteLine("{0} {1}%", m, p);
                            return false;
                        }
                });
        }
    }
}