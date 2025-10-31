namespace NotesApi.DTOs;
public record RegisterDto(string Username, string Password);
public record LoginDto(string Username, string Password);
public record AuthResult(string Token, int UserId, string Username);
