using AvatarGUI.Models;
using AvatarGUI.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvatarGUI
{
    sealed class JSONViewModelConverter
    {
        private static readonly JSONViewModelConverter instance = new JSONViewModelConverter();
        
        private JSONViewModelConverter()
        {
        }

        public static JSONViewModelConverter Instance
        {
            get
            {
                return instance;
            }
        }

        public SerializerHelper ViewModelToJson(SceneListViewModel viewmodel)
        {
            SceneList sceneList = new SceneList();
            sceneList.AudioFolderName = viewmodel.AudioFolderName;
            foreach (SceneViewModel sceneViewModel in viewmodel.SceneList)
             {
                if (sceneViewModel.Background == null)
                {
                    return new SerializerHelper(SerializerHelper.SERIALIZATION_FAIL,
                                string.Format("La escena {0} no tiene un escenario asignado.", viewmodel.SceneList.IndexOf(sceneViewModel) + 1));
                }
                for (int i= 0; i<sceneViewModel.scene.steps.Count; i++)
                {
                    Step step = sceneViewModel.scene.steps[i];
                    if (step.actor !=Constants.NARRATOR)
                    {
                        if (sceneViewModel.scene.prefabs[step.actor].modelName == Constants.PREFAB_VACIO)
                        {
                            return new SerializerHelper(SerializerHelper.SERIALIZATION_FAIL,
                                string.Format("La escena {0} tiene en el paso {1} un actor sin asignar.", viewmodel.SceneList.IndexOf(sceneViewModel)+1,i+1));
                        }
                    }
                    if (step.actor == Constants.NARRATOR && (sceneViewModel.scene.narratorMode == Constants.AUDIOMODE || sceneViewModel.scene.narratorMode == Constants.TEXTAUDIOMODE)
                        || step.actor != Constants.NARRATOR && (sceneViewModel.scene.characterMode == Constants.AUDIOMODE || sceneViewModel.scene.characterMode == Constants.TEXTAUDIOMODE))
                    {
                        if (step.audioName == null || viewmodel.AudioFolderName != null && !step.audioName.StartsWith(viewmodel.AudioFolderName) && viewmodel.AudioFolderName != "")
                        {
                            return new SerializerHelper(SerializerHelper.SERIALIZATION_FAIL, 
                                string.Format("La escena {0} tiene en el paso {1} ningun audio asignado, siendo que esta en modo de uso de audio."
                                , viewmodel.SceneList.IndexOf(sceneViewModel)+1,i+1));
                        }
                    }                   
                }
                sceneViewModel.scene.prefabs.RemoveAll(prefab => prefab.modelName == Constants.PREFAB_VACIO);
                sceneList.scenes.Add(sceneViewModel.scene);      
             }
            return new SerializerHelper(SerializerHelper.SERIALIZATION_SUCCESS,JsonConvert.SerializeObject(sceneList));
        }

        public void JsonToViewModel(SceneListViewModel sceneListViewModel, string fileContent)
        {
            SceneList sceneList = JsonConvert.DeserializeObject<SceneList>(fileContent);
            sceneListViewModel.SceneList.Clear();
            sceneListViewModel.AudioFolderName = sceneList.AudioFolderName;
            foreach (Scene scene in sceneList.scenes)
            {
                sceneListViewModel.SceneList.Add(new SceneViewModel(sceneListViewModel,scene));
            }
        }

    }
}
