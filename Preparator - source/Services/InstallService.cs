using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Preparator.Models;

namespace Preparator.Services
{
    public class InstallService
    {
        public void Install(IEnumerable<AppItem> apps)
        {
            var selected = apps.Where(a => a.IsSelected).ToList();

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