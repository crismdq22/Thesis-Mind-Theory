using AvatarGUI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AvatarGUI.ViewModels
{
    public class SceneViewModel: INotifyPropertyChanged
    {
        private SceneListViewModel parentVM;
        private Scene.MementoScene mementoScene;
        public Scene scene { get; set; }
        public int NumeroEscena { get; set; }
        public int getNumeroEscena { get { return parentVM.getNumeroEscena(this) + 1; } }
        public SceneEditWindow window { get; set; }

        public string AudioFolderName
        {
            get
            {
                return parentVM.AudioFolderName;
            }
        }

        public List<string> AvailableBackgrounds {
            get
            {
                return parentVM.AvailableBackgrounds;
            }
        }

        public List<string> AvailablePrefabs {
            get
            {
                return ResourceLoader.Instance.GetPrefabs();
            }
        }

        public Dictionary<int, string> Modes
        {
            get
            {
                return parentVM.Modes;
            }
        }

        public SceneViewModel(SceneListViewModel vm, Scene scene)
        {
            parentVM = vm;
            EditSceneCommand= new RelayCommand(new Action<object>(EditScene));
            DeleteCommand = new RelayCommand(new Action<object>(DeleteScene));
            EditStepsCommand = new RelayCommand(new Action<object>(EditSteps));
            SaveChangesCommand = new RelayCommand(new Action<object>(SaveChanges));
            if (scene == null)
            {
                this.scene = new Scene();
            }
            else
            {
                this.scene = scene;
            }
            while (this.scene.prefabs.Count < 4)
            {
                AddPrefab();
            }
            foreach(PrefabInfo prefabInfo in this.scene.prefabs)
            {
                Prefabs.Add(prefabInfo.modelName);
                PrefabPositions.Add(prefabInfo.position);
            }
        }

        public void SaveMemento()
        {
            mementoScene = this.scene.saveMemento();
        }

        private ICommand _editStepsCommand;
        public ICommand EditStepsCommand
        {
            get
            {
                return _editStepsCommand;
            }
            set
            {
                _editStepsCommand = value;
            }
        }

        private ICommand _editSceneCommand;
        public ICommand EditSceneCommand
        {
            get
            {
                return _editSceneCommand;
            }
            set
            {
                _editSceneCommand = value;
            }
        }
        private ICommand _saveChangesCommand;
        public ICommand SaveChangesCommand
        {
            get
            {
                return _saveChangesCommand;
            }
            set
            {
                _saveChangesCommand = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private ICommand _deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                return _deleteCommand;
            }
            set
            {
                _deleteCommand = value;
            }
        }

        public string AllInfo
        {
            get
            {
                return string.Format("Escena Numero={5} ; Pausa entre Audios={0} ; Escenario={1} ; Modo Personajes={2} ; Modo Narrador={3} ; Numero de Personajes={4}", 
                    scene.offsetAudio, scene.backgroundColor,parentVM.Modes[scene.characterMode],parentVM.Modes[scene.narratorMode],scene.prefabs.FindAll(p => p.modelName!=Constants.PREFAB_VACIO).Count,this.getNumeroEscena);
            }
        }

        public float Offset
        {
            get { return scene.offsetAudio; }
            set { scene.offsetAudio = value; }
        }

        public string Background
        {
            get { return scene.backgroundColor; }
            set { scene.backgroundColor = value; }
        }

        public int CharacterMode
        {
            get { return scene.characterMode; }
            set { scene.characterMode = value; }
        }
        
        public int NarratorMode
        {
            get { return scene.narratorMode; }
            set { scene.narratorMode = value; }
        }

        public List<string> Prefabs { get; set; } = new List<string>();

        public List<float> PrefabPositions { get; set; } = new List<float>();

        private void SetScenePrefabInfo()
        {
            for (int i = 0; i < scene.prefabs.Count; i++)
            {
                scene.prefabs[i].position = PrefabPositions[i];
                scene.prefabs[i].modelName = Prefabs[i];
            }
        }

        public void SetPrefabDropdowns()
        {
            for (int i = 0; i < scene.prefabs.Count; i++)
            {
                PrefabPositions[i] = scene.prefabs[i].position;
                Prefabs[i] = scene.prefabs[i].modelName;
            }
        }

        public void AddPrefab()
        {
            scene.prefabs.Add(new PrefabInfo(Constants.PREFAB_VACIO,0));
        }

        private void EditScene(object obj)
        {
            parentVM.EditScene(this);
        }

        private void DeleteScene(object obj)
        {
            parentVM.DeleteScene(this);
        }

        private void EditSteps(object obj)
        {
            SetScenePrefabInfo();
            StepsWindow stepsWindow = new StepsWindow(scene, this);
            stepsWindow.ShowDialog();
        }

        private void SaveChanges(object obj)
        {
            SetScenePrefabInfo();
            mementoScene = scene.saveMemento();
        }

        public void OnWindowClose()
        {
            scene.restoreMemento(mementoScene);
            parentVM.RefreshScenes();
        }
    }
}
