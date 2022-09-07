using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloggingApis.Models
{
    public class TokenResponse
    {
        public string TokenString { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
