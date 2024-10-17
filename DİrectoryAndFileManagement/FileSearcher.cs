using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DİrectoryAndFileManagement
{
    public class FileSearcher:IFileSearcher
    {
        // Tüm sürücülerde dosya aramak için bir yardımcı metod
        private IEnumerable<string> SearchFiles(Func<string, bool> predicate)
        {
            var result = new List<string>();
            var drives = DriveInfo.GetDrives().Where(drive => drive.IsReady).ToList();

            Parallel.ForEach(drives, drive =>
            {
                try
                {
                    var files = SearchDirectory(drive.Name, predicate);
                    lock (result)
                    {
                        result.AddRange(files);
                    }
                }
                catch (UnauthorizedAccessException) { /* Bazı dizinlere erişim izni olmayabilir */ }
                catch (PathTooLongException) { /* Bazı yollar çok uzun olabilir */ }
            });
            //if (result.Count<1)
            //{
            //    var a = "Dosya buluanamadı.";
            //    result.Add(a);
            //    return result;
            //}
            return result;
        }

        // Belirli bir dizinde dosya aramak için bir yardımcı metod
        private IEnumerable<string> SearchDirectory(string directoryPath, Func<string, bool> predicate)
        {
            var result = new List<string>();
            try
            {
                var files = Directory.EnumerateFiles(directoryPath, "*", SearchOption.TopDirectoryOnly)
                                     .Where(file => predicate(file));

                lock (result)
                {
                    result.AddRange(files);
                }

                var directories = Directory.EnumerateDirectories(directoryPath).ToList();
                Parallel.ForEach(directories, directory =>
                {
                    try
                    {
                        var subDirFiles = SearchDirectory(directory, predicate);
                        lock (result)
                        {
                            result.AddRange(subDirFiles);
                        }
                    }
                    catch (UnauthorizedAccessException) { /* Alt dizine erişim izni olmayabilir */ }
                    catch (PathTooLongException) { /* Alt dizin yolları çok uzun olabilir */ }
                });
            }
            catch (UnauthorizedAccessException) { /* Üst dizine erişim izni olmayabilir */ }
            catch (PathTooLongException) { /* Üst dizin yolu çok uzun olabilir */ }

            if (result.Count > 0)
            {
                var a = result;
           }

            return result;
        }

        // Dosya türüne göre arama
        public IEnumerable<string> SearchByFileType(string fileType)
        {
            return SearchFiles(file => Path.GetExtension(file).Equals($".{fileType}", StringComparison.OrdinalIgnoreCase));
        }

        // Dosya adına göre arama
        public IEnumerable<string> SearchByFileName(string fileName)
        {
            return SearchFiles(file => Path.GetFileName(file).Contains(fileName, StringComparison.OrdinalIgnoreCase));
        }

        // Dosya oluşturulma tarihine göre arama
        public IEnumerable<string> SearchByCreationDate(string creationDate)
        {
            var splittedCreationDate=creationDate.Split(" ");
            var dateTime=DateTimeConverter.ConvertToDateTime(splittedCreationDate[0], splittedCreationDate[1]);
            return SearchFiles(file =>
            {
                try
                {
                    var fileInfo = new FileInfo(file);
                    return fileInfo.CreationTime.Hour == dateTime.Hour && fileInfo.CreationTime.Second==dateTime.Second && fileInfo.CreationTime.Minute==dateTime.Minute && fileInfo.CreationTime.Date == dateTime.Date;
                }
                catch (FileNotFoundException) { return false; }
            });
        }
    }
}
