using System.IO;
using System.Threading.Tasks;
using Grpc.Core;
using OfficeOpenXml;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Experion.PickMyBook.Infrastructure;
using Experion.PickMyBook.Infrastructure.Models;

namespace GrpcLibraryService.Services
{
    public class LibraryServiceImpl : LibraryService.LibraryServiceBase
    {
        private readonly LibraryContext _context;
       

        public LibraryServiceImpl(LibraryContext context)
        {
            _context = context;
            
        }

        public override async Task<UploadFileResponse> UploadBookData(UploadFileRequest request, ServerCallContext context)
        {
            var response = new UploadFileResponse();
            var tempFilePath = Path.Combine(Path.GetTempPath(), request.FileName);

            try
            {
                await File.WriteAllBytesAsync(tempFilePath, request.File.ToByteArray());

                using (var package = new ExcelPackage(new FileInfo(tempFilePath)))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var books = new List<Book>();

                    for (int row = 2; row <= worksheet.Dimension.Rows; row++)
                    {
                        var book = new Book
                        {
                            Title = worksheet.Cells[row, 1].Text,
                            Author = worksheet.Cells[row, 2].Text,
                            ISBN = worksheet.Cells[row, 3].Text,
                            Publisher = worksheet.Cells[row, 4].Text,
                            AvailableCopies = int.TryParse(worksheet.Cells[row, 5].Text, out var copies) ? copies : 0,
                            PublishedYear = int.TryParse(worksheet.Cells[row, 6].Text, out var year) ? year : 0,
                            Genre = worksheet.Cells[row, 7].Text,
                            ImageUrls = new string[]
                            {
                            worksheet.Cells[row, 8].Text,
                            worksheet.Cells[row, 9].Text,
                            worksheet.Cells[row, 10].Text
                            }
                        };

                        books.Add(book);
                    }

                    _context.Books.AddRange(books);
                    await _context.SaveChangesAsync();
                }

                response.Success = true;
                response.Message = "Books uploaded successfully";
            }
            catch (Exception ex)
            {
                
                response.Success = false;
                response.Message = "Error uploading books";
            }
            finally
            {
                if (File.Exists(tempFilePath))
                {
                    File.Delete(tempFilePath);
                }
            }

            return response;
        }

        public override async Task<UploadFileResponse> UploadUserData(UploadFileRequest request, ServerCallContext context)
        {
            var response = new UploadFileResponse();
            var tempFilePath = Path.Combine(Path.GetTempPath(), request.FileName);

            try
            {
                await File.WriteAllBytesAsync(tempFilePath, request.File.ToByteArray());

                using (var package = new ExcelPackage(new FileInfo(tempFilePath)))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var users = new List<User>();

                    for (int row = 2; row <= worksheet.Dimension.Rows; row++)
                    {
                        var user = new User
                        {
                            UserName = worksheet.Cells[row, 1].Text,
                            Roles = worksheet.Cells[row, 2].Text.Split(',') // Assuming roles are comma-separated in the Excel file
                        };

                        users.Add(user);
                    }

                    _context.Users.AddRange(users);
                    await _context.SaveChangesAsync();
                }

                response.Success = true;
                response.Message = "Users uploaded successfully";
            }
            catch (Exception ex)
            {
               
                response.Success = false;
                response.Message = "Error uploading users";
            }
            finally
            {
                if (File.Exists(tempFilePath))
                {
                    File.Delete(tempFilePath);
                }
            }

            return response;
        }
    }
}
