using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;
using NotesApi.DTOs;
using NotesApi.Models;
using NotesApi.Repositories;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepo;
    private readonly AuthService _authService;
    public AuthController(IUserRepository userRepo, AuthService authService)
    {
        _userRepo = userRepo;
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        try
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest("Username and password are required");

            var existing = await _userRepo.GetByUsernameAsync(dto.Username.Trim());
            if (existing != null) return BadRequest("Username already used");

            var hashed = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            var user = new User { Username = dto.Username.Trim(), Password = hashed };

            var id = await _userRepo.CreateAsync(user);
            user.Id = id;

            var token = _authService.GenerateJwtToken(user);
            return Ok(new AuthResult(token, user.Id, user.Username));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var user = await _userRepo.GetByUsernameAsync(dto.Username.Trim());
        if (user == null) return Unauthorized("Invalid credentials");

        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            return Unauthorized("Invalid credentials");

        var token = _authService.GenerateJwtToken(user);
        return Ok(new AuthResult(token, user.Id, user.Username));
    }
}
