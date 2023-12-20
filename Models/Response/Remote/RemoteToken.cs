using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VendingMachine.Models.Response.Remote
{
    public class RemoteToken
    {
        [JsonPropertyName("userId")]
        public string UserId { get; set; }

        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; }

        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }

        internal static RemoteToken? fromToken(Token? token)
        {
            if (token == null)
            {
                return null;
            }

            return new RemoteToken()
            {
                UserId = token.UserId,
                AccessToken = token.AccessToken,
                RefreshToken = token.RefreshToken,
            };
        }
    }
}
