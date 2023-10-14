namespace JewelryStore.Models
{
    public class Users
    {
        public int Id { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public int RolesId { get; set; }
        public string surname { get; set; }
        public string firstname { get; set; }
        public string? patronymic { get; set; }
        public string email { get; set; }
        public string? phone { get; set; }

        public Roles Roles { get; set; }
    }
}
