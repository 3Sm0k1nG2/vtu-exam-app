namespace AppRestaurant.Models.Forms
{
    public class AddressFormDataModel
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string Phone { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
    }
}
