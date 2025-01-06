using MiliNeu.Models.ViewModels;

namespace MiliNeu.Models.Services.Interfaces
{
    public interface ICollectionService
    {
        public Task<PagerVM<Collection>> getCollectionsPageAsync(int pageNumber, int pageSize);
        public Task<IEnumerable<Collection>> getAllCollectionsAsync();


    }
}
