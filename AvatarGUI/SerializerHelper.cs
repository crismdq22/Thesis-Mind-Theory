using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvatarGUI
{
    class SerializerHelper
    {
        public static bool SERIALIZATION_SUCCESS = true;
        public static bool SERIALIZATION_FAIL = false;
        public string data { get; set; }
        public bool state { get; set; }
        public SerializerHelper(bool state, string data)
        {
            this.state = state;
            this.data = data;
        }
    }
}
