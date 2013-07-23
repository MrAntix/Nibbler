using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SharpShell.Attributes;

namespace Antix.Nibbler.Shell
{
    [ComVisible(true)]
    [COMServerAssociation(AssociationType.ClassOfExtension, ".js", ".css")]
    public class JsCssCompressContextMenu : CompressContextMenu
    {
        protected override bool CanShowMenu()
        {
            return SelectedItemPaths.All(file => file.EndsWith(".js"))
                   || SelectedItemPaths.All(file => file.EndsWith(".css"));
        }

        protected override ContextMenuStrip CreateMenu()
        {
            var menu = new ContextMenuStrip();

            var compressFilesToMinMenu = new ToolStripMenuItem("Compress to .min");
            var compressFilesToMenu = new ToolStripMenuItem("Compress to ...");

            compressFilesToMinMenu.Click += (sender, args) => CompressFilesToMinAsync();
            compressFilesToMenu.Click += (sender, args) => CompressFilesToSingleAsync();

            menu.Items.Add(compressFilesToMinMenu);
            menu.Items.Add(compressFilesToMenu);

            return menu;
        }
    }
}