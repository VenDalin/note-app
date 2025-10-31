using Dapper;
using NotesApi.DTOs;
using NotesApi.Models;
using System.Data;
using System.Text;

namespace NotesApi.Repositories
{

    public interface INoteRepository
    {
        Task<PaginatedResult<Note>> QueryNotesAsync(int userId, NoteQueryParams p);
        Task<Note?> GetByIdAsync(int id, int userId);
        Task<int> CreateAsync(Note note);
        Task UpdateAsync(Note note);
        Task DeleteAsync(int id, int userId);
    }
    public class NoteRepository : INoteRepository
    {
        private readonly IDbConnection _db;

        public NoteRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<PaginatedResult<Note>> QueryNotesAsync(int userId, NoteQueryParams p)
        {
            var whereClause = new StringBuilder("WHERE UserId = @UserId");
            var parameters = new DynamicParameters();
            parameters.Add("UserId", userId);
            parameters.Add("Skip", p.GetSkip());
            parameters.Add("PageSize", p.GetValidPageSize());

            // Search
            if (!string.IsNullOrWhiteSpace(p.SearchTerm))
            {
                whereClause.Append(" AND (Title LIKE @SearchTerm OR Content LIKE @SearchTerm)");
                parameters.Add("SearchTerm", $"%{p.SearchTerm}%");
            }

            // Date range filter
            if (!string.IsNullOrWhiteSpace(p.StartDate))
            {
                whereClause.Append(" AND CAST(CreatedAt AS DATE) >= @StartDate");
                parameters.Add("StartDate", DateTime.Parse(p.StartDate).Date);
            }
            if (!string.IsNullOrWhiteSpace(p.EndDate))
            {
                whereClause.Append(" AND CAST(CreatedAt AS DATE) <= @EndDate");
                parameters.Add("EndDate", DateTime.Parse(p.EndDate).Date);
            }

            // Sorting
            var validSortColumns = new[] { "createdat", "updatedat", "title" };
            var sortByValue = p.SortBy ?? "createdat";
            var sortBy = validSortColumns.Contains(sortByValue.ToLower())
                ? sortByValue.ToLower() : "createdat";
            
            var sortOrder = p.SortOrder?.ToLower() == "asc" ? "ASC" : "DESC";
            
            var columnMap = new Dictionary<string, string>
            {
                { "createdat", "CreatedAt" },
                { "updatedat", "UpdatedAt" },
                { "title", "Title" }
            };
            var orderByClause = $"ORDER BY {columnMap[sortBy]} {sortOrder}";

            var countSql = $"SELECT COUNT(*) FROM Notes {whereClause}";
            var totalCount = await _db.ExecuteScalarAsync<int>(countSql, parameters);

            var dataSql = $@"
                SELECT Id, Title, Content, CreatedAt, UpdatedAt, UserId 
                FROM Notes 
                {whereClause}
                {orderByClause}
                OFFSET @Skip ROWS 
                FETCH NEXT @PageSize ROWS ONLY";

            var notes = await _db.QueryAsync<Note>(dataSql, parameters);

            return new PaginatedResult<Note>
            {
                Items = notes.ToList(),
                TotalCount = totalCount,
                Page = p.GetValidPage(),
                PageSize = p.GetValidPageSize()
            };
        }

        public async Task<Note?> GetByIdAsync(int id, int userId)
        {
            var sql = @"
                SELECT n.Id, n.Title, n.Content, n.CreatedAt, n.UpdatedAt, n.UserId, u.UserName AS CreatedBy
                FROM Notes n
                JOIN Users u ON n.UserId = u.Id
                WHERE n.Id = @Id AND n.UserId = @UserId";
            return await _db.QueryFirstOrDefaultAsync<Note>(sql, new { Id = id, UserId = userId });
        }

        public async Task<int> CreateAsync(Note note)
        {
            var sql = @"
                INSERT INTO Notes (Title, Content, CreatedAt, UserId) 
                VALUES (@Title, @Content, GETUTCDATE(), @UserId);
                SELECT CAST(SCOPE_IDENTITY() as int);";
            return await _db.ExecuteScalarAsync<int>(sql, note);
        }

        public async Task UpdateAsync(Note note)
        {
            var sql = @"
                UPDATE Notes 
                SET Title = @Title, Content = @Content, UpdatedAt = GETUTCDATE() 
                WHERE Id = @Id AND UserId = @UserId";
            await _db.ExecuteAsync(sql, note);
        }

        public async Task DeleteAsync(int id, int userId)
        {
            var sql = "DELETE FROM Notes WHERE Id = @Id AND UserId = @UserId";
            await _db.ExecuteAsync(sql, new { Id = id, UserId = userId });
        }
    }
}