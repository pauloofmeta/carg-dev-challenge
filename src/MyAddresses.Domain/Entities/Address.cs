namespace MyAddresses.Domain.Entities
{
    public class Address: BaseEntity
    {
        public string ZipCode { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Complement { get; set; }
        public string State { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}