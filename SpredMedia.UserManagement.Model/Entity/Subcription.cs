using SpredMedia.CommonLibrary;
namespace SpredMedia.UserManagement.Model.Entity
{
    public class Subscription : BaseModel
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal Duration { get; set; }
        public IEnumerable<User> Users {get; set;}
    }
}
