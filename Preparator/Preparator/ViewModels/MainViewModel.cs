using System.Windows.Input;
using Preparator.Helpers;
using Preparator.Views;


namespace Preparator.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private object _currentView;

        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public ICommand ShowDashboardCommand { get; }
        public ICommand ShowSystemCommand { get; }

        public MainViewModel()
        {
            ShowDashboardCommand = new RelayCommand(_ =>
            CurrentView = new DashboardView());

            ShowSystemCommand = new RelayCommand(_ =>
            CurrentView = new SystemView());

            //Initial View
            CurrentView = new DashboardView();
        }
    }
}
