namespace MiliNeu.Models.ViewModels
{
    public class PagerVM<T>
    {
        public IEnumerable<T> Items { get; set; } // The paginated list of items
        public int CurrentPage { get; set; } // Current page number

        public int TotalPages { get; set; } // Total pages

    }
}
