using System;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Antix.Nibbler.Shell.Properties;

namespace Antix.Nibbler.Shell
{
    [ComVisible(true)]
    public class CompressToMinContextMenu : CompressContextMenuBase
    {
        protected override bool CanShowMenu()
        {
            return SelectedItemPaths.All(file => file.EndsWith(".png", StringComparison.OrdinalIgnoreCase)
                                                 || file.EndsWith(".js", StringComparison.OrdinalIgnoreCase)
                                                 || file.EndsWith(".css", StringComparison.OrdinalIgnoreCase));
        }

        protected override ContextMenuStrip CreateMenu()
        {
            
            var menu = new ContextMenuStrip();

            var image = Resources.CompressToMin;
            image.MakeTransparent(Color.White);

            var compressFilesToMinMenu = new ToolStripMenuItem("Compress to .min")
            {
                Image = image,
                ImageScaling = ToolStripItemImageScaling.SizeToFit
            };

            compressFilesToMinMenu.Click += (sender, args) => CompressFilesToMinAsync();

            menu.Items.Add(compressFilesToMinMenu);

            return menu;
        }
    }
}