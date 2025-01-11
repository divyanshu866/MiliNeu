namespace MiliNeu.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }  // e.g., "Color", "Size"
        public string Value { get; set; } // e.g., "Red", "M"

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
