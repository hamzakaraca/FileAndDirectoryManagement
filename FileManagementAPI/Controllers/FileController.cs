using DİrectoryAndFileManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
 

namespace FileManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        FileSearcher fileSearcher = new FileSearcher();
        FileHelper fileHelper = new FileHelper();
        public FileHelper FileHelper;

        public FileSearcher FileSearcher { get; set; }

        [HttpGet("[action]")]
        public IActionResult SearchFilesByFileName(string fileName)
        {
            var result = fileSearcher.SearchByFileName(fileName);
            
            if (result==null)
            {
                return BadRequest(result);
            }
            
            return Ok(result);
        }

        [HttpGet("[action]")]
        public IActionResult SearchFilesByFileType(string fileType)
        {
            var result = fileSearcher.SearchByFileType(fileType);
            if (result ==null)
            {
                return BadRequest(result);

            }
            return BadRequest(result);
        }

        [HttpGet("[action]")]
        public IActionResult SearchFilesByCreationDate(string dateTime)
        {
            var result = fileSearcher.SearchByCreationDate(dateTime);
            if (result == null)
            {
                return BadRequest(result);

            }
            return Ok(result);
        }

        [HttpGet("[action]")]
        public IActionResult GetAllFilesAsync(string startDirectory,int pageNumber,int pageSize) 
        {
            var result = fileHelper.GetAllFilesAsync(startDirectory,pageNumber,pageSize);
            if (result.IsCompletedSuccessfully)
            {
                return Ok(result);
            }
            return BadRequest(result.Exception);
        }

        [HttpGet("[action]")]
        public IActionResult GetFileCreationTime(string filePath)
        {
            var result = fileHelper.GetFileCreationTime(filePath);
            return Ok(result);
        }

        [HttpGet("[action]")]
        public IActionResult ReadFile(string filePath)
        {
            var result = fileHelper.ReadFile(filePath);
            if (result==null)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("[action]")]
        public IActionResult CopyFile(string sourceFilePath,string destinationFilePath)
        {
            var result = fileHelper.CopyFile(sourceFilePath,destinationFilePath);
            if (result == null) 
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("[action]")]
        public IActionResult CreateFile(string filePath,string content)
        {
            var result = fileHelper.CreateAndWriteFile(filePath,content);
            if (result == null)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("[action]")]
        public IActionResult WriteFile(string filePath,string content)
        {
            var result = fileHelper.WriteFile(filePath,content);
            if (result == null)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpDelete("[action]")]
        public IActionResult DeleteFile(string filePath)
        {
            var result = fileHelper.DeleteFile(filePath);
            if (result == null)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpDelete("[action]")]
        public IActionResult DeleteDirectory(string directoryPath)
        {
            var result = fileHelper.DeleteDirectory(directoryPath);
            if (result == null)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
