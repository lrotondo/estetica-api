namespace dentist_panel_api.DTOs
{
    public class BaseFilter
    {
        public int Page { get; set; } = 0;
        public int PerPage { get; set; } = 10;

        public Sort Sort { get; set; }
    }

    public class Sort
    {
        public string Field { get; set; } = "createdAt";
        public bool IsAscending { get; set; } = true;
    }
}
