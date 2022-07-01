using PayPal.Api;

namespace Knjizara.Models.PayPal
{
    public class Configuration
    {
        public readonly static string ClientId;
        public readonly static string ClientSecret;

        static Configuration()
        {
            var config = GetConfig();
            ClientId = config["clientId"];
            ClientSecret = config["clientSecret"];
        }

        // getting properties from the web.config
        public static Dictionary<string, string> GetConfig()
        {
            return new Dictionary<string, string>
            {
                { "clientId", "ARJrffa4Hd3tk6x_PE6i8WD8rdr2DfvPO2GnSAuxF2AomSkEPn30nW2fKtQ6Atm5MGh4bYo-Es451-2S" },
                { "clientSecret", "EDQV49lVACxgUahmjql8jcry6clrczhOnN1UjBnvJ8ya2U-ivnRVHogPxKf6E5dtUWny0sp0NtoJ2jBQ" }
            };
        }

        private static string GetAccessToken()
        {               
            string accessToken = new OAuthTokenCredential
        (ClientId, ClientSecret, GetConfig()).GetAccessToken();

            return accessToken;
        }

        public static APIContext GetAPIContext()
        {
            // return apicontext object by invoking it with the accesstoken
            APIContext apiContext = new APIContext(GetAccessToken());
            apiContext.Config = GetConfig();
            return apiContext;
        }
    }
}
