

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Preparator.Models
{
    public class PresetItem : INotifyPropertyChanged
    {
        public bool _isSelected;
        public string Name { get; set; }
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}


