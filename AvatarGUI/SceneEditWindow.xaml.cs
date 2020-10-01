using AvatarGUI.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AvatarGUI
{
    /// <summary>
    /// Interaction logic for SceneEditWindow.xaml
    /// </summary>
    public partial class SceneEditWindow : Window
    {
        private SceneViewModel viewModel;

        public SceneEditWindow(SceneViewModel viewModel)
        {
            this.viewModel = viewModel;
            viewModel.window = this;
            viewModel.SaveMemento();
            viewModel.SetPrefabDropdowns();
            InitializeComponent();
            DataContext = viewModel;
        }

        void SceneEditWindow_Closing(object sender, CancelEventArgs e)
        {
            viewModel.OnWindowClose();
        }

        private void Prefab0_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Prefab0.SelectedValue.ToString() == Constants.PREFAB_VACIO)
            {
                Prefab1.SelectedValue = Constants.PREFAB_VACIO;
                Prefab2.SelectedValue = Constants.PREFAB_VACIO;
                Prefab3.SelectedValue = Constants.PREFAB_VACIO;
                Prefab1.IsEnabled = false;
                Prefab2.IsEnabled = false;
                Prefab3.IsEnabled = false;
            }
            else
            {
                Prefab1.IsEnabled = true;
            }
        }

        private void Prefab1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Prefab1.SelectedValue.ToString() == Constants.PREFAB_VACIO)
            {
                Prefab2.SelectedValue = Constants.PREFAB_VACIO;
                Prefab3.SelectedValue = Constants.PREFAB_VACIO;
                Prefab2.IsEnabled = false;
                Prefab3.IsEnabled = false;
            }
            else
            {
                Prefab2.IsEnabled = true;
            }
        }

        private void Prefab2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Prefab2.SelectedValue.ToString() == Constants.PREFAB_VACIO)
            {
                Prefab3.SelectedValue = Constants.PREFAB_VACIO;
                Prefab3.IsEnabled = false;
            }
            else
            {
                Prefab3.IsEnabled = true;
            }
        }

        public void OnSceneViewModelMount()
        {
            Prefab0_SelectionChanged(null, null);
            Prefab1_SelectionChanged(null, null);
            Prefab2_SelectionChanged(null, null);
        }

        private void HelpOffset_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Determina cuanto tiempo en segundos se espera entre cada paso de la escena. Si se trata de un audio, sera la duracion del audio mas este numero." + Environment.NewLine
                + "Si es solo texto, sera el tiempo total entre pasos.");
        }

        private void HelpBackground_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Respresenta el escenario en donde se reproducira la escena. Si se deja vacio, se notificara de tal error al Guardar o Reproducir la rutina.");
        }

        private void HelpCharacterMode_Click(object sender, RoutedEventArgs e)
        {

            MessageBox.Show("Determina el modo en el que se comunican los personajes en la rutina, solo texto en pantalla, solo clips de audios, o ambos." + Environment.NewLine
                + "En caso de utilize audios, todos los pasos asociados a los personajes deben tener un clip de audio asignado, de lo contrario se notificara de tal error al querer" +
                "Guardar o Reproducir la rutina.");
        }

        private void HelpNarratorMode_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Determina el modo en el que se comunica el narrador en la rutina, solo texto en pantalla, solo clips de audios, o ambos." + Environment.NewLine 
                +"En caso de utilize audios, todos los pasos asociados al Narrador deben tener un clip de audio asignado, de lo contrario se notificara de tal error al querer" +
                "Guardar o Reproducir la rutina.");
        }

        private void HelpCharacters_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Los personajes se mostraran de izquierda a derecha en el reproductor, tal como se definan aqui." + Environment.NewLine +
                "Cabe destacar que los personajes deben definirse de manera secuencial, es decir, primero el personaje 1, segundo personaje 2, etc." + Environment.NewLine +
                "Cuando un espacio de personaje se deja en vacio, automaticamente los siguientes se desactivan para asegurar dicha consistencia de posicionamiento." + Environment.NewLine +
                "Si cambian un personaje por otro en un mismo espacio, todos los pasos en los que actuaria el personaje anterior se transfieren automaticamente al nuevo personaje asignado.");
        }
    }
}
