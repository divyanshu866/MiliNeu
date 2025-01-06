using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MiliNeu.DataAccess.Data;
using MiliNeu.Models.Services.Interfaces;
using MiliNeu.Models.ViewModels;
using PaymentGateway;

namespace MiliNeu.Models.Services.Implementations
{
    public class CollectionService : ICollectionService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IRazorpayPaymentService _razorpayPaymentService;
        private readonly IHttpContextAccessor _httpcontextAccessor;



        public CollectionService(ApplicationDbContext context, IConfiguration configuration, IRazorpayPaymentService razorpayPaymentService, IHttpContextAccessor httpcontextAccessor)
        {
            _context = context;
            _configuration = configuration;
            _razorpayPaymentService = razorpayPaymentService;
            _httpcontextAccessor = httpcontextAccessor;
        }
        public async Task<PagerVM<Collection>> getCollectionsPageAsync(int pageNumber, int pageSize)
        {
            IEnumerable<Collection> collections = _context.Collections
                .IgnoreQueryFilters()
                .Include(i => i.Images)
                .Include(p => p.Products)
                .ThenInclude(c => c.Variants)
                .ThenInclude(i => i.Images).OrderBy(p => p.Name) // You can order by any property
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            var totalCollections = _context.Collections.Count();

            PagerVM<Collection> viewModel = new PagerVM<Collection>
            {
                Items = collections,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling((double)totalCollections / pageSize)
            };

            //CollectionViewModel collectionViewModel = new CollectionViewModel
            //{
            //    AllCollections = collections
            //};

            return viewModel;
        }
        //Displays Collections with crud options
        public async Task<IEnumerable<Collection>> getAllCollectionsAsync()
        {

            return _context.Collections
                .Include(c => c.Products);



            /* return _context.Collections != null ?
                         View(await _context.Collections.Include(c => c.Products).ToListAsync()) :
                         Problem("Entity set 'ApplicationDbContext.Collections'  is null.");*/
        }









    }
}
