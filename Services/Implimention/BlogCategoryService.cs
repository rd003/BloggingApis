using BloggingApis.Common;
using BloggingApis.Common.Service;
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
        private readonly ISortHelper<BlogCategory> _sortHelper;
        public BlogCategoryService(DatabaseContext context, ISortHelper<BlogCategory> sortHelper)
        {
            this.context = context;
            this._sortHelper = sortHelper;
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
        public async Task<IQueryable<BlogCategory>> GetBlogCategories(string term="")
        {
            var categories = await (from blogCategory in context.BlogCategory
                              join
      parentBlogCategory in context.BlogCategory
      on blogCategory.ParentCategory_Id equals parentBlogCategory.Id into blog_parent
                              from parentBlogData in blog_parent.DefaultIfEmpty()
                              where string.IsNullOrWhiteSpace(term) || blogCategory.CategoryName.ToLower().StartsWith(term)
                              select new BlogCategory
                              {
                                  CategoryName = blogCategory.CategoryName,
                                  Id = blogCategory.Id,
                                  ParentCategoryName = parentBlogData.CategoryName,
                                  ParentCategory_Id = blogCategory.ParentCategory_Id

                              }).ToListAsync();
            return categories.AsQueryable();
        }
        public async Task<PagedList<BlogCategory>> GetAll(GetAllBlogCategoryParams model)
        {
            if (!string.IsNullOrEmpty(model.Term))
                model.Term = model.Term.ToLower();
            var categories = await this.GetBlogCategories(model.Term);
            var sortedCategories=this._sortHelper.ApplySort(categories, model.OrderBy);
            var pagedList = await PagedList<BlogCategory>.ToPagedList(sortedCategories, model.PageNo, model.PageSize);
            return pagedList;
        }

    }
}
