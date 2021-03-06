﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApI.DTOs;
using ApI.Extensions;
using ApI.Helpers;
using ApI.Models;
using ApI.Models.Data;
using ApI.Services;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public ProductsController(APIContext context,
            IWebHostEnvironment webHostEnvironment,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        //Get /api/products
        [HttpGet]
        [Route("api/[controller]/[action]")]
        public async Task<ActionResult<IEnumerable<Product>>> GetPaginatedProducts(int p = 1)
        {
            int pageSize = 4;
            var products = _context.Products.OrderBy(x => x.Id)
                    .Include(x => x.Category).Skip((p - 1) * pageSize)
                    .Take(pageSize);

            return await products.ToListAsync();
        }

        //Get /api/products/category
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsByCategory([FromQuery]ProductParams productParams)
        {
            var products =  _context.Products.AsQueryable();
                
            if(productParams.CategoryId != -1)
            {
                var categoryFromDb = await _context.Categories.FirstOrDefaultAsync(x => x.Id == productParams.CategoryId);
                if (categoryFromDb == null)
                    return NotFound();

                products = products.Where(x => x.CategoryId == productParams.CategoryId);
            }
            var listToReturn = await PagedList<Product>.CreateAsync(products, productParams.PageNumber, productParams.PageSize);
            
            Response.AddPagination(listToReturn.CurrentPage, listToReturn.PageSize,
             listToReturn.TotalCount, listToReturn.TotalPages);

            var productsToReturn = _mapper.Map<IEnumerable<ProductDTO>>(listToReturn);

            return Ok(productsToReturn);
        }

        //Get /api/products/count/category
        [HttpGet]
        [Route("api/[controller]/[action]")]
        public async Task<ActionResult<int>> GetProductCount(string slug)
        {
            if (slug == "all")
                return await _context.Products.CountAsync();
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Slug == slug);
            return await _context.Products.Where(x => x.CategoryId == category.Id).CountAsync();
        }

        [HttpGet]
        [Route("api/[controller]/[action]")]
        public async Task<ActionResult<List<Product>>> GetAllProducts()
        {
            return await _context.Products.ToListAsync();
        }

        //Get /api/products/GetById/id
        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
        }

        //POST /api/products
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Authorize]
        public async Task<ActionResult<Product>> CreateProduct([FromForm] ProductDTO product)
        {
            string imageName = "noImage.png";
            if (product.ImageUpload != null)
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

            return Ok(new { Id = productToCreate.Id });
        }


        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Authorize]
        public async Task<ActionResult<Product>> CreateProductFromBody( ProductDTO product)
        {
            var productToCreate = new Product
            {
                Image = product.Image,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId
            };
            _context.Products.Add(productToCreate);
            await _context.SaveChangesAsync();

            return Ok(new { Id = productToCreate.Id });
        }

        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Authorize]
        public async Task<ActionResult<Product>> CreateListProduct(List<ProductDTO> products)
        {
            foreach(var product in products)
            {
                var productToCreate = new Product
                {
                    Image = product.Image,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    CategoryId = product.CategoryId
                };
                _context.Products.Add(productToCreate);
                await _context.SaveChangesAsync();
            }
            

            return Ok();
        }
        //Put /api/products
        [HttpPut]
        [Route("api/[controller]/[action]")]
        [Authorize]
        public async Task<ActionResult<Product>> UpdateUpdateProduct ([FromForm] Product product)
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
            if (product.ImageUpload == null)
            {
                _context.Entry(product).Property("Image").IsModified = false;
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        //Delete /api/products/delete/id
        [HttpDelete]
        [Route("api/[controller]/[action]")]
        [Authorize]
        public async Task<ActionResult<Product>> Delete(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
