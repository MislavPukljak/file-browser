using AutoMapper;
using Moq;
using FileBrowser.Business.Services;
using FileBrowser.Data.Repositories;
using FileBrowser.Business.DTOs;
using FileBrowser.Business.Exceptions;
using FileBrowser.Data.Entities;

namespace FileBrowser.Business.Tests
{
    public class FolderServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly FolderService _folderService;

        public FolderServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _folderService = new FolderService(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task AddFolderAsync_WhenFolderNameIsUnique_ShouldAddFolder()
        {
            // Arrange
            var newFolderDto = new FolderDto { Name = "New Folder", ParentFolderId = null };
            var folderEntity = new Folder { Id = Guid.NewGuid(), Name = newFolderDto.Name };

            _mockUnitOfWork.Setup(x => x.Folder.ParentFolderExistsAsync(newFolderDto.ParentFolderId))
                          .ReturnsAsync(true);
            _mockUnitOfWork.Setup(x => x.Folder.FolderExistsAsync(newFolderDto.ParentFolderId, newFolderDto.Name))
                           .ReturnsAsync(false);

            _mockMapper.Setup(m => m.Map<Folder>(newFolderDto)).Returns(folderEntity);
            _mockMapper.Setup(m => m.Map<FolderDto>(folderEntity)).Returns(newFolderDto);

            // Act
            var result = await _folderService.AddFolderAsync(newFolderDto);

            // Assert
            _mockUnitOfWork.Verify(x => x.Folder.AddAsync(It.Is<Folder>(f => f.Name == newFolderDto.Name)), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(newFolderDto.Name, result.Name);
        }

        [Fact]
        public async Task UpdateFolderAsync_WhenFolderExistsAndNameIsValid_ShouldUpdateFolder()
        {
            // Arrange
            var folderDto = new FolderDto { Id = Guid.NewGuid(), Name = "Updated Folder" };
            var existingFolder = new Folder { Id = folderDto.Id, Name = "My Folder" };

            _mockUnitOfWork.Setup(x => x.Folder.GetByIdAsync(folderDto.Id)).ReturnsAsync(existingFolder);
            _mockUnitOfWork.Setup(x => x.Folder.FolderExistsAsync(It.IsAny<Guid?>(), folderDto.Name)).ReturnsAsync(false);
            _mockMapper.Setup(m => m.Map<FolderDto>(It.IsAny<Folder>())).Returns(folderDto);

            // Act
            var result = await _folderService.UpdateFolderAsync(folderDto);

            // Assert
            _mockUnitOfWork.Verify(x => x.Folder.UpdateAsync(It.IsAny<Folder>()), Times.Once);
            Assert.Equal(folderDto.Name, result.Name);
        }

        [Fact]
        public async Task DeleteAsync_WhenFolderExists_ShouldDeleteFolderAndSubEntities()
        {
            // Arrange
            var folderId = Guid.NewGuid();
            var subFolderId = Guid.NewGuid();
            var fileId = Guid.NewGuid();

            var folder = new Folder { Id = folderId, Name = "Root" };
            var subFolders = new List<Folder> { new Folder { Id = subFolderId, Name = "Sub" } };
            var files = new List<FileEntity> { new FileEntity { Id = fileId, Name = "file" } };

            _mockUnitOfWork.Setup(x => x.Folder.GetByIdAsync(folderId)).ReturnsAsync(folder);
            _mockUnitOfWork.Setup(x => x.Folder.GetSubFoldersAsync(folderId)).ReturnsAsync(subFolders);
            _mockUnitOfWork.Setup(x => x.Folder.GetSubFoldersAsync(subFolderId)).ReturnsAsync(new List<Folder>());
            _mockUnitOfWork.Setup(x => x.Files.GetByFolderIdAsync(folderId)).ReturnsAsync(new List<FileEntity>());
            _mockUnitOfWork.Setup(x => x.Files.GetByFolderIdAsync(subFolderId)).ReturnsAsync(files);

            // Act
            await _folderService.DeleteAsync(folderId);

            // Assert
            _mockUnitOfWork.Verify(x => x.Files.DeleteAsync(fileId), Times.Once);
            _mockUnitOfWork.Verify(x => x.Folder.DeleteAsync(subFolderId), Times.Once);
            _mockUnitOfWork.Verify(x => x.Folder.DeleteAsync(folderId), Times.Once);
        }

        [Fact]
        public async Task GetByFolderIdAsync_WhenFolderExists_ShouldReturnFolderDto()
        {
            // Arrange
            var folderId = Guid.NewGuid();
            var folder = new Folder { Id = folderId, Name = "Test Folder" };
            var folderDto = new FolderDto { Id = folderId, Name = "Test Folder" };

            _mockUnitOfWork.Setup(x => x.Folder.GetByIdAsync(folderId)).ReturnsAsync(folder);
            _mockMapper.Setup(m => m.Map<FolderDto?>(folder)).Returns(folderDto);

            // Act
            var result = await _folderService.GetByFolderIdAsync(folderId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(folderId, result.Id);
        }
    }
}