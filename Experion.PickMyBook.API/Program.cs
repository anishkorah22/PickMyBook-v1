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
using Experion.PickMyBook.Business.Services;
using Experion.PickMyBook.Data;



var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

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

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<LibraryContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IRepository<User>, UserRepository>();
builder.Services.AddScoped<IRepository<Book>, BookRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<BookService>();

builder.Services.AddGraphQLServer()
    .AddQueryType<ApiQueryType>()
    .AddMutationType<ApiMutationType>()
    .AddType<BookType>()
    .AddType<UserType>()
    .AddType<BorrowingType>()
    .AddAuthorization();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UsePlayground(new PlaygroundOptions { QueryPath = "/graphql", Path = "/playground" });
}

/*app.UseRouting();*/
app.UseHttpsRedirection();
app.MapGraphQL();
app.MapControllers();


app.Run();
