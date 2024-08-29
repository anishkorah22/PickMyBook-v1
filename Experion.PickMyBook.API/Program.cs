using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Experion.PickMyBook.Infrastructure.Models;
using Experion.PickMyBook.API.GraphQLTypes;
using HotChocolate.AspNetCore.Playground;
using HotChocolate.AspNetCore;
using Experion.PickMyBook.Infrastructure;
using Microsoft.Extensions.Configuration;
using Experion.PickMyBook.Business.Service;
using Experion.PickMyBook.Data;
using Experion.PickMyBook.Business.Service.IService;
using Experion.PickMyBook.Data.IRepository;
using Experion.PickMyBook.Business.Services;
var builder = WebApplication.CreateBuilder(args);

// Configure JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.AddAuthorization();

// Configure services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure DbContext
builder.Services.AddDbContext<LibraryContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBorrowingsRepository, BorrowingsRepository>();
builder.Services.AddScoped<IRequestRepository, RequestRepository>();

// Register Services
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBorrowingService, BorrowingService>();
builder.Services.AddScoped<IRequestService, RequestService>();

// Configure GraphQL Server
builder.Services.AddGraphQLServer()
    .AddQueryType<ApiQueryType>()
    .AddMutationType<ApiMutationType>()
    .AddSubscriptionType<ApiSubscriptionType>() 
    .AddType<BookType>()
    .AddType<UserType>()
    .AddType<BorrowingType>()
    .AddType<UserType.DashboardCountsType>()
    .AddInMemorySubscriptions()
    .AddAuthorization();

var app = builder.Build();
app.UseWebSockets(); // Ensure WebSockets are enabled
app.UseRouting(); // Add routing middleware before mapping endpoints

// CORS configuration
app.UseCors(builder =>
    builder.WithOrigins("https://localhost:7131")
           .AllowAnyHeader()
           .AllowAnyMethod()
           .AllowCredentials()
           .WithExposedHeaders("Content-Disposition"));

app.UseAuthentication();
app.UseAuthorization();
app.MapGraphQL(); // Map GraphQL endpoint
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
