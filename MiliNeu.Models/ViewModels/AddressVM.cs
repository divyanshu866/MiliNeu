namespace MiliNeu.Models.ViewModels
{
    public class AddressVM
    {
        public IEnumerable<Address>? Addresses { get; set; }
        public ShippingAddress SelectedAddressSnapshot { get; set; }
        public int? SelectedAddressId { get; set; }
        public Address? NewAddress { get; set; }
        public bool SaveAddress { get; set; } = false;
    }
}
