using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApI.Models;
using ApI.Models.Data;
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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> Get()
        {
            return await _context.Categories.OrderBy(x => x.Id).ToListAsync();
        }
    }
}
