using AvatarGUI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AvatarGUI
{
    sealed class ResourceLoader
    {
        private static readonly ResourceLoader instance 
            = new ResourceLoader(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\Recursos.json");

        private static readonly string filepath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

        private Resources resources;

        private ResourceLoader(string filePath)
        {
            resources = JsonConvert.DeserializeObject<Resources>(File.ReadAllText(filePath));
        }

        private List<string> GetBackgroundImages()
        {
            try {
                DirectoryInfo d = new DirectoryInfo(filepath + @"\Imagenes");
                Regex reg = new Regex(@"\w*.(png|jpg)");
                FileInfo[] Files = d.GetFiles();
                List<string> images = new List<string>();
                foreach (FileInfo file in Files)
                {
                    if (reg.IsMatch(file.Name))
                        images.Add(file.Name);
                }
                return images;
            }catch (Exception e)
            {
                return new List<string>();
            }
        }

        public static ResourceLoader Instance
        {
            get
            {
                return instance;
            }
        }

        public List<string> GetPrefabs()
        {
            return resources.characters;
        }

        public List<string> GetAudios(string folderName)
        {
            List<string> allAudios = new List<string>();
            folderName = folderName == null ? "" : folderName;

            foreach (string d in Directory.GetDirectories(filepath + @"\Audio"))
            {
                if (d.Substring(d.LastIndexOf("\\")+1).Contains(folderName) || folderName == "")
                {
                    foreach (string f in Directory.GetFiles(d))
                    {
                        if (f.EndsWith(".wav"))
                            allAudios.Add(d.Substring(d.LastIndexOf("\\")+1) + @"/" + f.Substring(f.LastIndexOf("\\")+1));
                    }
                }
            }

            return allAudios;
        }

        public List<string> GetBackgrounds()
        {
            return resources.backgrounds.Concat(GetBackgroundImages()).ToList();
        }
    }
}
