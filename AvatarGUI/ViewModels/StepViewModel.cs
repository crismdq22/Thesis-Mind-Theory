using AvatarGUI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace AvatarGUI.ViewModels
{
    class StepViewModel : INotifyPropertyChanged
    {
        private StepListViewModel parentVm { get; set; }
        public Step step { get; set; }

        public StepViewModel(StepListViewModel parentVm)
        {
            this.parentVm = parentVm;
            DeleteCommand = new RelayCommand(new Action<object>(DeleteStep));
        }

        public int Actor
        {
            get { return step.actor; }
            set {
                step.actor = value;
                OnPropertyChanged("Actor");
            }
        }

        public string TextMessage
        {
            get { return step.textMessage; }
            set { step.textMessage = value; }
        }

        public string AudioName
        {
            get { return step.audioName; }
            set { step.audioName = value; }
        }

        public int EmotionState
        {
            get { return step.emotionState; }
            set { step.emotionState = value; }
        }

        private ICommand _deleteCommand;

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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

        public void DeleteStep(object obj)
        {
            parentVm.DeleteStep(this);
        }

    }
}
