using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TalkToApi.V1.Models.DTO
{
    public class TokenDTO
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpirationRefreshToken { get; set; }
    }
}
