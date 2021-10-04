using System.Diagnostics;

namespace CommitViewer.Shared.Extensions
{
    public static class ProcessExtensions
    {
        public static ProcessStartInfo CreateProcessStartInfo(string fileName, string arguments)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(fileName, arguments);
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.CreateNoWindow = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.UseShellExecute = false;
            return startInfo;
        }
    }
}
