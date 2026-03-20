using System.Windows;
using Preparator.ViewModels;

namespace Preparator
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //ViewModel LINK
            DataContext = new MainViewModel();
        }
    }
}