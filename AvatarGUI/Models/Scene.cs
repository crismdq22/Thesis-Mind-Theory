using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvatarGUI.Models
{
    public class Scene
    {
        public int id { get; set; }
        public float offsetAudio { get; set; } = 3;
        public float offsetSubtitle { get; set; } = 3;
        public string backgroundColor { get; set; }
        public int characterMode { get; set; }
        public int narratorMode { get; set; }
        public List<PrefabInfo> prefabs { get; set; } = new List<PrefabInfo>(4);
        public List<Step> steps { get; set; } = new List<Step>();

        public MementoScene saveMemento()
        {
            return new MementoScene(this);
        }

        public void restoreMemento(MementoScene memento)
        {
            id = memento.id;
            offsetAudio = memento.offsetAudio;
            offsetSubtitle = memento.offsetSubtitle;
            backgroundColor = memento.backgroundColor;
            characterMode = memento.characterMode;
            narratorMode = memento.narratorMode;
            prefabs = memento.prefabs;
            steps = memento.steps;
        }

        public class MementoScene
        {
            public int id { get; }
            public float offsetAudio { get; }
            public float offsetSubtitle { get; }
            public string backgroundColor{get;}
            public int characterMode { get; }
            public int narratorMode { get; }
            public List<PrefabInfo> prefabs { get; }
            public List<Step> steps { get; }

            public MementoScene(Scene scene)
            {
                id = scene.id;
                offsetAudio = scene.offsetAudio;
                offsetSubtitle = scene.offsetSubtitle;
                backgroundColor = scene.backgroundColor;
                characterMode = scene.characterMode;
                narratorMode = scene.narratorMode;
                prefabs = new List<PrefabInfo>();
                scene.prefabs.ForEach(prefab => prefabs.Add((PrefabInfo)prefab.Clone()));
                steps = new List<Step>();
                scene.steps.ForEach(step => steps.Add((Step)step.Clone()));
            }
        }
    }
}
