using Dapper;
using System.Data;
using NotesApi.Models;

namespace NotesApi.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<int> CreateAsync(User user);
        Task<User?> GetByIdAsync(int id);
    }

    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _db;
        
        public UserRepository(IDbConnection db) 
        {
            _db = db;
        }

        public async Task<int> CreateAsync(User user)
        {
            var sql = @"INSERT INTO Users (Username, Password, CreatedAt)
                        VALUES (@Username, @Password, GETUTCDATE());
                        SELECT CAST(SCOPE_IDENTITY() as int);";
            return await _db.ExecuteScalarAsync<int>(sql, user);
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            var sql = "SELECT Id, Username, Password, CreatedAt FROM Users WHERE Id = @Id";
            return await _db.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            var sql = "SELECT Id, Username, Password, CreatedAt FROM Users WHERE Username = @Username";
            return await _db.QueryFirstOrDefaultAsync<User>(sql, new { Username = username });
        }
    }
}