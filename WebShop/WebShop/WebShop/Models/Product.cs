namespace WebShop.Models
{
    internal class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string ProductInfo { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public ClothingSize? ClothingSize { get; set; }
        public ShoeSize? ShoeSize { get; set; }
        public string Supplier { get; set; }
        public int Stock { get; set; }
        public bool SelectedProduct { get; set; }

        //relation between product and category
        public virtual Category? Category { get; set; }

        //relation between product and order
        public virtual ICollection<Order> Orders { get; }
    }
}
