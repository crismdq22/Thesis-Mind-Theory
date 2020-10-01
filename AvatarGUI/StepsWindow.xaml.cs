using AvatarGUI.Models;
using AvatarGUI.ViewModels;
using System;
using System.Collections;
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
using System.Windows.Shapes;

namespace AvatarGUI
{
    /// <summary>
    /// Interaction logic for StepsWindow.xaml
    /// </summary>
    public partial class StepsWindow : Window
    {
        public StepsWindow(Scene scene, SceneViewModel sceneViewModel)
        {
            InitializeComponent();
            var viewModel = new StepListViewModel(scene,this, sceneViewModel);
            DataContext = viewModel;
        }

        void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex()+1).ToString();
        }

        public void DataGrid_RowChanged()
        {
            for (int i=0; i < StepList.Items.Count; i++)
            {
                var row = StepList.ItemContainerGenerator.ContainerFromIndex(i) as DataGridRow;
                row.Header = (row.GetIndex()+1).ToString();
            }
        }

    }
}
