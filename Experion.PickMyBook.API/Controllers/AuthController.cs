using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using Experion.PickMyBook.Data;
using Experion.PickMyBook.Infrastructure.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Experion.PickMyBook.Infrastructure;
using Experion.PickMyBook.API.Options;
using Microsoft.Extensions.Options;

namespace Experion.PickMyBook.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly LibraryContext _context;
        private readonly JwtOptions _jwtOptions;

        public class EnumerableToStringArrayConverter : ValueConverter<IEnumerable<string>, string[]>
        {
            public EnumerableToStringArrayConverter()
                : base(
                    v => v.ToArray(), // Convert IEnumerable<string> to string[]
                    v => v.AsEnumerable()) // Convert string[] to IEnumerable<string>
            {
            }
        }
        public AuthController(LibraryContext context, IOptions<JwtOptions> jwtOptions)
        {
            _context = context;
            _jwtOptions = jwtOptions.Value;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            // Check if the user already exists
            if (_context.Users.Any(u => u.UserName == request.Email))
                return Conflict("User already exists");

            // Create a new user
            var user = new User
            {
                UserName = request.Email,
                Roles = new List<string> { "User" }, // Default role
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User created successfully.");
        }

        // Endpoint to authenticate and issue a JWT token
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == email);

            if (user == null || user.IsDeleted)
                return Unauthorized("Invalid credentials");

            var token = GenerateJwtToken(user);
            return Ok(new { token });
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtOptions.Key);

            // Convert IEnumerable<string> to IList<string> or directly use ToList() for string.Join
            var rolesList = user.Roles.ToList(); // Convert to List to ensure it's in the expected format

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, string.Join(",", rolesList)), // Use the converted list
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        [Authorize]
        [HttpGet("getUserInfo")]
        public IActionResult GetUserInfo()
        {
            var userName = User.Identity.Name;
            var user = _context.Users.FirstOrDefault(u => u.UserName == userName);

            if (user == null)
                return NotFound("User not found");

            return Ok(new
            {
                user.UserId,
                user.UserName,
                Roles = user.Roles,
                user.IsDeleted,
                user.CreatedAt,
                user.UpdatedAt
            });
        }
    }

    public class RegisterRequest
    {
        public string Email { get; set; }
    }
}
