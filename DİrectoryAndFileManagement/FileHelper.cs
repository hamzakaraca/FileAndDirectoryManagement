﻿using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace DİrectoryAndFileManagement
{
    public class FileHelper:IFileHelper
    {
        

        public async Task<List<FileNode>> GetAllFilesAsync()
        {
            FileTreeGenerator generator = new FileTreeGenerator();
            var result = generator.BuildFileTreeForDrives();
            return result;
        }

        private async Task<List<string>> ListFilesAndDirectoriesAsync(string directory, int indentLevel)
        {
            var result = new List<string>();

            try
            {
                string[] files = Directory.GetFiles(directory);
                foreach (string file in files)
                {
                    try
                    {
                        result.Add($"{new string(' ', indentLevel * 2)}- {Path.GetFileName(file)}");
                    }
                    catch (UnauthorizedAccessException)
                    {
                        result.Add($"{new string(' ', indentLevel * 2)}- [Erişim hatası]: {Path.GetFileName(file)}");
                    }
                }

                string[] directories = Directory.GetDirectories(directory);
                foreach (string subDir in directories)
                {
                    try
                    {
                        result.Add($"{new string(' ', indentLevel * 2)}+ {Path.GetFileName(subDir)}");
                        result.AddRange(await ListFilesAndDirectoriesAsync(subDir, indentLevel + 1));
                    }
                    catch (UnauthorizedAccessException)
                    {
                        result.Add($"{new string(' ', indentLevel * 2)}+ [Erişim hatası]: {Path.GetFileName(subDir)}");
                    }
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                result.Add($"Ana dizinde erişim hatası: {directory} - {ex.Message}");
            }
            catch (Exception ex)
            {
                result.Add($"Bir hata oluştu: {ex.Message}");
            }

            return result;
        }

        public string CreateAndWriteFile(string filePath, string content)
        {
            // Dosyayı aç ve içeriği yaz
            using (FileStream fs = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.None))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    // Eğer dosya boş değilse, bir satır sonu ekle
                    if (fs.Length > 0)
                    {
                        writer.WriteLine();
                        writer.AutoFlush=true;
                    }
                    writer.WriteLine(content + " " + DateTime.Now);
                    
                }

                fs.Close();
            }
            
            // Dosya güvenliğini ayarla
            SetFileSecurity(filePath);

            return "Dosya oluşturuldu ve yazıldı: " + filePath;
        }
        public string WriteFile(string filePath, string content)
        {
            using (FileStream fs = new FileStream(filePath,FileMode.Append,FileAccess.Write,FileShare.Write))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    if (fs.Length>0)
                    {
                        writer.WriteLine();
                        
                    }
                    writer.WriteLine(content);
                    writer.AutoFlush = true;
                    return "Dosyaya İçerik Yazıldı : " + content;
                }
                fs.Close();
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
    }

}
