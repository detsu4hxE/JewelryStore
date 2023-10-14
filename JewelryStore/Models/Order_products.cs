namespace JewelryStore.Models
{
    public class Order_products
    {
        public int Id { get; set; }
        public int OrdersID { get; set; }
        public int ProductsID { get; set; }
        public int amount { get; set; }
        public double price { get; set; }
        public Orders Orders { get; set; }
        public Products Products { get; set; }
    }
}
