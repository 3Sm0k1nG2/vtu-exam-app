namespace AppRestaurant.Models
{
    public class AddressModel
    {
        public AddressModel(string street, string city, string postCode)
        {
            Street = street;
            City = city;
            PostCode = postCode;
        }
        public AddressModel(int id, string street, string city, string postCode, string phone)
        {
            Id = id;
            Street = street;
            City = city;
            PostCode = postCode;
            Phone = phone;
        }

        public int Id { get; set; }
        public string Phone { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
    }
}
