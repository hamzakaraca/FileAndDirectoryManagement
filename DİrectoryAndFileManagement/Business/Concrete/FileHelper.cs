using DİrectoryAndFileManagement.Business.Abstract;
using DİrectoryAndFileManagement.Entites;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace DİrectoryAndFileManagement.Business.Concrete
{
    public class FileHelper:IFileHelper
    {
        

        public async Task<List<FileNode>> LoadChildren(string path)
        {
            var children = new List<FileNode>();

            try
            {
                var directoryInfo = new DirectoryInfo(path);

                // Sadece sistem olmayan klasörleri al
                var directories = directoryInfo.GetDirectories()
                    .Where(dir => (dir.Attributes & FileAttributes.System) == 0);

                foreach (var dir in directories)
                {
                    bool hasChildren;
                    try
                    {
                        hasChildren = Directory.EnumerateFileSystemEntries(dir.FullName).Any();
                    }
                    catch (UnauthorizedAccessException)
                    {
                        hasChildren = true; // Yetki hatası varsa, içinde dosya olabilir diye varsayalım.
                    }

                    children.Add(new FileNode
                    {
                        Name = dir.Name,
                        IsDirectory = true,
                        FilePath = dir.FullName,
                        HasChildren = hasChildren
                    });
                }

                foreach (var file in directoryInfo.GetFiles())
                {
                    children.Add(new FileNode
                    {
                        Name = file.Name,
                        IsDirectory = false,
                        Size = file.Length,
                        FilePath = file.FullName
                    });
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Erişim izni yok: {path}. Hata: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Bir hata oluştu: {path}. Hata: {ex.Message}");
            }

            return children;

        }

        public string CreateFile(string filePath)
        {
            // Dosyayı aç ve içeriği yaz
            using (FileStream fs = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.None))
            {
                

                fs.Close();
            }
            
            // Dosya güvenliğini ayarla
            SetFileSecurity(filePath);

            return "Dosya oluşturuldu: " + filePath;
        }
        public string WriteFile(string filePath, string content)
        {
            // Dosyayı sıfırdan oluşturacak ve içeriğini yazacak
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    // Yazılacak içeriği dosyaya ekleyin
                    writer.WriteLine(content + " " + DateTime.Now);
                    writer.AutoFlush = true;
                    return "Dosyaya İçerik Yazıldı: " + content;
                }
            }
        }

        private void SetFileSecurity(string filePath)
        {
            // Dosya güvenliğini ayarla
            FileSecurity fileSecurity = new FileSecurity();

            // Mevcut kullanıcıya tam kontrol izni ver
            string user = Environment.UserDomainName + "\\" + Environment.UserName;
            fileSecurity.AddAccessRule(new FileSystemAccessRule(user, FileSystemRights.FullControl, AccessControlType.Allow));

            // FileInfo kullanarak dosya güvenliğini uygula
            FileInfo fileInfo = new FileInfo(filePath);
            fileInfo.SetAccessControl(fileSecurity);
        }

        public string ReadFile(string filePath)
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                return reader.ReadToEnd();
            }
        }

        public string CopyFile(string sourceFilePath, string destFilePath)
        {
            if (!File.Exists(sourceFilePath)) 
            {
                throw new FileNotFoundException("Dosya Bulunamadı.");
            }
            File.Copy(sourceFilePath, destFilePath, true);
            return "Dosya kopyalandı: " + sourceFilePath + " -> " + destFilePath;
        }

        public string DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return "Dosya silindi: " + filePath;
            }
            else
            {
                throw new Exception("Silinecek dosya bulunamadı: " + filePath);
            }
        }

        public DateTime GetFileCreationTime(string filePath)
        {
            return File.GetCreationTime(filePath);
        }

        public string DeleteDirectory(string directoryPath)
        {
            Directory.Delete(directoryPath);
            return "Klasör silindi: " + directoryPath;
        }

        public List<FileNode> GetRootDrives()
        {
            List<FileNode> drives = new List<FileNode>();
            var localDrives = DriveInfo.GetDrives().Where(drive => drive.IsReady).ToList();

            foreach (var localDrive in localDrives)
            {
                if (Directory.Exists(localDrive.Name))
                {
                    drives.Add(new FileNode
                    {
                        Name = localDrive.Name,
                        IsDirectory = true,
                        FilePath = localDrive.Name,
                        HasChildren = Directory.EnumerateFileSystemEntries(localDrive.Name).Any()
                    });
                }
            }

            return drives;
        }
    }

}
