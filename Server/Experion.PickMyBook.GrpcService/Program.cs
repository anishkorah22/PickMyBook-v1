using Experion.PickMyBook.GrpcService.Services;
using Experion.PickMyBook.Infrastructure;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<LibraryContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddGrpc();
/*builder.Services.AddSingleton<IExcelParser, ExcelParser>();*/

var app = builder.Build();
app.MapGrpcService<FileUploadService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
