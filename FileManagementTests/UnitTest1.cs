
using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using FileManagementAPI.Controllers;
using DÝrectoryAndFileManagement.Business.Abstract;


namespace FileManagementTests
//{
//    public class Tests
//    {
//        [SetUp]
//        public void Setup()
//        {
//        }

//        [Test]
//        public void Test1()
//        {
//            Assert.Pass();
//        }
//    }
//}


//namespace FileManagementAPITests
{
    [TestFixture]
    public class FileControllerTests
    {
        private Mock<IFileSearcher> _mockFileSearcher;
        private Mock<IFileHelper> _mockFileHelper;
        private FileController _controller;

        [SetUp]
        public void Setup()
        {
            _mockFileSearcher = new Mock<IFileSearcher>();
            _mockFileHelper = new Mock<IFileHelper>();

            _controller = new FileController(_mockFileHelper.Object, _mockFileSearcher.Object);
        }
        [Test]
        public void SearchFilesByFileName_ShouldReturnOk_WhenFilesFound()
        {
            // Arrange
            string fileName = "hello.txt";
            var expectedFiles = new List<string> { "hello.txt" };
            _mockFileSearcher.Setup(fs => fs.SearchByFileName(fileName)).Returns(expectedFiles);

            // Act
            var result = _controller.SearchFilesByFileName(fileName);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);  // null olup olmadýðýný test et
            Assert.That(okResult.Value, Is.EqualTo(expectedFiles));  // Eþitliði test et
        }


        //[Test]
        //public void SearchFilesByFileName_ShouldReturnOk_WhenFilesFound()
        //{
        //    // Arrange
        //    string fileName = "hello.txt";
        //    var expectedFiles = new List<string> { "hello.txt" };
        //    _mockFileSearcher.Setup(fs => fs.SearchByFileName(fileName)).Returns(expectedFiles);

        //    // Act
        //    var result = _controller.SearchFilesByFileName(fileName);

        //    // Assert
        //    var okResult = result as OkObjectResult;
        //    Assert.That(okResult, Is.Not.Null); // OkObjectResult'ýn null olmadýðýný kontrol et
        //    Assert.That(okResult.Value, Is.EqualTo(expectedFiles)); // OkResult'ýn içindeki deðerin expectedFiles'a eþit olduðunu kontrol
        //}

        [Test]
        public void SearchFilesByFileName_ShouldReturnBadRequest_WhenNoFilesFound()
        {
            // Arrange
            string fileName = "hello.txt";
            _mockFileSearcher.Setup(fs => fs.SearchByFileName(fileName)).Returns(new List<string>());  // Boþ liste döner

            // Act
            var result = _controller.SearchFilesByFileName(fileName);

            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult, Is.Not.Null); // BadRequestResult döndüðünü kontrol eder
            Assert.That(badRequestResult.Value, Is.EqualTo("Dosya bulunamadý."));
            // Assert

        }

        [Test]
        public void GetFileCreationTime_ShouldReturnOk_WhenFileExists()
        {
            // Arrange
            string filePath = "C://Test/hello.txt";
            var expectedTime = DateTime.Now;
            _mockFileHelper.Setup(fh => fh.GetFileCreationTime(filePath)).Returns(expectedTime);

            // Act
            var result = _controller.GetFileCreationTime(filePath);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(expectedTime, Is.EqualTo(okResult.Value));
        }

        [Test]
        public void CopyFile_ShouldReturnOk_WhenCopySuccess()
        {
            // Arrange
            string sourcePath = "C:\\Test\\hello.txt";
            string destinationPath = "C:\\hamza\\hello.txt";
            _mockFileHelper.Setup(fh => fh.CopyFile(sourcePath, destinationPath))
               .Returns("Dosya kopyalandý: " + sourcePath + " -> " + destinationPath);


            // Act
            var result = _controller.CopyFile(sourcePath, destinationPath);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null); // OkResult döndüðünü kontrol eder
            Assert.That(okResult.Value, Is.EqualTo("Dosya kopyalandý: " + sourcePath + " -> " + destinationPath));
        }

        [Test]
        public void CopyFile_ShouldReturnBadRequest_WhenCopyFails()
        {
            // Arrange
            string sourcePath = "C:\\Test\\hello.txt";
            string destinationPath = "C:\\hamza\\hello.txt";
            _mockFileHelper.Setup(fh => fh.CopyFile(sourcePath, destinationPath)).Returns(("Dosya bulunamadý."));

            // Act
            var result = _controller.CopyFile(sourcePath, destinationPath);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult, Is.Not.Null);  // BadRequestResult döndüð
            Assert.That(badRequestResult.Value, Is.EqualTo("Dosya bulunamadý."));
        }

        // Diðer metotlar için benzer testler yazabilirsiniz...
    }
}
