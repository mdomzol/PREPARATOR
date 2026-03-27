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
        public ICommand ShowPowerCommand { get; }
        public ICommand ShowAppsCommand { get; }

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

            ShowPowerCommand = new RelayCommand(_ =>
            {
                CurrentView = new PowerView();
                SelectedView = "Power";
            });

            ShowAppsCommand = new RelayCommand(_ =>
            {
                CurrentView = new AppsView();
                SelectedView = "Apps";
            });

            CurrentView = new DashboardView();
            SelectedView = "Dashboard";
        }
    }
}