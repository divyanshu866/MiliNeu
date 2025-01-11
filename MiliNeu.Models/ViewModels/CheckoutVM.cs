namespace MiliNeu.Models.ViewModels
{
    public class CheckoutVM
    {
        //Customer
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        //Shipping
        /* public IEnumerable<Address>? SavedAddresses { get; set; }
         [Required]
         public int? SelectedAddressId { get; set; }*/
        public AddressVM AddressVM { get; set; }
        /*   public string Appartment { get; set; }
           public string City { get; set; }
           public string State { get; set; }
           public string PinCode { get; set; }
           public string StreetAddress { get; set; }
           public string Country { get; set; }*/
        public string EstimatedDelivery { get; set; }
        public List<CartItem> CartItems { get; set; }

        //Payment
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
        public decimal Discount { get; set; }
        public decimal Shipping { get; set; }

        public string CustomerNotes { get; set; }

    }
}
