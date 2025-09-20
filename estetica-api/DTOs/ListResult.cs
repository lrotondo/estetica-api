namespace dentist_panel_api.DTOs
{
    public class ListResult<T>
    {
        public ListResult(int total, List<T> items)
        {
            Total = total;
            Items = items;
        }

        public int Total { get; set; }
        public List<T> Items { get; set; }
    }
}
