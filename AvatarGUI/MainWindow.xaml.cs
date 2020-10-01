using AvatarGUI.ViewModels;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AvatarGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var viewModel = new SceneListViewModel(this);
            DataContext = viewModel;
        }

        public void RefreshScenes()
        {
            CollectionViewSource.GetDefaultView(SceneList.ItemsSource).Refresh();
        }

        private void HelpButtonFolder_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Aqui debe especificarse la ruta a la carpeta donde se encuentra el reproductor de rutinas en la maquina remota." 
                + Environment.NewLine + @"Por ejemplo: C:\Usuarios\Usuario\Escritorio\TesisTom");
        }

        private void HelpButtonIp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Aqui debe especificarse la IP de la maquina remota." + Environment.NewLine + "Por ejemplo: 192.168.0.138");
        }

        private void HelpButtonRoutineName_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Este campo es opcional y sirve para delimitar la carpeta de la cual se podran seleccionar los audios para esta rutina." +
                Environment.NewLine + "Si se deja vacio, se mostraran todos los audios en el sistema." + Environment.NewLine +
                "En caso de que se creen pasos que utilizan audios de cierta carpeta, y luego se cambia este campo, se informara de un error de inconsistencia " 
                + Environment.NewLine + " ya que dichos pasos no poseeran ningun audio. Favor de revisar en caso de cambiar este campo.");
        }

        private void HelpButtonSceneList_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("En esta grilla se podran agregar y visualizar las distintas escenas que componen la rutina." + Environment.NewLine
                + "Para editar una escena, basta con hacer click sobre la misma para abrir la ventana de edicion de escena." + Environment.NewLine
                + Environment.NewLine + "Ademas se disponen los controles para reproducir, pausar y parar la rutina." + Environment.NewLine +
                " En caso de que la rutina este pausada, el boton de reproducir la reanudara." + Environment.NewLine +
                "En caso de que se presente alguna inconsistencia en la rutina, o los campos de direccion de carpeta o IP sean erroneos se notificara de dicho error.");
        }
    }
}
