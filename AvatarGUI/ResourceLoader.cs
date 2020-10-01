using AvatarGUI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvatarGUI
{
    sealed class ResourceLoader
    {
        private static readonly ResourceLoader instance 
            = new ResourceLoader(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\Recursos.json");

        private Resources resources;

        private ResourceLoader(string filePath)
        {
            resources = JsonConvert.DeserializeObject<Resources>(File.ReadAllText(filePath));
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
            foreach (Audios audio in resources.audioFolders.Where(audio => audio.folderName.StartsWith(folderName) || folderName == ""))
            {
                for (int i = 0; i < audio.audios.Count; i++)
                {
                    allAudios.Add(audio.folderName + @"/" + audio.audios[i]);
                }
            }
            return allAudios;
        }

        public List<string> GetBackgrounds()
        {
            return resources.backgrounds;
        }
    }
}
