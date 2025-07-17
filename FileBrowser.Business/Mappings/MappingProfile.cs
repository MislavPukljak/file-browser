using AutoMapper;
using FileBrowser.Business.DTOs;
using FileBrowser.Data.Entities;

namespace FileBrowser.Business.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<FileEntity, FileDto>()
                .ReverseMap();

            CreateMap<Folder, FolderDto>()
                .ReverseMap();

            CreateMap<FolderDto, Folder>()
            .ForMember(dest => dest.Files, opt => opt.Ignore())
            .ForMember(dest => dest.SubFolders, opt => opt.Ignore())
            .ForMember(dest => dest.ParentFolder, opt => opt.Ignore());
        }
    }
}
