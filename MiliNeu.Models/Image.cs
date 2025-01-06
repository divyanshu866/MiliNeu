using System.ComponentModel.DataAnnotations;

namespace MiliNeu.Models
{
    public class Image
    {
        [Key]
        public int Id { get; set; }

        public string Path { get; set; }


    }
}
