namespace WebShop.Models
{
    internal class Payment
    {
        public int Id { get; set; }
        public string PaymentMethod { get; set; }

        public Payment()
        {

            Orders = new HashSet<Order>();
        }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
