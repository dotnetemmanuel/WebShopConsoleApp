namespace WebShop.Models
{
    internal class Category
    {

        public int Id { get; set; }
        public string Name { get; set; }

        //Relation between Category and Product
        public Category()
        {
            Products = new HashSet<Product>();
        }
        public virtual ICollection<Product> Products { get; set; }
    }
}
