using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BloggingApis.Models.Domain
{
    public class BlogCategory
    {
        public int Id { get; set; }
        [Required]
        public string CategoryName { get; set; }
        public int? ParentCategory_Id { get; set; }

        [NotMapped]
        public string ParentCategoryName { get; set; }

    }
}
