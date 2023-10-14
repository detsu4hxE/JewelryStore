namespace JewelryStore.Models
{
    public class Orders
    {
        public int Id { get; set; }
        public int UsersID { get; set; }
        public double total_price { get; set; }
        public Users Users { get; set; }
    }
}
