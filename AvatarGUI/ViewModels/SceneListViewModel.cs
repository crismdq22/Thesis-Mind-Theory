using AvatarGUI.Models;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AvatarGUI.ViewModels
{
    public class SceneListViewModel : INotifyPropertyChanged
    {
        private SceneEditWindow sceneWindow;
        private MainWindow mainWindow;
        private TCPAgent tCPAgent;

        public ObservableCollection<SceneViewModel> SceneList { get; set; } = new ObservableCollection<SceneViewModel>();

        public List<string> AvailableBackgrounds { get; set; }

        public Dictionary<int,string> Modes { get; set; } = new Dictionary<int, string>();

        public List<string> Prefabs { get; set; }

        public string RemoteIP { get; set; }

        public string RemoteFolder { get; set; }

        private string _audioFolderName;
        public string AudioFolderName {
            get
            {
                return _audioFolderName;
            }
            set
            {
                _audioFolderName = value;
                OnPropertyChanged("AudioFolderName");
            } 
        }

        private string RemoteSharedFolder;

        private ICommand _addSceneCommand;
        public ICommand AddSceneCommand
        {
            get
            {
                return _addSceneCommand;
            }
            set
            {
                _addSceneCommand = value;
            }
        }

        private ICommand _saveRoutineCommand;
        public ICommand SaveRoutineCommand
        {
            get
            {
                return _saveRoutineCommand;
            }
            set
            {
                _saveRoutineCommand = value;
            }
        }

        private ICommand _loadRoutineCommand;
        public ICommand LoadRoutineCommand
        {
            get
            {
                return _loadRoutineCommand;
            }
            set
            {
                _loadRoutineCommand = value;
            }
        }

        private ICommand _playCommand;
        public ICommand PlayCommand
        {
            get
            {
                return _playCommand;
            }
            set
            {
                _playCommand = value;
            }
        }

        private ICommand _pauseCommand;
        public ICommand PauseCommand
        {
            get
            {
                return _pauseCommand;
            }
            set
            {
                _pauseCommand = value;
            }
        }

        private ICommand _stopCommand;

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand StopCommand
        {
            get
            {
                return _stopCommand;
            }
            set
            {
                _stopCommand = value;
            }
        }

        public SceneListViewModel(MainWindow mainWindow)
        {
            tCPAgent = new TCPAgent();
            this.mainWindow = mainWindow;
            Prefabs = new List<string>();
            AddSceneCommand = new RelayCommand(new Action<object>(AddScene));
            SaveRoutineCommand = new RelayCommand(new Action<object>(SaveRoutine));
            LoadRoutineCommand = new RelayCommand(new Action<object>(LoadRoutine));
            PlayCommand = new RelayCommand(new Action<object>(PlayRoutine));
            PauseCommand = new RelayCommand(new Action<object>(PauseRoutine));
            StopCommand = new RelayCommand(new Action<object>(StopRoutine));
            Modes.Add(Constants.TEXTMODE, "Texto");
            Modes.Add(Constants.AUDIOMODE, "Audio");
            Modes.Add(Constants.TEXTAUDIOMODE, "Texto + Audio");
            if (!File.Exists(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\Recursos.json")) {
                System.Windows.MessageBox.Show("No se ha encontrado el archivo Recursos.json en la carpeta " +
                    Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName));
            }
            else
            {
                Prefabs = ResourceLoader.Instance.GetPrefabs();
                Prefabs.Add(Constants.PREFAB_VACIO);
                AvailableBackgrounds = ResourceLoader.Instance.GetBackgrounds();
            }
        }

        private void ShowIntegrityErrors(string routine)
        {
            System.Windows.MessageBox.Show(routine);
        }

        private void SaveRoutine(object obj)
        {
            if (JSONViewModelConverter.Instance.ViewModelToJson(this).state)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "*.json|*.json";
                if (saveFileDialog.ShowDialog() == true)
                    File.WriteAllText(saveFileDialog.FileName, JSONViewModelConverter.Instance.ViewModelToJson(this).data);
            }
            else
            {
                ShowIntegrityErrors(JSONViewModelConverter.Instance.ViewModelToJson(this).data);
            }                            
        }

        private void LoadRoutine(object obj)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "*.json|*.json";
            string fileContent;
            if (openFileDialog.ShowDialog() == true)
            {
                fileContent = File.ReadAllText(openFileDialog.FileName);
                try
                {
                    JSONViewModelConverter.Instance.JsonToViewModel(this, fileContent);
                }catch(Exception e)
                {
                    MessageBox.Show("La rutina posee un formato invalido.");
                }
                
            }
        }

        private bool IsIPValid(IPAddress iPAddress)
        {
            try
            {
                Ping ping = new Ping();
                PingReply pong = ping.Send(iPAddress);
                if (pong.Status == IPStatus.Success)
                {
                    return true;
                }
                return false;
            }
            catch (PingException e)
            {
                return false;
            }           
        }

        //State pattern para play pause y stop

        private void PlayRoutine(object obj)
        {
            IPAddress iPAddress;
            if (RemoteIP != null && IPAddress.TryParse(RemoteIP, out iPAddress) && IsIPValid(iPAddress))
            {
                if (RemoteFolder!=null && RemoteFolder.Length > 2)
                { 
                    RemoteSharedFolder = @"\\" + @RemoteIP + @RemoteFolder.Substring(2); 
                    if (Directory.Exists(RemoteSharedFolder) && Directory.GetDirectories(RemoteSharedFolder).Contains(RemoteSharedFolder+@"\TesisTOM_Data"))
                    {
                            if (tCPAgent.isConnected)
                            {
                                tCPAgent.SendMessage(Constants.PLAY);
                            }
                            else
                            {
                                if (JSONViewModelConverter.Instance.ViewModelToJson(this).state)
                                {
                                    File.WriteAllText(@RemoteSharedFolder + @"\TesisTOM_Data\StreamingAssets\data.json", JSONViewModelConverter.Instance.ViewModelToJson(this).data);
                                    tCPAgent.ConnectServer(RemoteIP);
                                    tCPAgent.SendMessage(Constants.PLAY);
                                }
                                else
                                {
                                    ShowIntegrityErrors(JSONViewModelConverter.Instance.ViewModelToJson(this).data);
                                }
                            }

                    }
                    else
                    {
                         MessageBox.Show("Directorio remoto no valido");
                    }
                }
                else                    
                  MessageBox.Show("Directorio remoto no valido");
            }
            else
                MessageBox.Show("IP no Valida");
        }

        private void PauseRoutine(object obj)
        {
            if (RemoteSharedFolder != null && Directory.Exists(@RemoteSharedFolder))
            {
                if (tCPAgent.isConnected)
                {
                    tCPAgent.SendMessage(Constants.PAUSE);
                }
                else
                {
                    MessageBox.Show("Conexion con el reproductor no establecida.");
                }     
            }
            else
                MessageBox.Show("Directorio no valido");
        }

        private void StopRoutine(object obj)
        {
            if (RemoteSharedFolder != null && Directory.Exists(@RemoteSharedFolder))
            {
                if (tCPAgent.isConnected)
                {
                    tCPAgent.SendMessage(Constants.STOP);
                }
                else
                {
                    MessageBox.Show("Conexion con el reproductor no establecida.");
                }
            }
            else
                MessageBox.Show("Directorio no valido");
        }

        public int getNumeroEscena(SceneViewModel sceneViewModel)
        {
            return SceneList.IndexOf(sceneViewModel);
        }

        public void EditScene(SceneViewModel sceneViewModel)
        {
            sceneWindow = new SceneEditWindow(sceneViewModel);
            sceneWindow.ShowDialog();
        }

        public void MoveSceneUp(SceneViewModel sceneViewModel)
        {
            if (getNumeroEscena(sceneViewModel) > 0)
            {
                SwapScenes(getNumeroEscena(sceneViewModel), getNumeroEscena(sceneViewModel) - 1);
            }
        }

        public void MoveSceneDown(SceneViewModel sceneViewModel)
        {
            if (getNumeroEscena(sceneViewModel) < SceneList.Count - 1)
            {
                SwapScenes(getNumeroEscena(sceneViewModel), getNumeroEscena(sceneViewModel) + 1);
            }
        }

        private void SwapScenes(int firstIndex, int secondIndex)
        {
            SceneViewModel aux = SceneList[firstIndex];
            SceneList[firstIndex] = SceneList[secondIndex];
            SceneList[secondIndex] = aux;
        }

        public void DeleteScene(SceneViewModel viewModel)
        {
            sceneWindow.Close();
            SceneList.Remove(viewModel);
        }
        public void AddScene(object obj)
        {
            SceneList.Add(new SceneViewModel(this, null));
        }

        public void RefreshScenes()
        {
            mainWindow.RefreshScenes();
        }
    }
}
