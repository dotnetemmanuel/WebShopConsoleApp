namespace WebShop.Models
{
    internal class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public decimal Sum { get; set; }
        public DateTime Date { get; set; }
        public int PaymentId { get; set; }
        public int DeliveryId { get; set; }

        ////Relation between Customer and Order
        public virtual Customer? Customer { get; set; }

        public virtual Payment? Payment { get; set; }

        public virtual Delivery? Delivery { get; set; }

        //Relation between Product and Order
        public virtual ICollection<Product> Products { get; set; }
    }
}
