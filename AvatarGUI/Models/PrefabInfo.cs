using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvatarGUI.Models
{
    public class PrefabInfo: ICloneable
    {
        public string modelName;
        public float position;

        public PrefabInfo(string model, float position)
        {
            modelName = model;
            this.position = position;
        }

        public object Clone()
        {
            return new PrefabInfo(modelName, position);
        }
    }
}
