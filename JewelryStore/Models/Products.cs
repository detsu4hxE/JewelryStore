namespace JewelryStore.Models
{
    public class Products
    {
        public int Id { get; set; }
        public string name { get; set; }
        public int Product_typesID { get; set; }
        public int MaterialsID { get; set; }
        public double price { get; set; }
        public double weight { get; set; }
        public string? image { get; set; }
        public Product_types Product_Types { get; set; }
        public Materials Materials { get; set; }
    }
}
