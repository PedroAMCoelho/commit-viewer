using System;
using System.IO;

namespace CommitViewer.Shared.Extensions
{
    public static class StreamReaderExtensions
    {
        public static string ReadFileJsonString(string fileName)
        {
            var lines = String.Concat(File.ReadAllLines(fileName));
            return lines;
        }
    }
}
