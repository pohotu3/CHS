using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Apis.Drive.v2;

namespace CrystalHomeSystems
{
    class Patch
    {
        private string url = "";

        public Patch(bool googleDriveTest)
        {
            if (MainWindow.systemBuild == "PC_Only_Build")
                url = "https://docs.google.com/document/export?format=txt&id=1Sw7JQMHHeURAlHRhuWFt58uQ7dSoUPfvjg6L-FoHgOo&token=AC4w5VjC4StKDpdZjdBa-bbiN7YgaDBLcA%3A1435202231644";
            else if (MainWindow.systemBuild == "Module_Build")
                url = "https://docs.google.com/document/u/0/export?format=txt&id=1wTwNa3EXPcfevF8xa1-s09dQsuske84NhW9uMREaE-I&token=AC4w5ViDdraSLrv71yfFu11fumEM7aS2_Q%3A1435202354348";

            init();
        }

        private void init()
        {
            string filePath = "C:\\temp-crystal-version.txt";

            using (var client = new System.Net.WebClient())
            {
                client.DownloadFile(url, filePath);
            }

            string text = System.IO.File.ReadAllText(filePath);
            if (text != MainWindow.systemVersion)
                needsPatch();
            try
            {
                System.IO.File.Delete(filePath);
            }
            catch (Exception e) { }
        }

        private void needsPatch()
        {
            MessageBox.Show("fail");
        }
    }
}
