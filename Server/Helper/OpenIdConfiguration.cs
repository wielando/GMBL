using System.Text.Json.Serialization;

namespace GMBL.Server.Helper
{
    public class OpenIdConfiguration
    {
        [JsonPropertyName("authorization_endpoint")]
        public string AuthorizationEndpoint { get; set; }

        [JsonPropertyName("token_endpoint")]
        public string TokenEndpoint { get; set; }

        [JsonPropertyName("userinfo_endpoint")]
        public string UserinfoEndpoint { get; set; }

        // Weitere Eigenschaften können hier hinzugefügt werden, je nachdem, welche Konfigurationsdetails Sie benötigen

        // Beispiel:
        // [JsonPropertyName("jwks_uri")]
        // public string JwksUri { get; set; }
    }

}
