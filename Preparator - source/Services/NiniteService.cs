namespace Preparator.Services
{
    using System;
    using System.Diagnostics;

    public class NiniteService
    {
        public event Action<string> OutputReceived;

        public void StartInstall(string exePath)
        {
            var process = new Process();

            process.StartInfo.FileName = exePath;
            process.StartInfo.Arguments = "/silent";

            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;

            process.OutputDataReceived += (s, e) =>
            {
                if (!string.IsNullOrWhiteSpace(e.Data))
                {
                    OutputReceived?.Invoke(e.Data);
                }
            };

            process.Start();
            process.BeginOutputReadLine();
        }
    }
}