using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Preparator.Models;
using Preparator.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace Preparator.ViewModels
{
    public partial class AppsViewModel : ObservableObject
    {
        // ================= SERVICES =================
        private readonly PresetService _presetService = new();
        private readonly InstallService _installService = new();
        private readonly LogService _logService = new();

        // ================= DATA =================
        public ObservableCollection<AppItemViewModel> Apps { get; }

        public ICollectionView GroupedApps { get; }

        public ObservableCollection<string> PresetNames { get; }

        public ObservableCollection<LogItem> InstallLogs { get; } = new();

        public SystemStatsViewModel Stats { get; } = new();

        // ================= STATE =================
        [ObservableProperty]
        private string searchText;

        [ObservableProperty]
        private string activePreset;

        public int SelectedCount => Apps.Count(a => a.IsSelected);

        // ================= CTOR =================
        public AppsViewModel()
        {
            // LOAD APPS
            Apps = new ObservableCollection<AppItemViewModel>(
                LoadApps().Select(a => new AppItemViewModel(a))
            );

            // PRESETS
            PresetNames = new ObservableCollection<string>(_presetService.GetPresetNames());

            // GROUPING
            GroupedApps = CollectionViewSource.GetDefaultView(Apps);
            GroupedApps.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
            GroupedApps.Filter = FilterApps;

            // SELECT COUNT UPDATE
            foreach (var app in Apps)
            {
                app.PropertyChanged += (_, e) =>
                {
                    if (e.PropertyName == nameof(AppItemViewModel.IsSelected))
                        OnPropertyChanged(nameof(SelectedCount));
                };
            }
        }

        // ================= FILTER =================
        private bool FilterApps(object obj)
        {
            if (obj is not AppItemViewModel app)
                return false;

            if (string.IsNullOrWhiteSpace(SearchText))
                return true;

            return app.Name.Contains(SearchText, System.StringComparison.OrdinalIgnoreCase);
        }

        partial void OnSearchTextChanged(string value)
        {
            GroupedApps.Refresh();
        }

        // ================= COMMANDS =================

        [RelayCommand]
        private void ToggleApp(AppItemViewModel app)
        {
            if (app == null) return;

            app.IsSelected = !app.IsSelected;
        }

        [RelayCommand]
        private void ApplyPreset(string preset)
        {
            if (string.IsNullOrWhiteSpace(preset))
                return;

            ActivePreset = preset;

            var ids = _presetService.GetApps(preset);

            foreach (var app in Apps)
                app.IsSelected = ids.Contains(app.NiniteId);

            OnPropertyChanged(nameof(SelectedCount));
        }

        [RelayCommand]
        private void Install()
        {
            _installService.Install(Apps.Select(a => a.Model));
        }

        [RelayCommand]
        private void ClearSelection()
        {
            foreach (var app in Apps)
                app.IsSelected = false;

            ActivePreset = null;

            OnPropertyChanged(nameof(SelectedCount));
        }

        // ================= DATA SOURCE =================

        private List<AppItem> LoadApps()
        {
            return new()
            {
                // BROWSERS
                new AppItem { Name = "Chrome", NiniteId = "chrome", Category = "Browsers", IconPath = "/Assets/Icons/chrome.png" },
                new AppItem { Name = "Firefox", NiniteId = "firefox", Category = "Browsers", IconPath = "/Assets/Icons/firefox.png" },
                new AppItem { Name = "Opera", NiniteId = "opera", Category = "Browsers", IconPath = "/Assets/Icons/opera.png" },

                // COMMUNICATION
                new AppItem { Name = "Discord", NiniteId = "discord", Category = "Communication", IconPath = "/Assets/Icons/discord.png" },
                new AppItem { Name = "Thunderbird", NiniteId = "thunderbird", Category = "Communication", IconPath = "/Assets/Icons/thunderbird.png" },

                // GAMING
                new AppItem { Name = "Steam", NiniteId = "steam", Category = "Gaming", IconPath = "/Assets/Icons/steam.png" },
                new AppItem { Name = "Epic Games", NiniteId = "epicgames", Category = "Gaming", IconPath = "/Assets/Icons/epicgames.png" },

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
                new AppItem { Name = "OpenOffice", NiniteId = "openoffice", Category = "Office", IconPath = "/Assets/Icons/openoffice.png" },

                // DEV
                new AppItem { Name = "VS Code", NiniteId = "vscode", Category = "Development", IconPath = "/Assets/Icons/vscode.png" },
                new AppItem { Name = "Notepad++", NiniteId = "notepadplusplus", Category = "Development", IconPath = "/Assets/Icons/notepad.png" },
                new AppItem { Name = "PuTTY", NiniteId = "putty", Category = "Development", IconPath = "/Assets/Icons/putty.png" },
                new AppItem { Name = "WinSCP", NiniteId = "winscp", Category = "Development", IconPath = "/Assets/Icons/winscp.png" },

                // OTHER
                new AppItem { Name = "CDBurnerXP", NiniteId = "cdburnerxp", Category = "Other", IconPath = "/Assets/Icons/cdburnerxp.png" },
            };
        }
    }
}