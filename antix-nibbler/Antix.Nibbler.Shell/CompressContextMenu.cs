using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SharpShell.Attributes;

namespace Antix.Nibbler.Shell
{
    [ComVisible(true)]
    //[COMServerAssociation(AssociationType.ClassOfExtension, "*")]
    public class CompressContextMenu : CompressContextMenuBase
    {
        protected override bool CanShowMenu()
        {
            return SelectedItemPaths.All(file => file.EndsWith(".png", StringComparison.OrdinalIgnoreCase));
        }

        protected override ContextMenuStrip CreateMenu()
        {
            var menu = new ContextMenuStrip();

            var compressFilesMenu = new ToolStripMenuItem("Compress");

            compressFilesMenu.Click += (sender, args) => CompressFilesAsync();

            menu.Items.Add(compressFilesMenu);

            return menu;
        }
    }
}