namespace NotesApi.DTOs
{
    public class NoteQueryParams
    {
        public string? SearchTerm { get; set; }
        public string? SortBy { get; set; } = "createdAt"; 
        public string? SortOrder { get; set; } = "desc";
        
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        
        public string? StartDate { get; set; }
        public string? EndDate { get; set; } 

        public int GetValidPage() => Page < 1 ? 1 : Page;
        public int GetValidPageSize() => PageSize < 1 ? 10 : (PageSize > 100 ? 100 : PageSize);
        public int GetSkip() => (GetValidPage() - 1) * GetValidPageSize();
    }
}