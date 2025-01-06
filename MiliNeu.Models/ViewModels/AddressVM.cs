namespace MiliNeu.Models.ViewModels
{
    public class AddressVM
    {
        public IEnumerable<Address>? Addresses { get; set; }
        public OrderAddress SelectedAddressSnapshot { get; set; }
        public int? SelectedAddressId { get; set; }
        public Address? NewAddress { get; set; }
        public bool SaveAddress { get; set; } = true;
    }
}
