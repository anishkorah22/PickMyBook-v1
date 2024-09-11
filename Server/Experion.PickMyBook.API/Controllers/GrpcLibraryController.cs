/*using GrpcLibraryService;
using Microsoft.AspNetCore.Mvc;

namespace Experion.PickMyBook.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GrpcLibraryController : ControllerBase
    {
        private readonly LibraryService.LibraryServiceClient _grpcClient;

        public GrpcLibraryController(LibraryService.LibraryServiceClient grpcClient)
        {
            _grpcClient = grpcClient;
        }

        [HttpPost("uploadBooks")]
        public async Task<string> UploadBooks(IFormFile file)
        {
            try {
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);

                var request = new UploadFileRequest
                {
                    File = Google.Protobuf.ByteString.CopyFrom(memoryStream.ToArray()),
                    FileName = file.FileName
                };

                var response = await _grpcClient.UploadBookDataAsync(request);

                return response.Message;
            }
            catch (Exception ex)
            {
                // Log the exception
                return $"An error occurred: {ex.Message}";
            }

        }

        public async Task<string> UploadUsers(IFormFile file)
        {
            try
            {
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);

                var request = new UploadFileRequest
                {
                    File = Google.Protobuf.ByteString.CopyFrom(memoryStream.ToArray()),
                    FileName = file.FileName
                };

                var response = await _grpcClient.UploadUserDataAsync(request);

                return response.Message;
            }
            catch (Exception ex)
            {
                // Log the exception
                return $"An error occurred: {ex.Message}";
            }

        }
    }

}
*/