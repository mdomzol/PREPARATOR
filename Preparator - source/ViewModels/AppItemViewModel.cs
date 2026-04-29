using CommunityToolkit.Mvvm.ComponentModel;
using Preparator.Models;

namespace Preparator.ViewModels
{
    public partial class AppItemViewModel : ObservableObject
    {
        public AppItem Model { get; }

        public string Name => Model.Name;
        public string IconPath => Model.IconPath;
        public string Category => Model.Category;
        public string NiniteId => Model.NiniteId;

        [ObservableProperty]
        private bool isSelected;

        public AppItemViewModel(AppItem model)
        {
            Model = model;
            isSelected = model.IsSelected;
        }

        partial void OnIsSelectedChanged(bool value)
        {
            Model.IsSelected = value;
        }
    }
}