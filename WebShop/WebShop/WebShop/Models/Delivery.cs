namespace WebShop.Models
{
    internal class Delivery
    {
        public int Id { get; set; }
        public string DeliveryMethod { get; set; }
        public int Price { get; set; }

        public Delivery()
        {
            Orders = new HashSet<Order>();
        }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
