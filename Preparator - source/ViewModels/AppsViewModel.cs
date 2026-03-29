using Preparator.Helpers;
using Preparator.Models;
using Preparator.Views;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows.Data;

namespace Preparator.ViewModels
{
    public class AppsViewModel : BaseViewModel
    {
        public ObservableCollection<AppItem> Apps { get; set; }

        public ICollectionView AppsView { get; set; }
        public ICommand ToggleAppCommand { get; }
        public ICommand InstallCommand { get; }

        public AppsViewModel()
        {
            Apps = new ObservableCollection<AppItem>
            {
                // BROWSERS
                new AppItem { Name = "Chrome", NiniteId = "chrome", Category = "Browsers", IconPath = "/Assets/Icons/chrome.png" },
                new AppItem { Name = "Firefox", NiniteId = "firefox", Category = "Browsers",  IconPath = "/Assets/Icons/firefox.png" },
                new AppItem { Name = "Opera", NiniteId = "opera", Category = "Browsers",  IconPath = "/Assets/Icons/opera.png" },

                // COMMUNICATION
                new AppItem { Name = "Discord", NiniteId = "discord", Category = "Communication", IconPath = "/Assets/Icons/discord.png"},
                new AppItem { Name = "Thunderbird", NiniteId = "thunderbird", Category = "Communication", IconPath = "/Assets/Icons/thunderbird.png"},

                // GAMING
                new AppItem { Name = "Steam", NiniteId = "steam", Category = "Gaming", IconPath = "/Assets/Icons/steam.png"},
                new AppItem { Name = "Epic Games", NiniteId = "epicgames", Category = "Gaming", IconPath = "/Assets/Icons/epicgames.png"},

                // UTILITIES
                new AppItem { Name = "7-Zip", NiniteId = "7zip", Category = "Utilities", IconPath = "/Assets/Icons/7zip.png" },
                new AppItem { Name = "Everything", NiniteId = "everything", Category = "Utilities", IconPath = "/Assets/Icons/everything.png" },
                new AppItem { Name = "WizTree", NiniteId = "wiztree", Category = "Utilities", IconPath = "/Assets/Icons/wiztree.png" },
                new AppItem { Name = "RevoUninstaller", NiniteId = "revo", Category = "Utilities", IconPath = "/Assets/Icons/revo.png" },

                // SECURITY
                new AppItem { Name = "KeePass 2", NiniteId = "keepass", Category = "Security", IconPath = "/Assets/Icons/keepass.png" },

                // MEDIA
                new AppItem { Name = "VLC", NiniteId = "vlc", Category = "Media", IconPath = "/Assets/Icons/vlc.png" },
                new AppItem { Name = "Spotify", NiniteId = "spotify", Category = "Media", IconPath = "/Assets/Icons/spotify.png" },
                new AppItem { Name = "Audacity", NiniteId = "audacity", Category = "Media", IconPath = "/Assets/Icons/audacity.png" },

                // CREATIVE
                new AppItem { Name = "GIMP", NiniteId = "gimp", Category = "Creative", IconPath = "/Assets/Icons/gimp.png" },
                new AppItem { Name = "Blender", NiniteId = "blender", Category = "Creative", IconPath = "/Assets/Icons/blender.png" },

                // OFFICE
                new AppItem { Name = "LibreOffice", NiniteId = "libreoffice", Category = "Office", IconPath = "/Assets/Icons/libreoffice.png" },

                // DEV
                new AppItem { Name="VS Code", NiniteId="vscode", Category="Development", IconPath = "/Assets/Icons/vscode.png" },
                new AppItem { Name="Notepad++", NiniteId="notepadplusplus", Category="Development", IconPath = "/Assets/Icons/notepad.png" },
                new AppItem { Name="PuTTY", NiniteId="putty", Category="Development", IconPath = "/Assets/Icons/putty.png" },
                new AppItem { Name="WinSCP", NiniteId="winscp", Category="Development", IconPath = "/Assets/Icons/winscp.png" },

                // EXTRAS
                new AppItem { Name = "CDBurnerXP", NiniteId = "cdburnerxp", IconPath="/Assets/Icons/cdburnerxp.png" },
            };

            AppsView = CollectionViewSource.GetDefaultView(Apps);
            AppsView.GroupDescriptions.Add(new PropertyGroupDescription("Category"));

            ToggleAppCommand = new RelayCommand(app =>
            {
                if (app is AppItem item)
                {
                    item.IsSelected = !item.IsSelected;

                    OnPropertyChanged(nameof(Apps)); // 🔥 refresh UI
                    OnPropertyChanged(nameof(SelectedCount));
                }
            });

            foreach (var app in Apps)
            {
                app.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(AppItem.IsSelected))
                    {
                        OnPropertyChanged(nameof(SelectedCount));
                    }
                };
            }

            InstallCommand = new RelayCommand(_ => Install());
        }

        public int SelectedCount => Apps.Count(a => a.IsSelected);

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