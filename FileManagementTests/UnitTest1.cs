
using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using FileManagementAPI.Controllers;
using D�rectoryAndFileManagement.Business.Abstract;


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
            Assert.That(okResult, Is.Not.Null);  // null olup olmad���n� test et
            Assert.That(okResult.Value, Is.EqualTo(expectedFiles));  // E�itli�i test et
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
        //    Assert.That(okResult, Is.Not.Null); // OkObjectResult'�n null olmad���n� kontrol et
        //    Assert.That(okResult.Value, Is.EqualTo(expectedFiles)); // OkResult'�n i�indeki de�erin expectedFiles'a e�it oldu�unu kontrol
        //}

        [Test]
        public void SearchFilesByFileName_ShouldReturnBadRequest_WhenNoFilesFound()
        {
            // Arrange
            string fileName = "hello.txt";
            _mockFileSearcher.Setup(fs => fs.SearchByFileName(fileName)).Returns(new List<string>());  // Bo� liste d�ner

            // Act
            var result = _controller.SearchFilesByFileName(fileName);

            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult, Is.Not.Null); // BadRequestResult d�nd���n� kontrol eder
            Assert.That(badRequestResult.Value, Is.EqualTo("Dosya bulunamad�."));
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
               .Returns("Dosya kopyaland�: " + sourcePath + " -> " + destinationPath);


            // Act
            var result = _controller.CopyFile(sourcePath, destinationPath);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null); // OkResult d�nd���n� kontrol eder
            Assert.That(okResult.Value, Is.EqualTo("Dosya kopyaland�: " + sourcePath + " -> " + destinationPath));
        }

        [Test]
        public void CopyFile_ShouldReturnBadRequest_WhenCopyFails()
        {
            // Arrange
            string sourcePath = "C:\\Test\\hello.txt";
            string destinationPath = "C:\\hamza\\hello.txt";
            _mockFileHelper.Setup(fh => fh.CopyFile(sourcePath, destinationPath)).Returns(("Dosya bulunamad�."));

            // Act
            var result = _controller.CopyFile(sourcePath, destinationPath);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult, Is.Not.Null);  // BadRequestResult d�nd��
            Assert.That(badRequestResult.Value, Is.EqualTo("Dosya bulunamad�."));
        }

        // Di�er metotlar i�in benzer testler yazabilirsiniz...
    }
}
