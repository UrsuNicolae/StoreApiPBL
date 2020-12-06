using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApI.DTOs;
using ApI.Models;
using ApI.Models.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly APIContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(APIContext context,
            IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        //Get /api/products
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Product>>> Get(int p = 1)
        {
            int pageSize = 4;
            var products = _context.Products.OrderBy(x => x.Id)
                    .Include(x => x.Category).Skip((p - 1) * pageSize)
                    .Take(pageSize);

            return await products.ToListAsync();
        }

        //Get /api/products/category
        [HttpGet("{slug}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetByCategory(string slug, int p = 1)
        {
            var categoryFromDb = await _context.Categories.FirstOrDefaultAsync(x => x.Slug == slug);
            if (categoryFromDb == null)
                return NotFound();

            int pageSize = 4;
            var products = _context.Products.OrderBy(x => x.Id)
                    .Where(x => x.CategoryId == categoryFromDb.Id).Skip((p - 1) * pageSize)
                    .Take(pageSize);

            return await products.ToListAsync();
        }

        //Get /api/products/count/category
        [HttpGet("count/{slug}")]
        public async Task<ActionResult<int>> GetProductCount(string slug)
        {
            if (slug == "all")
                return await _context.Products.CountAsync();
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Slug == slug);
            return await _context.Products.Where(x => x.CategoryId == category.Id).CountAsync();
        }

        //Get /api/products/GetById/id
        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<Product>> GetById(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
        }

        //POST /api/products
        [HttpPost("create")]
        [Authorize]
        public async Task<ActionResult<Product>> Create([FromForm] ProductDTO product)
        {
            string imageName = "noImage.png";
            if(product.ImageUpload != null)
            {
                string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media\\products");
                imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                string filePath = Path.Combine(uploadsDir, imageName);
                FileStream fs = new FileStream(filePath, FileMode.Create);
                await product.ImageUpload.CopyToAsync(fs);
                fs.Close();
            }

            var productToCreate = new Product
            {
                Image = imageName,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId
            };
            _context.Products.Add(productToCreate);
            await _context.SaveChangesAsync();

            return Ok();
        }

        //Put /api/products
        [HttpPut("update")]
        [Authorize]
        public async Task<ActionResult<Product>> Update ([FromForm] Product product)
        {
            if (product.ImageUpload != null)
            {
                string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/ products");

                var currentImage = (from p in _context.Products
                                    where p.Id == product.Id
                                    select p.Image).Single();

                if (!string.Equals(currentImage, "noimage.png"))
                {
                    string oldImagePath = Path.Combine(uploadsDir, currentImage);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                var imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                string filePath = Path.Combine(uploadsDir, imageName);
                FileStream fs = new FileStream(filePath, FileMode.Create);
                await product.ImageUpload.CopyToAsync(fs);
                fs.Close();
                product.Image = imageName;
            }
            _context.Entry(product).State = EntityState.Modified;
            if(product.ImageUpload == null)
            {
                _context.Entry(product).Property("Image").IsModified = false;
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        //Delete /api/products/delete/id
        [HttpDelete("delete/{id}")]
        [Authorize]
        public async Task<ActionResult<Product>> Delete(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

            if(!string.Equals(product.Image, "noimage.png"))
            {
                string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath,
                    "media/products");
                string oldImagePath = Path.Combine(uploadsDir, product.Image);
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
