namespace SpredMedia.Authentication.Model.model
{
    public class ClientEndpoint
    {
        public string ClientID { get; set; }
        public Client client { get; set; }

        public string EndPointId { get; set; }
        public EndPoint EndPoint { get; set; }
    }
}
