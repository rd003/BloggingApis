using BloggingApis.Common;
using BloggingApis.Models.Domain;
using BloggingApis.Models.DTO;
using BloggingApis.Services.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloggingApis.Services.Implimention
{
    public class BlogCategoryService : IBlogCategoryService
    {
        private readonly DatabaseContext context;
        public BlogCategoryService(DatabaseContext context)
        {
            this.context = context;
        }
        public async Task<bool> AddUpdate(BlogCategory category)
        {
            try
            {
                if (category.Id == 0)
                    context.BlogCategory.Add(category);
                else
                    context.BlogCategory.Update(category);
                await context.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var category = await GetById(id);
                if (category == null)
                    return false;
                context.BlogCategory.Remove(category);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<BlogCategory> GetById(int id)
        {
            var category = await context.BlogCategory.FindAsync(id);
            return category;
        }
        public async Task<PagedList<BlogCategory>> GetAll(int pageNo, int pageSize)
        {
            const int maxPageSize = 50;
            if (pageSize > maxPageSize)
                pageSize = maxPageSize;
            var categories =  (from blogCategory in context.BlogCategory
                                             join
                     parentBlogCategory in context.BlogCategory
                     on blogCategory.ParentCategory_Id equals parentBlogCategory.Id into blog_parent
                                             from parentBlogData in blog_parent.DefaultIfEmpty()
                                             select new BlogCategory
                                             {
                                                 CategoryName = blogCategory.CategoryName,
                                                 Id = blogCategory.Id,
                                                 ParentCategoryName = parentBlogData.CategoryName,
                                                 ParentCategory_Id = blogCategory.ParentCategory_Id
                                             }).AsQueryable();
            var pagedList = await PagedList<BlogCategory>.ToPagedList(categories, pageNo, pageSize);
            return pagedList;
        }

       
    }
}
