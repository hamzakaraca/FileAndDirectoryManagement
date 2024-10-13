using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DİrectoryAndFileManagement
{
    public interface IFileSearcher
    {
        public IEnumerable<string> SearchByFileType(string fileType);
        public IEnumerable<string> SearchByFileName(string fileName);
        public IEnumerable<string> SearchByCreationDate(string creationDate);

    }
}
