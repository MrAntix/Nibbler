using System;

namespace Antix.Nibbler
{
    public class NibblerCompressOptions
    {
        public string CompressedFilesPattern { get; set; }
        public string CompressedFile { get; set; }
        public bool DeleteOriginal { get; set; }

        public Func<string, int, bool> Progress { get; set; }
    }
}