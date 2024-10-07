using Experion.PickMyBook.GrpcContracts;
using Experion.PickMyBook.Infrastructure;
using Experion.PickMyBook.Infrastructure.Models;
using Grpc.Core;
using OfficeOpenXml;

namespace Experion.PickMyBook.GrpcService.Services
{
    public class FileUploadService : GrpcContracts.FileUploadService.FileUploadServiceBase
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
                var books = ParseBooksFromExcel(request.FileContent.ToByteArray());
                if (books.Any())
                {
                    await _context.Books.AddRangeAsync(books);
                    await _context.SaveChangesAsync();
                }

                return new FileUploadResponse
                {
                    Message = "Books uploaded successfully.",
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new FileUploadResponse
                {
                    Message = $"Error occurred: {ex.Message}",
                    Success = false
                };
            }
        }

        public override async Task<FileUploadResponse> UploadUserFile(FileUploadRequest request, ServerCallContext context)
        {
            try
            {
                var users = ParseUsersFromExcel(request.FileContent.ToByteArray());
                if (users.Any())
                {
                    await _context.Users.AddRangeAsync(users);
                    await _context.SaveChangesAsync();
                }

                return new FileUploadResponse
                {
                    Message = "Users uploaded successfully.",
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new FileUploadResponse
                {
                    Message = $"Error occurred: {ex.Message}",
                    Success = false
                };
            }
        }

        private List<Book> ParseBooksFromExcel(byte[] fileContent)
        {
            var books = new List<Book>();
            using (var package = new ExcelPackage(new MemoryStream(fileContent)))
            {
                var worksheet = package.Workbook.Worksheets[0];
                var rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    var book = new Book
                    {
                        Title = worksheet.Cells[row, 1].Text,
                        Author = worksheet.Cells[row, 2].Text,
                        ISBN = worksheet.Cells[row, 3].Text,
                        Publisher = worksheet.Cells[row, 4].Text,
                        AvailableCopies = int.Parse(worksheet.Cells[row, 5].Text),
                        PublishedYear = int.Parse(worksheet.Cells[row, 6].Text),
                        Genre = worksheet.Cells[row, 7].Text,
                        ImageUrls = worksheet.Cells[row, 8].Text.Split(',').Select(s => s.Trim()).ToArray(),
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    books.Add(book);
                }
            }

            return books;
        }

        private List<User> ParseUsersFromExcel(byte[] fileContent)
        {
            var users = new List<User>();
            using (var package = new ExcelPackage(new MemoryStream(fileContent)))
            {
                var worksheet = package.Workbook.Worksheets[0];
                var rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    var user = new User
                    {
                        UserName = worksheet.Cells[row, 1].Text,
                        RoleTypeId = (int)Enum.Parse(typeof(RoleTypeValue), worksheet.Cells[row, 2].Text),
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
