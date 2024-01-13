using CoursIPI.Models;
using CoursIPI.Models.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CoursIPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly DatabaseContext _context;
    private readonly IConfiguration _config;
    public UserController(IConfiguration config, DatabaseContext context)
    {
        _config = config;
        _context = context;
    }

    [HttpPost("AddUser")]
    public async Task<IActionResult> AddUser([FromBody] User newUser)
    {
        // Vérification de la validité de l'utilisateur
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Ajout de l'utilisateur à la base de données
        await _context.Users.AddAsync(newUser);
        await _context.SaveChangesAsync();

        return Ok(newUser);
    }


    [AllowAnonymous]
    [HttpPost]
    public IActionResult Login([FromBody] User userModel)
    {
        var user = Authenticate(userModel);

        if (user != null)
        {
            var token = Generate(user);
            return Ok(token);
        }

        return NotFound("User not found");
    }

    private string Generate(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Username),
        };

        var token = new JwtSecurityToken(_config["Jwt:Issuer"],
          _config["Jwt:Audience"],
          claims,
          expires: DateTime.Now.AddMinutes(15),
          signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private User Authenticate(User userModel)
    {
        var currentUser = _context.Users.FirstOrDefault(u => u.Username.ToLower() == userModel.Username.ToLower() && u.Password == userModel.Password);

        if (currentUser != null)
        {
            return new User
            {
                Username = currentUser.Username,
            };
        }
        return null;
    }
}
