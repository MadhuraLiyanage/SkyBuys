    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SkyBuys.Models
{
    public class SkyBuysLoginResults
    {
        [JsonPropertyName("success")]
        public string Success { get; set; }
        [JsonPropertyName("data")]
        public Data Data { get; set; }

    }

    public class Data
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("loginToken")]
        public string LoginToken { get; set; }
        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; }
        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }
    }
}
