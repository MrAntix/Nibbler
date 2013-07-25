using System;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Antix.Nibbler.Shell.Properties;

namespace Antix.Nibbler.Shell
{
    [ComVisible(true)]
    //[COMServerAssociation(AssociationType.ClassOfExtension, "*")]
    public class CompressThisContextMenu : CompressContextMenuBase
    {
        protected override bool CanShowMenu()
        {
            return SelectedItemPaths.All(file => file.EndsWith(".png", StringComparison.OrdinalIgnoreCase));
        }

        protected override ContextMenuStrip CreateMenu()
        {
            var menu = new ContextMenuStrip();

            var image = Resources.CompressThis;
            image.MakeTransparent(Color.White);


            var compressFilesMenu = new ToolStripMenuItem("Compress")
                {
                    Image = image,
                    ImageScaling = ToolStripItemImageScaling.SizeToFit
                };

            compressFilesMenu.Click += (sender, args) => CompressFilesAsync();

            menu.Items.Add(compressFilesMenu);

            return menu;
        }
    }
}