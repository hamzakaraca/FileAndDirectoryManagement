using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DİrectoryAndFileManagement
{
    public interface IFileHelper
    {
        public Task<PaginatedResult<string>> GetAllFilesAsync(string startDirectory, int pageNumber, int pageSize);
        public string CreateAndWriteFile(string filePath, string content);
        public string WriteFile(string filePath, string content);
        public string ReadFile(string filePath);
        public string CopyFile(string sourceFilePath, string destFilePath);
        public string DeleteFile(string filePath);
        public DateTime GetFileCreationTime(string filePath);
        public string DeleteDirectory(string directoryPath);

    }
}
