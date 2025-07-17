using FileBrowser.Business.DTOs;
using FileBrowser.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace FileBrowser.Api.Controllers
{
    [ApiController]
    [Route("api/files")]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost]
        public async Task<ActionResult<FileDto>> AddAsync([FromBody] FileDto fileDto)
        {
            var newFile = await _fileService.AddFileAsync(fileDto);

            return CreatedAtRoute("GetByFileId", new { id = newFile.Id }, newFile);
        }

        [HttpGet("{id:guid}", Name = "GetByFileId")]
        public async Task<ActionResult<FileDto>> GetFileByIdAsync(Guid id)
        {
            var file = await _fileService.GetByFileIdAsync(id);

            return Ok(file);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FileDto>>> GetAllFilesAsync()
        {
            var files = await _fileService.GetAllFilesAsync();

            return Ok(files);
        }

        [HttpGet("folder/{folderId:guid}")]
        public async Task<ActionResult<IEnumerable<FileDto>>> GetByFolderidAsync(Guid folderId)
        {
            var files = await _fileService.GetByFolderIdAsync(folderId);

            return Ok(files);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<FileDto>>> SearchFilesAsync([FromQuery] string query, [FromQuery] int top = 10)
        {
            var files = await _fileService.SearchFilesAsync(query, top);

            return Ok(files);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _fileService.DeleteFileAsync(id);

            return NoContent();
        }

        [HttpPut]
        public async Task<ActionResult<FileDto>> UpdateFileAsync([FromBody] FileDto fileDto)
        {
            var updatedFile = await _fileService.UpdateFileAsync(fileDto);

            return Ok(updatedFile);
        }

        [HttpGet("search-in-folder/{folderId}")]
        public async Task<ActionResult<IEnumerable<FileDto>>> SearchFilesInFolderAsync(Guid folderId, [FromQuery] string search, [FromQuery] int top = 10)
        {
            var files = await _fileService.SearchFilesInFolderAsync(folderId, search, top);

            return Ok(files);
        }
    }
}
