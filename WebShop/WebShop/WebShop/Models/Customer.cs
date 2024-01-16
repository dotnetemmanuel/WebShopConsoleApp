namespace WebShop.Models
{
    internal class Customer

    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Zipcode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }

        //Relation between Customer and Payment and between Customer and Order
        public Customer()
        {

            Orders = new HashSet<Order>();
        }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
