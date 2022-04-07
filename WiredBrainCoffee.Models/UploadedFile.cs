using System;
using System.Collections.Generic;
using System.Text;

namespace WiredBrainCoffee.Shared
{
    public class UploadedFile
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] FileContent { get; set; }
    }
}
