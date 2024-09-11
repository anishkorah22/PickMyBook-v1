using Grpc.Core;
using System.IO;
using OfficeOpenXml;
using BookService;
using OfficeOpenXml.Core;
using Experion.PickMyBook.Data.IRepository;

/*public class BookUploadService : BookService.BookUploadServiceBase
{
    private readonly IBookRepository _bookRepository; // Assume you have a repository for DB operations

    public BookUploadService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public override async Task<BookUploadResponse> UploadBooks(BookUploadRequest request, ServerCallContext context)
    {
        try
        {
            // Convert the byte array back to a stream
            using var stream = new MemoryStream(request.File.ToByteArray());

            // Process the Excel file
            var books = ParseExcelFile(stream);

            // Save books to the database
            await _bookRepository.BulkInsertBooksAsync(books);

            return new BookUploadResponse
            {
                Success = true,
                Message = $"{books.Count} books have been uploaded successfully."
            };
        }
        catch (Exception ex)
        {
            return new BookUploadResponse
            {
                Success = false,
                Message = $"Error during upload: {ex.Message}"
            };
        }
    }

    private List<Book> ParseExcelFile(Stream stream)
    {
        var books = new List<Book>();

        using (var package = new ExcelPackage(stream))
        {
            var worksheet = package.Workbook.Worksheets[0];
            var rowCount = worksheet.Dimension.End.Row;
            var columnCount = worksheet.Dimension.End.Column;

            for (int row = 2; row <= rowCount; row++) // Assuming the first row is headers
            {
                var book = new Book
                {
                    Title = worksheet.Cells[row, 1].Value?.ToString(),
                    Author = worksheet.Cells[row, 2].Value?.ToString(),
                    ISBN = worksheet.Cells[row, 3].Value?.ToString(),
                    PublishedYear = int.Parse(worksheet.Cells[row, 4].Value?.ToString()),
                    AvailableCopies = int.Parse(worksheet.Cells[row, 5].Value?.ToString())
                };

                books.Add(book);
            }
        }
        return books;
    }
}
*/