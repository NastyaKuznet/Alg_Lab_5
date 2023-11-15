using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Alg_Lab_5.M
{
    public class DialogeOpen
    {
        public string CallFolderBrowserDialog()
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            DialogResult result = dialog.ShowDialog();
            string folder = "";
            if (result == DialogResult.OK)
                folder = dialog.SelectedPath;

            return folder;
        }
    }
}
