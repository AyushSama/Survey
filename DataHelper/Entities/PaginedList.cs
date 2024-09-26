namespace DataHelper.Entities
{
    public class PaginedList<T>
    {
        public int TotalItems { get; set; }
        public List<T> Items { get; set; }
    }
}
