using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SharpShell.Attributes;

namespace Antix.Nibbler.Shell
{
    [ComVisible(true)]
    [COMServerAssociation(AssociationType.ClassOfExtension, ".png")]
    public class PngCompressContextMenu : CompressContextMenu
    {
        protected override bool CanShowMenu()
        {
            return SelectedItemPaths.All(file => file.EndsWith(".png"));
        }

        protected override ContextMenuStrip CreateMenu()
        {
            var menu = new ContextMenuStrip();

            var compressFilesMenu = new ToolStripMenuItem("Compress");
            var compressFilesToMinMenu = new ToolStripMenuItem("Compress to .min");

            compressFilesMenu.Click += (sender, args) => CompressFilesAsync();
            compressFilesToMinMenu.Click += (sender, args) => CompressFilesToMinAsync();

            menu.Items.Add(compressFilesMenu);
            menu.Items.Add(compressFilesToMinMenu);

            return menu;
        }
    }
}