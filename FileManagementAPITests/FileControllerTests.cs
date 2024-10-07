// See https://aka.ms/new-console-template for more information
using DİrectoryAndFileManagement;
using FileManagementAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

public class FileControllerTests
{
    private readonly Mock<FileSearcher> _mockFileSearcher;
    private readonly Mock<FileHelper> _mockFileHelper;
    private readonly FileController _controller;

    public FileControllerTests()
    {
        _mockFileSearcher = new Mock<FileSearcher>();
        _mockFileHelper = new Mock<FileHelper>();
        _controller = new FileController
        {
            // Mock nesneleri ayarlayın (dependency injection)
            FileSearcher = _mockFileSearcher.Object,
            FileHelper = _mockFileHelper.Object
        };
    }

    [Fact]
    public void SearchFilesByFileName_ShouldReturnOk_WhenFilesFound()
    {
        // Arrange
        string fileName = "hello.txt";
        var expectedFiles = new List<string> { "hello.txt" };
        _mockFileSearcher.Setup(fs => fs.SearchByFileName(fileName)).Returns(expectedFiles);

        // Act
        var result = _controller.SearchFilesByFileName(fileName);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedFiles, okResult.Value);
    }

    [Fact]
    public void SearchFilesByFileName_ShouldReturnBadRequest_WhenNoFilesFound()
    {
        // Arrange
        string fileName = "C:\\Test\\hello.txt";
        _mockFileSearcher.Setup(fs => fs.SearchByFileName(fileName)).Returns((List<string>)null);

        // Act
        var result = _controller.SearchFilesByFileName(fileName);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public void GetFileCreationTime_ShouldReturnOk_WhenFileExists()
    {
        // Arrange
        string filePath = "C:\\Test\\saf.txt";
        var expectedTime = DateTime.Now;
        _mockFileHelper.Setup(fh => fh.GetFileCreationTime(filePath)).Returns(expectedTime);

        // Act
        var result = _controller.GetFileCreationTime(filePath);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedTime, okResult.Value);
    }

    [Fact]
    public void CopyFile_ShouldReturnOk_WhenCopySuccess()
    {
        // Arrange
        string sourcePath = "C:\\Test\\hello.txt";
        string destinationPath = "C:\\Hamza\\hello.txt";
        _mockFileHelper.Setup(fh => fh.CopyFile(sourcePath, destinationPath)).Returns("başarılı");

        // Act
        var result = _controller.CopyFile(sourcePath, destinationPath);

        // Assert
        var okResult = Assert.IsType<OkResult>(result);
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public void CopyFile_ShouldReturnBadRequest_WhenCopyFails()
    {
        // Arrange
        string sourcePath = "C:\\Test\\hello.txt";
        string destinationPath = "C:\\Hamza\\hello.txt";
        _mockFileHelper.Setup(fh => fh.CopyFile(sourcePath, destinationPath)).Returns(("başarısız"));

        // Act
        var result = _controller.CopyFile(sourcePath, destinationPath);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    // Diğer metotlar için benzer testler yazabilirsiniz...
}
    