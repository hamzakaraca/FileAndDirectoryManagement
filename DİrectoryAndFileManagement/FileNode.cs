using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DİrectoryAndFileManagement
{
    public class FileNode
    {
        public string Name { get; set; }  // Dosya veya klasör adı
        public bool IsDirectory { get; set; }  // Klasör mü yoksa dosya mı
        public List<FileNode>? Children { get; set; } = new List<FileNode>(); // Alt dosya/klasörler
        public long? Size { get; set; }  // Dosya boyutu (varsa)
        public string FilePath { get; set; }
    }
}
