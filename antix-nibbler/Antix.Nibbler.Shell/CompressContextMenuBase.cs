﻿using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpShell.SharpContextMenu;

namespace Antix.Nibbler.Shell
{
    public abstract class CompressContextMenuBase : SharpContextMenu
    {
        readonly NibblerService _nibblerService = new NibblerService();

        public async Task CompressFilesToMinAsync()
        {
            await Compress(new NibblerCompressOptions
                {
                    CompressedFile = "{0}.min{1}"
                });
        }

        public async Task CompressFilesAsync()
        {
            await Compress(new NibblerCompressOptions());
        }

        async Task Compress(NibblerCompressOptions options)
        {
            var waitDialog = new ProgressDialog
                {
                    Title = "Antix Nibber"
                };

            waitDialog.ShowDialog();

            try
            {
                options.Progress = GetProgressAction(waitDialog);
                await _nibblerService.CompressAsync(SelectedItemPaths, options);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            waitDialog.CloseDialog();
        }

        static Func<string, int, bool> GetProgressAction(ProgressDialog waitDialog)
        {
            return (m, p) =>
                {
                    var ms = m.Split('\n');
                    waitDialog.Header = ms[0];
                    if (ms.Length > 1) waitDialog.Message = ms[1];

                    waitDialog.Value = p;

                    return !waitDialog.HasUserCancelled;
                };
        }
    }
}