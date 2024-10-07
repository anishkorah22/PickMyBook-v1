using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Experion.PickMyBook.API.GraphQLTypes;
using Experion.PickMyBook.Infrastructure;
using Experion.PickMyBook.Business.Service;
using Experion.PickMyBook.Data;
using Experion.PickMyBook.Business.Service.IService;
using Experion.PickMyBook.Data.IRepository;
using Experion.PickMyBook.Business.Services;
using Experion.PickMyBook.API.Options;
using Microsoft.Extensions.Options;

namespace Experion.PickMyBook.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.Jwt));
            services.Configure<ConnectionStringOptions>(configuration.GetSection(ConnectionStringOptions.ConnectingServer));
            return services;
        }

        public static IServiceCollection AddApplicationAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var jwtOptions = new JwtOptions();
                configuration.GetSection(JwtOptions.Jwt).Bind(jwtOptions);

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key))
                };
            });

            return services;
        }

        public static IServiceCollection AddApplicationDbContext(this IServiceCollection services)
        {
            services.AddDbContext<LibraryContext>((serviceProvider, options) =>
            {
                var connectionStringOptions = serviceProvider.GetRequiredService<IOptions<ConnectionStringOptions>>().Value;
                options.UseNpgsql(connectionStringOptions.DefaultConnection);
            });

            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IBorrowingsRepository, BorrowingsRepository>();
            services.AddScoped<IRequestRepository, RequestRepository>();

            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBorrowingService, BorrowingService>();
            services.AddScoped<IRequestService, RequestService>();

            services.AddScoped<LibraryContext>();
            services.AddScoped<Query>();

            return services;
        }

        public static IServiceCollection AddApplicationGraphQL(this IServiceCollection services)
        {
            services.AddGraphQLServer()
                .AddQueryType<ApiQueryType>()
                .AddMutationType<ApiMutationType>()
                .AddSubscriptionType<ApiSubscriptionType>()
                .AddType<BookType>()
                .AddType<UserType>()
                .AddType<BorrowingType>()
                .AddType<UserType.DashboardCountsType>()
                .AddType<UploadType>()
                .AddInMemorySubscriptions()
                .AddAuthorization();
            

            return services;
        }
        public static IServiceCollection AddApplicationGrpc(this IServiceCollection services)
        {
            services.AddGrpcClient<Experion.PickMyBook.GrpcContracts.FileUploadService.FileUploadServiceClient>(o =>
            {
                o.Address = new Uri("https://localhost:7079"); // gRPC server address
            });

            return services;
        }

        public static IServiceCollection AddApplicationCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:5173")
                               .AllowAnyHeader()
                               .AllowAnyMethod()
                               .AllowCredentials(); // Allow cookies if needed
                    });
            });

            return services;
        }
    }
}
