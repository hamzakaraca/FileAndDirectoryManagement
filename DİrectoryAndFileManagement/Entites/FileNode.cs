using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DİrectoryAndFileManagement.Entites
{
    public class FileNode
    {
        public string Name { get; set; }
        public bool IsDirectory { get; set; }
        public long? Size { get; set; }
        public string FilePath { get; set; }
        public bool HasChildren { get; set; } // Lazy loading için çocukları var mı bilgisi
        public List<FileNode> Children { get; set; }


    }
}
