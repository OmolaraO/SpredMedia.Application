using SpredMedia.CommonLibrary;


namespace SpredMedia.Authentication.Model.model
{
    public class Client : BaseModel
    {
        public string ClientPassword { get; set; }
        public string ClientUsername { get; set; }
        public string ClientSecret { get; set; }
        public string CompanyName { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyPhone { get; set; }
        public IEnumerable<EndPoint> Endpoints { get; set; }
    }
}
