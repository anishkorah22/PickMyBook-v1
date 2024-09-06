using Google.Protobuf;
using Microsoft.AspNetCore.Mvc;
using Upload;
using OfficeOpenXml; // Ensure you have the EPPlus package installed
using Experion.PickMyBook.Infrastructure.Models; // Ensure this namespace is correct
using System.Linq;
using Experion.PickMyBook.Infrastructure;

namespace Experion.PickMyBook.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileUploadController : ControllerBase
    {
        private readonly Upload.FileUploadService.FileUploadServiceClient _grpcClient;
        private readonly LibraryContext _context; // Ensure you have your DbContext injected
        private readonly ILogger<FileUploadController> _logger;

        public FileUploadController(Upload.FileUploadService.FileUploadServiceClient grpcClient, LibraryContext context, ILogger<FileUploadController> logger)
        {
            _grpcClient = grpcClient;
            _context = context;
            _logger = logger;
        }

        [HttpPost("upload-books")]
        /* public async Task<IActionResult> UploadBooks(IFormFile file)
         {
             if (file == null || file.Length == 0)
                 return BadRequest("Invalid file.");

             using var memoryStream = new MemoryStream();
             await file.CopyToAsync(memoryStream);
             var fileBytes = memoryStream.ToArray();

             var response = await _grpcClient.UploadBookFileAsync(new FileUploadRequest
             {
                 FileContent = ByteString.CopyFrom(fileBytes),
                 FileName = file.FileName
             });

             if (!response.Success)
                 return StatusCode(500, response.Message);

             return Ok(response.Message);
         }*/
        public async Task<IActionResult> UploadBooks(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            try
            {
                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);
                var fileContent = stream.ToArray();

                var books = ParseBooksFromExcel(fileContent);

                if (books == null || !books.Any())
                {
                    return BadRequest("No valid book data found in the file.");
                }

                // Set CreatedAt and UpdatedAt for each book
                foreach (var book in books)
                {
                    book.CreatedAt = DateTime.UtcNow;
                    book.UpdatedAt = DateTime.UtcNow;
                }

                await _context.Books.AddRangeAsync(books);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Books uploaded successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while uploading books.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private List<Book> ParseBooksFromExcel(byte[] fileContent)
        {
            var books = new List<Book>();

            using (var package = new ExcelPackage(new MemoryStream(fileContent)))
            {
                var worksheet = package.Workbook.Worksheets[0];
                var rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++) // Assuming the first row is a header
                {
                    var title = worksheet.Cells[row, 1].Text;
                    var author = worksheet.Cells[row, 2].Text;
                    var isbn = worksheet.Cells[row, 3].Text;
                    var publisher = worksheet.Cells[row, 4].Text;
                    var availableCopies = int.Parse(worksheet.Cells[row, 5].Text);
                    var publishedYear = int.Parse(worksheet.Cells[row, 6].Text);
                    var genre = worksheet.Cells[row, 7].Text;
                    var imageUrlsString = worksheet.Cells[row, 8].Text; // Assuming ImageUrls is in the 8th column

                    // Convert the comma-separated string to a string array
                    var imageUrls = string.IsNullOrEmpty(imageUrlsString)
                        ? Array.Empty<string>()
                        : imageUrlsString.Split(',').Select(url => url.Trim()).ToArray();

                    var book = new Book
                    {
                        Title = title,
                        Author = author,
                        ISBN = isbn,
                        Publisher = publisher,
                        AvailableCopies = availableCopies,
                        PublishedYear = publishedYear,
                        Genre = genre,
                        ImageUrls = imageUrls, // Assign the image URLs array
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    books.Add(book);
                }
            }

            return books;
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

                var users = ParseUsersFromExcel(fileContent);

                if (users == null || !users.Any())
                {
                    return BadRequest("No valid user data found in the file.");
                }

                // Set CreatedAt and UpdatedAt for each user
                foreach (var user in users)
                {
                    user.CreatedAt = DateTime.UtcNow;
                    user.UpdatedAt = DateTime.UtcNow;
                }

                await _context.Users.AddRangeAsync(users);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Users uploaded successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while uploading users.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private List<User> ParseUsersFromExcel(byte[] fileContent)
        {
            var users = new List<User>();

            using (var package = new ExcelPackage(new MemoryStream(fileContent)))
            {
                var worksheet = package.Workbook.Worksheets[0];
                var rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++) // Assuming the first row is a header
                {
                    var userName = worksheet.Cells[row, 1].Text;
                    var roles = worksheet.Cells[row, 2].Text.Split(',').Select(r => r.Trim()).ToList();

                    var user = new User
                    {
                        UserName = userName,
                        Roles = roles,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    users.Add(user);
                }
            }

            return users;
        }
    }
}
