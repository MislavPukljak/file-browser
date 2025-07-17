using AutoMapper;
using Moq;
using FileBrowser.Business.Services;
using FileBrowser.Data.Repositories;
using FileBrowser.Business.DTOs;
using FileBrowser.Data.Entities;

namespace FileBrowser.Business.Tests
{
    public class FileServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly FileService _fileService;

        public FileServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _fileService = new FileService(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task AddFileAsync_WhenFileNameIsUnique_ShouldAddFile()
        {
            // Arrange
            var newFileDto = new FileDto { Name = "test", FolderId = Guid.NewGuid() };
            var fileEntity = new FileEntity { Id = Guid.NewGuid(), Name = newFileDto.Name };

            _mockUnitOfWork.Setup(x => x.Files.FileExistsAsync(newFileDto.FolderId, newFileDto.Name))
                          .ReturnsAsync(false);

            _mockMapper.Setup(m => m.Map<FileEntity>(newFileDto)).Returns(fileEntity);
            _mockMapper.Setup(m => m.Map<FileDto>(fileEntity)).Returns(newFileDto);

            // Act
            var result = await _fileService.AddFileAsync(newFileDto);

            // Assert
            _mockUnitOfWork.Verify(x => x.Files.AddAsync(It.Is<FileEntity>(f => f.Name == newFileDto.Name)), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(newFileDto.Name, result.Name);
        }

        [Fact]
        public async Task GetByFileIdAsync_WhenFileExists_ShouldReturnFileDto()
        {
            // Arrange
            var fileId = Guid.NewGuid();
            var fileEntity = new FileEntity { Id = fileId, Name = "test" };
            var fileDto = new FileDto { Id = fileId, Name = "test" };

            _mockUnitOfWork.Setup(x => x.Files.GetByIdAsync(fileId)).ReturnsAsync(fileEntity);
            _mockMapper.Setup(m => m.Map<FileDto>(fileEntity)).Returns(fileDto);

            // Act
            var result = await _fileService.GetByFileIdAsync(fileId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(fileId, result.Id);
            Assert.Equal(fileDto.Name, result.Name);
        }

        [Fact]
        public async Task GetAllFilesAsync_WhenFilesExist_ShouldReturnAllFiles()
        {
            // Arrange
            var files = new List<FileEntity>
            {
                new FileEntity { Id = Guid.NewGuid(), Name = "file1" },
                new FileEntity { Id = Guid.NewGuid(), Name = "file2" }
            };
            var fileDtos = files.Select(f => new FileDto { Id = f.Id, Name = f.Name }).ToList();

            _mockUnitOfWork.Setup(x => x.Files.GetAllAsync()).ReturnsAsync(files);
            _mockMapper.Setup(m => m.Map<IEnumerable<FileDto>>(files)).Returns(fileDtos);

            // Act
            var result = await _fileService.GetAllFilesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal(files[0].Name, result.First().Name);
            Assert.Equal(files[1].Name, result.Last().Name);
        }

        [Fact]
        public async Task GetByFolderIdAsync_WhenFolderExists_ShouldReturnFiles()
        {
            // Arrange
            var folderId = Guid.NewGuid();
            var files = new List<FileEntity>
            {
                new FileEntity { Id = Guid.NewGuid(), Name = "file1", FolderId = folderId },
                new FileEntity { Id = Guid.NewGuid(), Name = "file2", FolderId = folderId }
            };
            var fileDtos = files.Select(f => new FileDto { Id = f.Id, Name = f.Name, FolderId = f.FolderId }).ToList();

            _mockUnitOfWork.Setup(x => x.Folder.GetByIdAsync(folderId)).ReturnsAsync(new Folder { Id = folderId });
            _mockUnitOfWork.Setup(x => x.Files.GetByFolderIdAsync(folderId)).ReturnsAsync(files);
            _mockMapper.Setup(m => m.Map<IEnumerable<FileDto>>(files)).Returns(fileDtos);

            // Act
            var result = await _fileService.GetByFolderIdAsync(folderId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, file => Assert.Equal(folderId, file.FolderId));
        }

        [Fact]
        public async Task SearchFilesAsync_ShouldReturnMatchingFiles()
        {
            // Arrange
            var searchTerm = "test";
            var files = new List<FileEntity>
            {
                new FileEntity { Id = Guid.NewGuid(), Name = "test1" },
                new FileEntity { Id = Guid.NewGuid(), Name = "test2" }
            };
            var fileDtos = files.Select(f => new FileDto { Id = f.Id, Name = f.Name }).ToList();

            _mockUnitOfWork.Setup(x => x.Files.SearchFilesAsync(searchTerm, 10)).ReturnsAsync(files);
            _mockMapper.Setup(m => m.Map<IEnumerable<FileDto>>(files)).Returns(fileDtos);

            // Act
            var result = await _fileService.SearchFilesAsync(searchTerm);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, file => Assert.Contains(searchTerm, file.Name));
        }

        [Fact]
        public async Task DeleteFileAsync_WhenFileExists_ShouldDeleteFile()
        {
            // Arrange
            var fileId = Guid.NewGuid();
            var fileEntity = new FileEntity { Id = fileId, Name = "test" };

            _mockUnitOfWork.Setup(x => x.Files.GetByIdAsync(fileId)).ReturnsAsync(fileEntity);

            // Act
            await _fileService.DeleteFileAsync(fileId);

            // Assert
            _mockUnitOfWork.Verify(x => x.Files.DeleteAsync(fileId), Times.Once);
        }

        [Fact]
        public async Task UpdateFileAsync_WhenFileExistsAndNameIsValid_ShouldUpdateFile()
        {
            // Arrange
            var fileDto = new FileDto { Id = Guid.NewGuid(), Name = "updated", FolderId = Guid.NewGuid() };
            var existingFile = new FileEntity { Id = fileDto.Id, Name = "original", FolderId = fileDto.FolderId };

            _mockUnitOfWork.Setup(x => x.Files.GetByIdAsync(fileDto.Id)).ReturnsAsync(existingFile);
            _mockUnitOfWork.Setup(x => x.Files.FileExistsAsync(fileDto.FolderId, fileDto.Name)).ReturnsAsync(false);
            _mockMapper.Setup(m => m.Map<FileDto>(It.IsAny<FileEntity>())).Returns(fileDto);

            // Act
            var result = await _fileService.UpdateFileAsync(fileDto);

            // Assert
            _mockUnitOfWork.Verify(x => x.Files.UpdateAsync(It.IsAny<FileEntity>()), Times.Once);
            Assert.Equal(fileDto.Name, result.Name);
        }
    }
}