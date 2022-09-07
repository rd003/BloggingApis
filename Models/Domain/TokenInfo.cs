using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloggingApis.Models.Domain
{
    public class TokenInfo
    {
        public int  Id { get; set; }
        public string UserName { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
