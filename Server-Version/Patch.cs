using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Drive.v2;

namespace CrystalHomeSystems
{
    class Patch
    {
        public Patch(bool googleDriveTest)
        {
            if (googleDriveTest)
                googleDrivePatch();
            else
            {

            }

        }

        private void googleDrivePatch()
        {
            string url = "https://docs.google.com/document/export?format=txt&id=1Sw7JQMHHeURAlHRhuWFt58uQ7dSoUPfvjg6L-FoHgOo&token=AC4w5VjMAESLDOUgnUQxLHnUe05MV0BcXA%3A1435202040819";
            string filePath = "C:\\temp-crystal-version.txt";
  
            using (var client = new System.Net.WebClient())
            {
                client.DownloadFile(url, filePath);
            }
        }
    }
}
