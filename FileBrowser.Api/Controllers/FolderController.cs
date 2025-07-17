using FileBrowser.Business.DTOs;
using FileBrowser.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace FileBrowser.Api.Controllers
{
    [ApiController]
    [Route("api/folders")]
    public class FolderController : ControllerBase
    {
        private readonly IFolderService _folderService;

        public FolderController(IFolderService folderService)
        {
            _folderService = folderService;
        }

        [HttpGet("{id:guid}", Name = "GetFolderById")]
        public async Task<ActionResult<FolderDto>> GetFolderByIdAsync(Guid id)
        {
            var folder = await _folderService.GetByFolderIdAsync(id);

            return Ok(folder);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FolderDto>>> GetAllFoldersAsync()
        {
            var folders = await _folderService.GetFoldersAsync();

            return Ok(folders);
        }

        [HttpGet("subfolders/{parentId:guid}")]
        public async Task<ActionResult<IEnumerable<FolderDto>>> GetSubFoldersAsync(Guid parentId)
        {
            var subFolders = await _folderService.GetSubFoldersAsync(parentId);

            return Ok(subFolders);
        }

        [HttpPost]
        public async Task<ActionResult<FolderDto>> AddFolderAsync([FromBody] FolderDto folderDto)
        {
            var folder = await _folderService.AddFolderAsync(folderDto);

            return CreatedAtRoute("GetFolderById", new { id = folder.Id }, folder);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteFolderAsync(Guid id)
        {
            await _folderService.DeleteAsync(id);

            return NoContent();
        }

        [HttpPut]
        public async Task<ActionResult<FolderDto>> UpdateFolderAsync([FromBody] FolderDto folderDto)
        {
            var updatedFolder = await _folderService.UpdateFolderAsync(folderDto);

            return Ok(updatedFolder);
        }
    }
}
