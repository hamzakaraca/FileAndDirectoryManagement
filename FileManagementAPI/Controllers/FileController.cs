using DİrectoryAndFileManagement.Business.Abstract;
using DİrectoryAndFileManagement.Entites;
using DİrectoryAndFileManagement.Models;
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
            return Ok(result);
        }

        [HttpGet("[action]")]
        public IActionResult SearchFilesByCreationDate(string creationDate)
        {
            var result = _fileSearcher.SearchByCreationDate(creationDate);
            if (result == null)
            {
                return BadRequest(result);

            }
            return Ok(result);
        }


        // Kök dizinleri alır
        [HttpGet("root")]
        public ActionResult<List<FileNode>> GetRootDrives()
        {
            
            var rootDrives = _fileHelper.GetRootDrives();
            return Ok(rootDrives);
        }

        [HttpGet("[action]")]
        public IActionResult LoadChildren(string path)
        {
            var result = _fileHelper.LoadChildren(path);
            if (result == null)
            {
                return BadRequest(result);

            }
            return Ok(result);
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
        public IActionResult CreateFile(string filePath)
        {
            var result = _fileHelper.CreateFile(filePath);
            if (result == null)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("[action]")]
        public IActionResult WriteFile(FileEditModel fileEditModel)
        {
            var result = _fileHelper.WriteFile(fileEditModel.FilePath,fileEditModel.Content);
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
