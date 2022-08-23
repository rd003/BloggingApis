using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BloggingApis.Models.Domain;
using BloggingApis.Models.DTO;
using BloggingApis.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BloggingApis.Controllers
{
    [Route("api/[controller]/{action}")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class BlogCategoryController : ControllerBase
    {
        private readonly IBlogCategoryService blogCategoryService;
        public BlogCategoryController(IBlogCategoryService blogCategoryService)
        {
            this.blogCategoryService = blogCategoryService;
        }

        [HttpPost]
        public async Task<IActionResult> AddUpdate(BlogCategory model)
        {
            var status = new Status();
            if (!ModelState.IsValid)
            {
                status.StatusCode = 0;
                status.Message = "Please pass valid and complete data";
                return Ok(status);
            }
            var result = await blogCategoryService.AddUpdate(model);
            status.StatusCode = result ? 1 : 0;
            status.Message = result ? "Data has added/updated successfully" : "Data could not saved";
            return Ok(status);
        }

        public async Task<IActionResult> GetAll()
        {
            var data = await blogCategoryService.GetAll();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await blogCategoryService.GetById(id);
            return Ok(data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await blogCategoryService.Delete(id);
            var status = new Status
            {
                StatusCode = data ? 1 : 0,
                Message = data ? "Deleted" : "Error"
            };
            return Ok(status);
        }

        
    }
}
