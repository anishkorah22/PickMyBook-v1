using Microsoft.AspNetCore.Mvc;
using Experion.PickMyBook.Infrastructure.Models;
using Experion.PickMyBook.Infrastructure;
using OfficeOpenXml;
using Experion.PickMyBook.GrpcContracts;
using Google.Protobuf;

namespace Experion.PickMyBook.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileUploadController : ControllerBase
    {
        private readonly FileUploadService.FileUploadServiceClient _grpcClient;
        private readonly LibraryContext _context;

        public FileUploadController(FileUploadService.FileUploadServiceClient grpcClient, LibraryContext context)
        {
            _grpcClient = grpcClient;
            _context = context;
        }

        [HttpPost("upload-books")]
        public async Task<IActionResult> UploadBooks(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            try
            {
                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);
                var fileContent = stream.ToArray();

                var request = new FileUploadRequest
                {
                    FileContent = ByteString.CopyFrom(fileContent),
                    FileName = file.FileName
                };

                var response = await _grpcClient.UploadBookFileAsync(request);

                if (response.Success)
                {
                    return Ok(new { Message = response.Message });
                }
                else
                {
                    return BadRequest(response.Message);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPost("upload-users")]
        public async Task<IActionResult> UploadUsers(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            try
            {
                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);
                var fileContent = stream.ToArray();

                var request = new FileUploadRequest
                {
                    FileContent = ByteString.CopyFrom(fileContent),
                    FileName = file.FileName
                };

                var response = await _grpcClient.UploadUserFileAsync(request);

                if (response.Success)
                {
                    return Ok(new { Message = response.Message });
                }
                else
                {
                    return BadRequest(response.Message);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }


}
