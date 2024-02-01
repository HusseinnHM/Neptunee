using Microsoft.AspNetCore.Mvc;
using Neptunee.IO.Files;

namespace Sample.API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public  class FilesController : ControllerBase
{
    public record UploadRequest(IFormFile File);
    [HttpPost]
    public async Task<IActionResult> Upload([FromForm] UploadRequest request, [FromServices] INeptuneeFileService fileService)
    {
        return Ok(await fileService.UploadAsync(request.File));
    }
}