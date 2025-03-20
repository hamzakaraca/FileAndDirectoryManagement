using DİrectoryAndFileManagement.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DİrectoryAndFileManagement.Business.Abstract
{
    public interface IFileHelper
    {
        public string CreateFile(string filePath);
        public string WriteFile(string filePath, string content);
        public string ReadFile(string filePath);
        public string CopyFile(string sourceFilePath, string destFilePath);
        public string DeleteFile(string filePath);
        public DateTime GetFileCreationTime(string filePath);
        public string DeleteDirectory(string directoryPath);
        public Task<List<FileNode>> LoadChildren(string path);
        public List<FileNode> GetRootDrives();

    }
}
