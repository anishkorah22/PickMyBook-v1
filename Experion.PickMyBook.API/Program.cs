using Experion.PickMyBook.API.Extensions;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Playground;

var builder = WebApplication.CreateBuilder(args);

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