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
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly APIContext _context;

        public CategoryController(APIContext context)
        {
            _context = context;
        }

        //Get /api/category
        [HttpGet]
        [Route("api/[controller]/[action]")]
        public async Task<ActionResult<IEnumerable<Category>>> GetAllCategories()
        {
            return await _context.Categories.ToListAsync();
        }   


        [HttpPut]
        [Route("api/[controller]/[action]")]
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

        [HttpGet]
        [Route("api/[controller]/[action]")]
        public async Task<ActionResult<Category>> GetCategoryById(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            return Ok(category);
        }

        [HttpPost]
        [Route("api/[controller]/[action]")]
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

        [HttpDelete]
        [Route("api/[controller]/[action]")]
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
