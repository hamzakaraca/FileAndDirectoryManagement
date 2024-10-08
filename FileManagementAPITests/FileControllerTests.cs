﻿using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using FileManagementAPI.Controllers;
using DİrectoryAndFileManagement;

namespace FileManagementAPITests
{
    [TestFixture]
    public class FileControllerTests
    {
        private Mock<FileSearcher> _mockFileSearcher;
        private Mock<FileHelper> _mockFileHelper;
        private FileController _controller;

        [SetUp]
        public void Setup()
        {
            _mockFileSearcher = new Mock<FileSearcher>();
            _mockFileHelper = new Mock<FileHelper>();
            _controller = new FileController
            {
                FileSearcher = _mockFileSearcher.Object,
                FileHelper = _mockFileHelper.Object
            };
        }
        [Test]
        public void SearchFilesByFileName_ShouldReturnOk_WhenFilesFound()
        {
            // Arrange
            string fileName = "example.txt";
            var expectedFiles = new List<string> { "example.txt" };
            _mockFileSearcher.Setup(fs => fs.SearchByFileName(fileName)).Returns(expectedFiles);

            // Act
            var result = _controller.SearchFilesByFileName(fileName);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);  // null olup olmadığını test et
            Assert.That(okResult.Value, Is.EqualTo(expectedFiles));  // Eşitliği test et
        }


        //[Test]
        //public void SearchFilesByFileName_ShouldReturnOk_WhenFilesFound()
        //{
        //    // Arrange
        //    string fileName = "example.txt";
        //    var expectedFiles = new List<string> { "example.txt" };
        //    _mockFileSearcher.Setup(fs => fs.SearchByFileName(fileName)).Returns(expectedFiles);

        //    // Act
        //    var result = _controller.SearchFilesByFileName(fileName);

        //    // Assert
        //    var okResult = result as OkObjectResult;
        //    Assert.IsNotNull(okResult);
        //    Assert.AreEqual(expectedFiles, okResult.Value);
        //}

        [Test]
        public void SearchFilesByFileName_ShouldReturnBadRequest_WhenNoFilesFound()
        {
            // Arrange
            string fileName = "notfound.txt";
            _mockFileSearcher.Setup(fs => fs.SearchByFileName(fileName)).Returns((List<string>)null);

            // Act
            var result = _controller.SearchFilesByFileName(fileName);

            var badRequestResult = result as BadRequestResult;
            Assert.That(badRequestResult, Is.Not.Null); // BadRequestResult döndüğünü kontrol eder

            // Assert
            
        }

        [Test]
        public void GetFileCreationTime_ShouldReturnOk_WhenFileExists()
        {
            // Arrange
            string filePath = "path/to/file.txt";
            var expectedTime = DateTime.Now;
            _mockFileHelper.Setup(fh => fh.GetFileCreationTime(filePath)).Returns(expectedTime);

            // Act
            var result = _controller.GetFileCreationTime(filePath);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(okResult,Is.Not.Null);
            Assert.That(expectedTime, Is.EqualTo(okResult.Value));
        }

        [Test]
        public void CopyFile_ShouldReturnOk_WhenCopySuccess()
        {
            // Arrange
            string sourcePath = "source.txt";
            string destinationPath = "destination.txt";
            _mockFileHelper.Setup(fh => fh.CopyFile(sourcePath, destinationPath)).Returns("true");

            // Act
            var result = _controller.CopyFile(sourcePath, destinationPath);

            // Assert
            var okResult = result as OkResult;
            Assert.That(okResult, Is.Not.Null); // OkResult döndüğünü kontrol eder

        }

        [Test]
        public void CopyFile_ShouldReturnBadRequest_WhenCopyFails()
        {
            // Arrange
            string sourcePath = "source.txt";
            string destinationPath = "destination.txt";
            _mockFileHelper.Setup(fh => fh.CopyFile(sourcePath, destinationPath)).Returns(("başarısız"));

            // Act
            var result = _controller.CopyFile(sourcePath, destinationPath);

            // Assert
            var badRequestResult = result as BadRequestResult;
            Assert.That(badRequestResult, Is.Not.Null);  // BadRequestResult döndüğ

        }

        // Diğer metotlar için benzer testler yazabilirsiniz...
    }
}
