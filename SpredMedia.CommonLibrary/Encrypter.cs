using EasyEncryption;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.Net;


namespace SpredMedia.CommonLibrary
{
    public static class Encrypter
    {
        public static string Encode(string body, HttpContext context, ILogger _logger, IConfiguration configuration)
        {
            try
            {
                string password = context.Request.Headers["password"];
                string username = context.Request.Headers["username"];
                var isAuthenticModel = ConnectTODb(password, username, _logger, configuration);
                if (isAuthenticModel.IsSuccessful)
                {
                    _logger.Information("the user Exist ins the database administrator");
                    _logger.Information("about to decrpty the ClientSecret from the database using the AES algorithm");
                    _logger.Information("generating the IV and Key for decrypting the body parameter");
                    string DecryptSecretIV = Encryption.GenerateSHA256(username).Substring(0,16);
                    string DecryptSecretKey = Encryption.GenerateSHA256(password).Substring(0, 32);
                    isAuthenticModel.ClientSecret = Encryption.DecryptData(isAuthenticModel.ClientSecret, DecryptSecretKey, DecryptSecretIV);
                    _logger.Information("the user secret was decrypted successfully");
                    _logger.Information("generating the IV and Key for encrypt the body parameter");
                    string EncryptBodyIV = Encryption.GenerateSHA256(isAuthenticModel.ClientSecret).Substring(0, 16);
                    string EncryptIVKey = Encryption.GenerateSHA256(isAuthenticModel.ClientID).Substring(0, 32);
                    return Encryption.EncryptData(body, EncryptIVKey, EncryptBodyIV);
                }
                return "";

            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                _logger.Information(ex.StackTrace);
                return "";
            }
        }

        static Client ConnectTODb(string password, string username, ILogger _logger, IConfiguration _configuration)
        {
            string connectionString = _configuration.GetValue<string>("AuthenticationDb");
            _logger.Information("the connectionString is " + connectionString);
            _logger.Information("about to make a request to the database to get client instanced");
            var hashedPassword = SHA.ComputeSHA256Hash(password).ToString();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var responseData = new Client();
                connection.Open();

                string selectClientIdQuery = "SELECT Id,ClientSecret FROM Clients  WHERE ClientPassword = '" + hashedPassword + "' AND ClientUsername = '" + username + "'";

                using (SqlCommand firstcommand = new SqlCommand(selectClientIdQuery, connection))
                {
                    using (SqlDataReader firstReader = firstcommand.ExecuteReader())
                    {
                        while (firstReader.Read())
                        {
                            responseData.ClientID = firstReader.GetString(0);
                            responseData.ClientSecret = firstReader.GetString(1);
                            responseData.IsSuccessful = true;
                        }
                        firstReader.Close();
                    }
                }
                return responseData;
            }

        }
    }
    class Client
    {
        public string ClientID { get; set; }
        public string ClientSecret { get; set; }
        public bool IsSuccessful { get; set; }
    }
}
