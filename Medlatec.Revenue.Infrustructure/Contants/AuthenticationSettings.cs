namespace Medlatec.Revenue.Infrustructure.Contants
{
    public class AuthenticationSettings
    {
        public string PublicOrigin { get; set; }
        public string Authority { get; set; }
        public string ClientId { get; set; }
        public string ApiName { get; set; }
        public string ApiSecret { get; set; }
        public bool RequireHttpsMetadata { get; set; }
    }
}
