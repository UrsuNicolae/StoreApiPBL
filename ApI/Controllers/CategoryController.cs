using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApI.DTOs;
using ApI.Models;
using ApI.Models.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly APIContext _context;

        public CategoryController(APIContext context)
        {
            _context = context;
        }

        //Get /api/category
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<Category>>> Get()
        {
            return await _context.Categories.OrderBy(x => x.Id).ToListAsync();
        }

        [HttpPut("create")]
        [Authorize]
        public async Task<ActionResult<int>> CreateCategory(CategoryDTO model)
        {
            var category = new Category
            {
                Name = model.Name,
                Slug = model.Slug
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return Ok(new { Id = category.Id });

        }

        [HttpGet("get/id")]
        public async Task<ActionResult<Category>> GetCategoryById(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            return Ok(category);
        }

        [HttpPost("update")]
        public async Task<ActionResult> UpdateCategory(CategoryUpdateDTO model)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == model.Id);
            if(category != null)
            {
                category.Name = model.Name;
                category.Slug = model.Slug;

                _context.Categories.Update(category);

                await _context.SaveChangesAsync();
            }

            return NoContent();
        }

        [HttpDelete("delete")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if(category != null)
            {
                _context.Categories.Remove(category);

                await _context.SaveChangesAsync();
            }

            return NoContent();
        }
    }
}
