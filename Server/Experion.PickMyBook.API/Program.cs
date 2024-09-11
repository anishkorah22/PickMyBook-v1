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
    .AddApplicationGraphQL()
    .AddRazorPages();
    

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationCors();
builder.Services.AddApplicationGrpc();  

var app = builder.Build();

app.UseWebSockets();
app.UseRouting();
app.UseCors("AllowSpecificOrigins");


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

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles(); // Ensure static files middleware is enabled

app.UseRouting();
app.MapRazorPages();
app.MapControllers();

app.Run();