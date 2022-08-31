using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloggingApis.Models.DTO
{
    public class GetAllBlogCategoryParams
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public string Term { get; set; }
    }
}
