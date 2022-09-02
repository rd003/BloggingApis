using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloggingApis.Models.DTO
{
    public class GetAllBlogCategoryParams: QueryStringParameter
    {
        public GetAllBlogCategoryParams()
        {
            OrderBy = "Id";
        }
    }
}
