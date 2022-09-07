using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloggingApis.Models.DTO
{
    public class RefreshTokenRequestModel
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
