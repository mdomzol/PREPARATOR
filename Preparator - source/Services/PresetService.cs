using System.Collections.Generic;

namespace Preparator.Services
{
    public class PresetService
    {
        private readonly Dictionary<string, List<string>> _presets = new()
        {
            { "Dev", new List<string> { "vscode", "notepadplusplus", "putty", "winscp" } },
            { "Basic", new List<string> { "chrome", "7zip", "vlc" } },
            { "Gaming", new List<string> { "steam", "epicgames", "discord" } }
        };

        public IEnumerable<string> GetPresetNames()
        {
            return _presets.Keys;
        }

        public List<string> GetApps(string preset)
        {
            if (_presets.TryGetValue(preset, out var apps))
                return apps;

            return new List<string>();
        }
    }
}