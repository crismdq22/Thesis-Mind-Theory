using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvatarGUI.Models
{
    class SceneList
    {
        public string AudioFolderName { get; set; } = "";
        public int totalTime { get; set; } = 20;
        public List<Scene> scenes { get; set; } = new List<Scene>();
    }
}
