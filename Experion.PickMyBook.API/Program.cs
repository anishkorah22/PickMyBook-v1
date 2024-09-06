using Experion.PickMyBook.API.Extensions;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Playground;
using OfficeOpenXml;

var builder = WebApplication.CreateBuilder(args);

// Set the EPPlus license context
ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Set license context before building the host
// Configure Options and Services
builder.Services
    .AddApplicationOptions(builder.Configuration)
    .AddApplicationAuthentication(builder.Configuration)
    .AddApplicationDbContext()
    .AddApplicationServices()
    .AddApplicationGraphQL();

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddGrpcClient<Upload.FileUploadService.FileUploadServiceClient>(o =>
{
    o.Address = new Uri("https://localhost:7079"); // gRPC server address
});



var app = builder.Build();

app.UseWebSockets();
app.UseRouting();

// CORS configuration
app.UseCors(builder =>
    builder.WithOrigins("https://localhost:7131")
           .AllowAnyHeader()
           .AllowAnyMethod()
           .AllowCredentials()
           .WithExposedHeaders("Content-Disposition"));

app.UseAuthentication();
app.UseAuthorization();
app.MapGraphQL();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UsePlayground(new PlaygroundOptions { QueryPath = "/graphql", Path = "/playground" });
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();