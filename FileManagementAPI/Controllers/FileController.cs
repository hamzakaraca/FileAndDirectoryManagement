using DİrectoryAndFileManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;


namespace FileManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileHelper _fileHelper;
        private readonly IFileSearcher _fileSearcher;

        public FileController(IFileHelper fileHelper, IFileSearcher fileSearcher)
        {
            _fileHelper = fileHelper;
            _fileSearcher = fileSearcher;
        }


        [HttpGet("[action]")]
        public IActionResult SearchFilesByFileName(string fileName)
        {
            var result = _fileSearcher.SearchByFileName(fileName);
            
            if (result==null || !result.Any())
            {
                return BadRequest("Dosya bulunamadı.");
            }
            
            return Ok(result);
        }

        [HttpGet("[action]")]
        public IActionResult SearchFilesByFileType(string fileType)
        {
            var result = _fileSearcher.SearchByFileType(fileType);
            if (result ==null)
            {
                return BadRequest(result);

            }
            return BadRequest(result);
        }

        [HttpGet("[action]")]
        public IActionResult SearchFilesByCreationDate(string dateTime)
        {
            var result = _fileSearcher.SearchByCreationDate(dateTime);
            if (result == null)
            {
                return BadRequest(result);

            }
            return Ok(result);
        }

        

        [HttpGet("[action]")]
        [ProducesResponseType(typeof(List<FileNode>), StatusCodes.Status200OK)] // Başarılı yanıt tipi
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Hata yanıt tipi
        public async Task<IActionResult> GetFileTreeAsync()
        {
            var fileTreeGenerator = new FileTreeGenerator();

            // Dosya ağacını async olarak oluştur
            var fileTree = await Task.Run(() => fileTreeGenerator.BuildFileTreeForDrives());

            // Dosya ağacı boşsa hata döndür
            if (fileTree == null || fileTree.Count == 0)
            {
                return NotFound("Dosya ağacı bulunamadı veya erişim izni yok.");
            }

            // JSON olarak serileştir
            var jsonResult = JsonSerializer.Serialize(fileTree);

            // Chunking yerine Content-Length başlığını manuel olarak ayarlıyoruz
            var byteArray = Encoding.UTF8.GetBytes(jsonResult);
            Response.Headers.ContentLength = byteArray.Length;

            // Tüm veriyi toplu olarak geri döndür
            return new FileContentResult(byteArray, "application/json");
        }


        [HttpGet("[action]")]
        public IActionResult GetFileCreationTime(string filePath)
        {
            var result = _fileHelper.GetFileCreationTime(filePath);
            return Ok(result);
        }

        [HttpGet("[action]")]
        public IActionResult ReadFile(string filePath)
        {
            var result = _fileHelper.ReadFile(filePath);
            if (result=="Dosya bulunamadı.")
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("[action]")]
        public IActionResult CopyFile(string sourceFilePath,string destinationFilePath)
        {
            var result = _fileHelper.CopyFile(sourceFilePath,destinationFilePath);
            if (result == null || result.Contains("Dosya bulunamadı.")) 
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("[action]")]
        public IActionResult CreateFile(string filePath,string content)
        {
            var result = _fileHelper.CreateAndWriteFile(filePath,content);
            if (result == null)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("[action]")]
        public IActionResult WriteFile(string filePath,string content)
        {
            var result = _fileHelper.WriteFile(filePath,content);
            if (result == null)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpDelete("[action]")]
        public IActionResult DeleteFile(string filePath)
        {
            var result = _fileHelper.DeleteFile(filePath);
            if (result == null)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpDelete("[action]")]
        public IActionResult DeleteDirectory(string directoryPath)
        {
            var result = _fileHelper.DeleteDirectory(directoryPath);
            if (result == null)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
