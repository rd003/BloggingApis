using BloggingApis.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloggingApis.Services.Abstract
{
    public interface IBlogCategoryService
    {
        public Task<bool> AddUpdate(BlogCategory category);
        public Task<bool> Delete(int id);
        public Task<BlogCategory> GetById(int id);
        public Task<IEnumerable<BlogCategory>> GetAll();
    }
}
