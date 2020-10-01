using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvatarGUI.Models
{
    class Resources
    {
        public List<string> backgrounds { get; set; }

        public List<Audios> audioFolders { get; set; }

        public List<string> characters { get; set; }
    }

    class Audios
    {
        public string folderName { get; set; }
        public List<string> audios { get; set; }
    }
}
