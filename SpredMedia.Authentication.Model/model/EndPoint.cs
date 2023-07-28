using SpredMedia.CommonLibrary;

namespace SpredMedia.Authentication.Model.model
{
    public class EndPoint : BaseModel
    {
        public string ControllerName { get; set; }
        public string Endpoint { get; set; }
        public string Channel { get; set; }
        public string Method { get; set; }
        public IEnumerable<Client> Clients { get; set; }
    }
}
