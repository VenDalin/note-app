using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using NotesApi.Models;

public class AuthService
{
    private readonly IConfiguration _config;
    public AuthService(IConfiguration config) => _config = config;

    public string GenerateJwtToken(User user)
    {
        var key = _config["Jwt:Key"];
        if (string.IsNullOrWhiteSpace(key))
            throw new InvalidOperationException("JWT key not configured");

        var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim("id", user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username)
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}