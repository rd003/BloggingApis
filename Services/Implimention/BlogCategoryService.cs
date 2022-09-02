using BloggingApis.Common;
using BloggingApis.Models.Domain;
using BloggingApis.Models.DTO;
using BloggingApis.Services.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
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
        public async Task<PagedList<BlogCategory>> GetAll(GetAllBlogCategoryParams model)
        {
            //const int maxPageSize = 50;
            //if (model.PageSize > maxPageSize)
            //    model.PageSize = maxPageSize;
            
            if (!string.IsNullOrEmpty(model.Term))
                model.Term = model.Term.ToLower();
            var categories =  (from blogCategory in context.BlogCategory
                                             join
                     parentBlogCategory in context.BlogCategory
                     on blogCategory.ParentCategory_Id equals parentBlogCategory.Id into blog_parent
                                             from parentBlogData in blog_parent.DefaultIfEmpty()
                                             where string.IsNullOrWhiteSpace(model.Term) || blogCategory.CategoryName.ToLower().StartsWith(model.Term)
                                             select new BlogCategory
                                             {
                                                 CategoryName = blogCategory.CategoryName,
                                                 Id = blogCategory.Id,
                                                 ParentCategoryName = parentBlogData.CategoryName,
                                                 ParentCategory_Id = blogCategory.ParentCategory_Id
              
                                             }).AsQueryable();
            ApplySort(ref categories, model.OrderBy);
            var pagedList = await PagedList<BlogCategory>.ToPagedList(categories, model.PageNo, model.PageSize);
            return pagedList;
        }

        private void ApplySort(ref IQueryable<BlogCategory> records, string orderByQueryString)
        {
            if (!records.Any())
                return;
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                records = records.OrderBy(x => x.Id);
                return;
            }
            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(BlogCategory).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var orderQueryBuilder = new StringBuilder();
            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                    continue;
                var propertyFromQueryName = param.Split(" ")[0];
                var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));
                if (objectProperty == null)
                    continue;
                var sortingOrder = param.EndsWith(" desc") ? "descending" : "ascending";
                orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {sortingOrder}, ");
            }
            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');
            if (string.IsNullOrWhiteSpace(orderQuery))
            {
                records = records.OrderBy(x => x.Id);
                return;
            }
            records = records.OrderBy(orderQuery);
        }
    }
}
