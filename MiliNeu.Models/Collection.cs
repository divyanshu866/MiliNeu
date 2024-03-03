using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace MiliNeu.Models
{
    public class Collection
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Collection Name")]
        public string Name { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string Description { get; set; }

        public decimal Price { get; set; }

        [ValidateNever]
        public string Path { get; set; }

        //Nav Prop
        public List<Product>? Products { get; set; }

        public Collection()
        {

        }

        /* //Return all collections and products within them
         public async Task<List<Collection>> GetCollections(ApplicationDbContext _context)
         {
             List<Collection> collections = new List<Collection>();
             collections = await _context.Collection.Include(c => c.Products).ToListAsync();
             return collections;
         }

         //Return all collections 
         public async Task<List<Collection>> GetCollectionsOnly(ApplicationDbContext _context)
         {
             List<Collection> collections = new List<Collection>();
             collections = await _context.Collection.ToListAsync();
             return collections;
         }*/

    }
}
