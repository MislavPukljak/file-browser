using AutoMapper;
using FileBrowser.Business.DTOs;
using FileBrowser.Business.Exceptions;
using FileBrowser.Data.Entities;
using FileBrowser.Data.Repositories;

namespace FileBrowser.Business.Services
{
    public class FolderService : IFolderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FolderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<FolderDto> AddFolderAsync(FolderDto folderDto)
        {
            var parentFolder = await _unitOfWork.Folder.ParentFolderExistsAsync(folderDto.ParentFolderId);

            if (!parentFolder && folderDto.ParentFolderId != null)
            {
                throw new FolderException($"Parent folder with ID {folderDto.ParentFolderId} not found!", 404);
            }

            await NameCheckAsync(folderDto);

            var folder = _mapper.Map<Folder>(folderDto);
            await _unitOfWork.Folder.AddAsync(folder);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<FolderDto>(folder);
        }

        public async Task DeleteAsync(Guid id)
        {
            var folder = await _unitOfWork.Folder.GetByIdAsync(id);

            if (folder == null)
            {
                throw new FolderException($"Folder with Id {id} not found!", 404);
            }

            await DeleteFolderRecursivelyAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<FolderDto?> GetByFolderIdAsync(Guid id)
        {
            var folder = await _unitOfWork.Folder.GetByIdAsync(id);

            if (folder == null)
            {
                throw new FolderException($"Folder with Id {id} not found!", 404);
            }

            return _mapper.Map<FolderDto?>(folder);
        }

        public async Task<IEnumerable<FolderDto>> GetFoldersAsync()
        {
            var folders = await _unitOfWork.Folder.GetAllAsync();

            return _mapper.Map<IEnumerable<FolderDto>>(folders);
        }

        public async Task<IEnumerable<FolderDto>> GetSubFoldersAsync(Guid parentId)
        {
            var subFolders = await _unitOfWork.Folder.GetSubFoldersAsync(parentId);

            if (subFolders == null)
            {
                throw new FolderException($"Subfolders not found!", 404);
            }

            return _mapper.Map<IEnumerable<FolderDto>>(subFolders);
        }

        public async Task<FolderDto> UpdateFolderAsync(FolderDto folderDto)
        {
            if (folderDto.Id == folderDto.ParentFolderId)
            {
                throw new FolderException($"Folder an ParentFolder can't have same Id: {folderDto.Id}", 409);
            }

            var folder = await _unitOfWork.Folder.GetByIdAsync(folderDto.Id);

            if (folder == null)
            {
                throw new FolderException($"Folder with Id {folderDto.Id} not found!", 404);
            }

            await NameCheckAsync(folderDto);

            _mapper.Map(folderDto, folder);
            _unitOfWork.Folder.UpdateAsync(folder);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<FolderDto>(folder);
        }

        private async Task DeleteFolderRecursivelyAsync(Guid folderId)
        {
            var subFolders = await _unitOfWork.Folder.GetSubFoldersAsync(folderId);

            foreach (var subFolder in subFolders)
            {
                await DeleteFolderRecursivelyAsync(subFolder.Id);
            }

            var files = await _unitOfWork.Files.GetByFolderIdAsync(folderId);

            foreach (var file in files)
            {
                await _unitOfWork.Files.DeleteAsync(file.Id);
            }

            await _unitOfWork.Folder.DeleteAsync(folderId);
        }

        private async Task NameCheckAsync(FolderDto folderDto)
        {
            var nameExists = await _unitOfWork.Folder.FolderExistsAsync(folderDto.ParentFolderId, folderDto.Name);

            if (nameExists)
            {
                throw new FolderException($"A folder with the name '{folderDto.Name}' already exists in this folder!", 409);
            }
        }
    }
}
