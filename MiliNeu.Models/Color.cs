using System.ComponentModel.DataAnnotations;

namespace MiliNeu.Models
{
    public class Color
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string HexCode { get; set; }

    }
}
