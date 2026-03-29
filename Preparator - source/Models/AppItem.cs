using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Preparator.Models
{
    public class AppItem : INotifyPropertyChanged
    {
        private bool _isSelected;

        public string Category { get; set; }
        public string Name { get; set; }
        public string IconPath { get; set; }
        public string NiniteId { get; set; }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}