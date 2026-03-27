using Preparator.Helpers;
using Preparator.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;

namespace Preparator.ViewModels
{
    public class AppsViewModel : BaseViewModel
    {
        public ObservableCollection<AppItem> Apps { get; set; }

        public ICommand ToggleAppCommand { get; }
        public ICommand InstallCommand { get; }

        public AppsViewModel()
        {
            Apps = new ObservableCollection<AppItem>
            {
                new AppItem { Name = "Chrome", NiniteId = "chrome", IconPath="pack://application:,,,/Assets/Icons/chrome.png" },
                new AppItem { Name = "7-Zip", NiniteId = "7zip", IconPath="pack://application:,,,/Assets/Icons/7zip.png" },
                new AppItem { Name = "VLC", NiniteId = "vlc", IconPath="pack://application:,,,/Assets/Icons/vlc.png" },
                new AppItem { Name = "Notepad++", NiniteId = "notepadplusplus", IconPath="pack://application:,,,/Assets/Icons/notepad.png" },
                new AppItem { Name = "Discord", NiniteId = "discord", IconPath="pack://application:,,,/Assets/Icons/discord.png" }
            };

            ToggleAppCommand = new RelayCommand(app =>
            {
                if (app is AppItem item)
                {
                    item.IsSelected = !item.IsSelected;
                    OnPropertyChanged(nameof(Apps)); // 🔥 refresh UI
                }
            });

            InstallCommand = new RelayCommand(_ => Install());
        }

        private void Install()
        {
            var selected = Apps.Where(a => a.IsSelected).ToList();

            if (!selected.Any())
                return;

            var ids = string.Join("-", selected.Select(a => a.NiniteId));

            var url = $"https://ninite.com/{ids}/ninite.exe";

            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }
    }
}