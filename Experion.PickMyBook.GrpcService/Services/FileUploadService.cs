using Experion.PickMyBook.GrpcService;
using Experion.PickMyBook.Infrastructure;
using Experion.PickMyBook.Infrastructure.Models;
using Grpc.Core;
using OfficeOpenXml;
using Upload;

namespace Experion.PickMyBook.GrpcService.Services
{
    public class FileUploadService : Upload.FileUploadService.FileUploadServiceBase
    {
        private readonly LibraryContext _context;

        public FileUploadService(LibraryContext context)
        {
            _context = context;
        }

        public override async Task<FileUploadResponse> UploadBookFile(FileUploadRequest request, ServerCallContext context)
        {
            try
            {
                var books = ParseBooksFromExcel(request.FileContent.ToByteArray()); // Implement this to parse Excel file
                await _context.Books.AddRangeAsync(books);
                await _context.SaveChangesAsync();

                return new FileUploadResponse
                {
                    Message = "Books uploaded successfully",
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new FileUploadResponse
                {
                    Message = $"Error: {ex.Message}",
                    Success = false
                };
            }
        }

        public override async Task<FileUploadResponse> UploadUserFile(FileUploadRequest request, ServerCallContext context)
        {
            try
            {
                var users = ParseUsersFromExcel(request.FileContent.ToByteArray()); // Implement this to parse Excel file
                await _context.Users.AddRangeAsync(users);
                await _context.SaveChangesAsync();

                return new FileUploadResponse
                {
                    Message = "Users uploaded successfully",
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new FileUploadResponse
                {
                    Message = $"Error: {ex.Message}",
                    Success = false
                };
            }
        }

        private List<Book> ParseBooksFromExcel(byte[] fileContent)
        {
            using (var package = new ExcelPackage(new MemoryStream(fileContent)))
            {
                var worksheet = package.Workbook.Worksheets[0];
                var books = new List<Book>();

                for (int row = 2; row <= worksheet.Dimension.End.Row; row++) // Assuming first row is header
                {
                    var book = new Book
                    {
                        Title = worksheet.Cells[row, 1].Value?.ToString(),  // Parse from Excel
                        Author = worksheet.Cells[row, 2].Value?.ToString(), // Parse from Excel
                        ISBN = worksheet.Cells[row, 3].Value?.ToString(),   // Parse from Excel
                        Publisher = worksheet.Cells[row, 4].Value?.ToString(), // Parse from Excel
                        AvailableCopies = int.TryParse(worksheet.Cells[row, 5].Value?.ToString(), out var availableCopies) ? availableCopies : (int?)null, // Parse from Excel
                        PublishedYear = int.TryParse(worksheet.Cells[row, 6].Value?.ToString(), out var publishedYear) ? publishedYear : (int?)null, // Parse from Excel
                        Genre = worksheet.Cells[row, 7].Value?.ToString(),  // Parse from Excel
                        ImageUrls = worksheet.Cells[row, 8].Value?.ToString()?.Split(','),  // Parse from Excel
                        IsDeleted = false, // Generated in API call
                        CreatedAt = DateTime.UtcNow, // Generated in API call
                        UpdatedAt = DateTime.UtcNow  // Generated in API call
                    };

                    books.Add(book);
                }

                return books;
            }
        }


        private List<User> ParseUsersFromExcel(byte[] fileContent)
        {
            using (var package = new ExcelPackage(new MemoryStream(fileContent)))
            {
                var worksheet = package.Workbook.Worksheets[0];
                var users = new List<User>();

                for (int row = 2; row <= worksheet.Dimension.End.Row; row++) // Assuming first row is header
                {
                    var user = new User
                    {
                        UserName = worksheet.Cells[row, 1].Value?.ToString(),  // Only UserName from Excel
                        Roles = worksheet.Cells[row, 2].Value?.ToString()?.Split(','), // Only Roles from Excel
                        IsDeleted = false, // Generated in API call
                        CreatedAt = DateTime.UtcNow, 
                        UpdatedAt = DateTime.UtcNow  
                    };

                    users.Add(user);
                }

                return users;
            }
        }

    }

}
