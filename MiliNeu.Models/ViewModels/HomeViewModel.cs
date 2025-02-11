﻿namespace MiliNeu.Models.ViewModels
{
    public class HomeViewModel
    {
        public List<HeroSection>? HeroSections { get; set; }

        public List<Collection> AllCollections { get; set; }
        public List<Collection> TopCollections { get; set; }
        public IEnumerable<Product> BestSellers { get; set; }
    }
}
