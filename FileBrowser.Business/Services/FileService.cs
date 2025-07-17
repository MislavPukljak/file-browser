using AutoMapper;
using FileBrowser.Business.DTOs;
using FileBrowser.Business.Exceptions;
using FileBrowser.Data.Entities;
using FileBrowser.Data.Repositories;

namespace FileBrowser.Business.Services
{
    public class FileService : IFileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FileService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<FileDto> AddFileAsync(FileDto fileDto)
        {
            await NameCheckAsync(fileDto);

            var file = _mapper.Map<FileEntity>(fileDto); 
            await _unitOfWork.Files.AddAsync(file);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<FileDto>(file);
        }

        public async Task DeleteFileAsync(Guid id)
        {
            var file = await _unitOfWork.Files.GetByIdAsync(id);

            if (file == null)
            {
                throw new FileException($"A file with ID '{id}' not found!", 404);

            }

            await _unitOfWork.Files.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<FileDto>> GetAllFilesAsync()
        {
            var files = await _unitOfWork.Files.GetAllAsync();

            var filesDto = _mapper.Map<IEnumerable<FileDto>>(files);

            return filesDto;
        }

        public async Task<FileDto> GetByFileIdAsync(Guid id)
        {
            var file = await _unitOfWork.Files.GetByIdAsync(id);

            if (file == null)
            {
                throw new FileException($"A file with ID '{id}' not found!", 404);
            }

            var fileDto = _mapper.Map<FileDto>(file);

            return fileDto;
        }

        public async Task<IEnumerable<FileDto>> GetByFolderIdAsync(Guid folderId)
        {
            await FolderCheckAsync(folderId);

            var files = await _unitOfWork.Files.GetByFolderIdAsync(folderId);

            return _mapper.Map<IEnumerable<FileDto>>(files);
        }

        public async Task<IEnumerable<FileDto>> SearchFilesAsync(string search, int top = 10)
        {
            var files = await _unitOfWork.Files.SearchFilesAsync(search, top);

            return _mapper.Map<IEnumerable<FileDto>>(files);
        }

        public async Task<IEnumerable<FileDto>> SearchFilesInFolderAsync(Guid folderId, string search, int top = 10)
        {
            await FolderCheckAsync(folderId);
            
            var files = await _unitOfWork.Files.SearchFilesInFolderAsync(folderId, search, top);

            return _mapper.Map<IEnumerable<FileDto>>(files);
        }

        public async Task<FileDto> UpdateFileAsync(FileDto fileDto)
        {
            var file = await _unitOfWork.Files.GetByIdAsync(fileDto.Id);

            if (file == null)
            {
                throw new FileException($"A file with ID '{fileDto.Id}' not found!", 404);
            }

            await NameCheckAsync(fileDto);

            _mapper.Map(fileDto, file);

            _unitOfWork.Files.UpdateAsync(file);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<FileDto>(file);
        }
        
        private async Task NameCheckAsync(FileDto fileDto)
        {
            var nameExists = await _unitOfWork.Files.FileExistsAsync(fileDto.FolderId, fileDto.Name);

            if (nameExists)
            {
                throw new FileException($"A file with the name '{fileDto.Name}' already exists in this folder!", 409);
            }
        }

        private async Task FolderCheckAsync(Guid id)
        {
            var folder = await _unitOfWork.Folder.GetByIdAsync(id);

            if (folder == null)
            {
                throw new FolderException($"A folder with ID '{id}' not found!", 404);
            }
        }
    }
}
