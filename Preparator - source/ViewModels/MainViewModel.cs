using System.Windows.Input;
using Preparator.Helpers;
using Preparator.Views;

namespace Preparator.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private object _currentView;
        private string _selectedView;

        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public string SelectedView
        {
            get => _selectedView;
            set
            {
                _selectedView = value;
                OnPropertyChanged();
            }
        }

        public ICommand ShowDashboardCommand { get; }
        public ICommand ShowSystemCommand { get; }

        public MainViewModel()
        {
            ShowDashboardCommand = new RelayCommand(_ =>
            {
                CurrentView = new DashboardView();
                SelectedView = "Dashboard";
            });

            ShowSystemCommand = new RelayCommand(_ =>
            {
                CurrentView = new SystemView();
                SelectedView = "System";
            });

            CurrentView = new DashboardView();
            SelectedView = "Dashboard";
        }
    }
}