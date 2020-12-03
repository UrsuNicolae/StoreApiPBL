using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class PagesController : ControllerBase
    {
        private readonly APIContext _context;

        public PagesController(APIContext context)
        {
            _context = context;
        }

        //Get /api/pages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Page>>> Get()
        {
            return await _context.Pages.OrderBy(x => x.Id).ToListAsync();
        }

        //Post /api/pages
        [HttpPost]
        public async Task<ActionResult<Page>> Create(Page page)
        {
            await _context.Pages.AddAsync(page);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Create), page);
        }

        //Post /api/pages/s
        [HttpPut]
        public async Task<ActionResult<Page>> Update(int id, Page page)
        {
            if(id != page.Id)
            {
                return BadRequest();
            }

            _context.Entry(page).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        //Delete /api/pages
        [HttpDelete("{id}")]
        public async Task<ActionResult<Page>> Delete(int id)
        {
            var page = await _context.Pages.FirstOrDefaultAsync(x => x.Id == id);

            if (page == null) return BadRequest();
            _context.Pages.Remove(page);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
