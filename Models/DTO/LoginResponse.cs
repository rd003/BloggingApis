using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloggingApis.Models.DTO
{
    public class LoginResponse:Status
    {
        public string Token { get; set; }
        public DateTime? Expiration { get; set; }
        
    }
}
