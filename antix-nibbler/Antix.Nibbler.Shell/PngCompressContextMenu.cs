using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Antix.Nibbler.Shell
{
    [ComVisible(true)]
    public class CompressToMinContextMenu : CompressContextMenuBase
    {
        protected override bool CanShowMenu()
        {
            return SelectedItemPaths.All(file => file.EndsWith(".png")
                                                 || file.EndsWith(".js")
                                                 || file.EndsWith(".css"));
        }

        protected override ContextMenuStrip CreateMenu()
        {
            var menu = new ContextMenuStrip();

            var compressFilesToMinMenu = new ToolStripMenuItem("Compress to .min");

            compressFilesToMinMenu.Click += (sender, args) => CompressFilesToMinAsync();

            menu.Items.Add(compressFilesToMinMenu);

            return menu;
        }
    }
}