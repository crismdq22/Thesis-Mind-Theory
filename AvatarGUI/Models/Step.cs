using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvatarGUI.Models
{
    public class Step: ICloneable
    {
        public int actor { get; set; } = -1;
        public string textMessage { get; set; }
        public string audioName { get; set; }
        public int emotionState { get; set; }

        public object Clone()
        {
            Step step = new Step();
            step.actor = actor;
            step.textMessage = textMessage;
            step.audioName = audioName;
            step.emotionState = emotionState;
            return step;
        }
    }
}
