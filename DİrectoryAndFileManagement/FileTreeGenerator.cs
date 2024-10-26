using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DİrectoryAndFileManagement
{
    public class FileTreeGenerator
    {
        // C: ve D: sürücülerindeki dosyaları tarar
        public List<FileNode> BuildFileTreeForDrives()
        {
            List<FileNode> drives = new List<FileNode>();
            List<string> driveLetters = new List<string>();
            var localDrives = DriveInfo.GetDrives().Where(drive => drive.IsReady).ToList();

            foreach (var localDrive in localDrives)
            {
                driveLetters.Add(localDrive.Name);
            }

            // Parallel işlemleri başlat
            Parallel.ForEach(driveLetters, drive =>
            {
                try
                {
                    if (Directory.Exists(drive))
                    {
                        var fileTree = BuildFileTree(drive);
                        lock (drives) // Thread-safe erişim
                        {
                            drives.Add(fileTree);
                        }
                    }
                }
                catch (UnauthorizedAccessException ex)
                {
                    Console.WriteLine($"Erişim izni yok: {drive}. Hata: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Bir hata oluştu: {drive}. Hata: {ex.Message}");
                }
            });

            return drives;
        }

        // Belirtilen dizinden başlayarak ağaç yapısını oluşturur
        private FileNode BuildFileTree(string path)
        {
            try
            {
                if (Directory.Exists(path))
                {
                    var directoryInfo = new DirectoryInfo(path);
                    var directoryNode = new FileNode
                    {
                        Name = directoryInfo.Name,
                        IsDirectory = true,
                        FilePath = directoryInfo.FullName, // Dizin yolunu atayın
                        Children = new List<FileNode>()
                    };

                    Parallel.ForEach(directoryInfo.GetDirectories(), dir =>
                    {
                        try
                        {
                            var childNode = BuildFileTree(dir.FullName);
                            if (childNode != null) // Null olmayan düğümleri ekleyin
                            {
                                lock (directoryNode.Children)
                                {
                                    directoryNode.Children.Add(childNode);
                                }
                            }
                        }
                        catch (UnauthorizedAccessException ex)
                        {
                            Console.WriteLine($"Erişim izni yok: {dir.FullName}. Hata: {ex.Message}");
                        }
                    });

                    foreach (var file in directoryInfo.GetFiles())
                    {
                        directoryNode.Children.Add(new FileNode
                        {
                            Name = file.Name,
                            IsDirectory = false,
                            Size = file.Length,
                            FilePath = file.FullName // Dosya yolunu atayın
                        });
                    }

                    return directoryNode.Children.Count > 0 ? directoryNode : null; // Çocuk düğümü yoksa null döndür
                }
                else if (File.Exists(path))
                {
                    var fileInfo = new FileInfo(path);
                    return new FileNode
                    {
                        Name = fileInfo.Name,
                        IsDirectory = false,
                        Size = fileInfo.Length,
                        FilePath = fileInfo.FullName // Dosya yolunu atayın
                    };
                }
                else
                {
                    throw new FileNotFoundException("Dizin veya dosya bulunamadı.");
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Erişim izni yok: {path}. Hata: {ex.Message}");
                return null;
            }
        }


    }
}
