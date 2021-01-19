using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ApI.DTOs;
using ApI.Models;
using ApI.Models.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartController : ControllerBase
    {
        private readonly APIContext _context;

        public ChartController(APIContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route(nameof(CreateOrder))]
        public async Task<IActionResult> CreateOrder(CreateOrderDto model)
        {
            var chart = await _context.Charts.FirstOrDefaultAsync(x => x.Id == model.ChartId || x.UserId == model.UserId);
            if (chart == null)
            {
                chart = new Chart();
                chart.UserId = model.UserId;

                _context.Charts.Add(chart);

                await _context.SaveChangesAsync();
            }

            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == model.ProductId);
            if (product == null)
                return BadRequest("product not found");
            var orderFromDb = await _context.Orders.FirstOrDefaultAsync(x => x.ProductId == model.ProductId);
            if (orderFromDb == null)
            {
                var order = new Order
                {
                    UserId = model.UserId,
                    ChartId = chart.Id,
                    ProductId = model.ProductId,
                    Quantity = model.Quantity
                };
                _context.Orders.Add(order);
            }
            else
            {
                orderFromDb.Quantity += model.Quantity;
                _context.Orders.Update(orderFromDb);
            }
            chart.Total += model.Quantity * product.Price;
            _context.Charts.Update(chart);
            await _context.SaveChangesAsync();

            return Ok();
        }


        [HttpPost]
        [Route("api/[controller]/[action]")]
        public async Task<IActionResult> UpdateOrder(Order model)
        {
            var cart = await _context.Charts.FirstOrDefaultAsync(x => x.Id == model.ChartId);

            if (cart == null)
                return BadRequest("cart not found!");
            var orderFromDb = await _context.Orders.Include(i => i.Product).FirstOrDefaultAsync(x => x.Id == model.Id);
            if (orderFromDb == null)
                return BadRequest("Order not found!");

            orderFromDb.Quantity = model.Quantity;
            cart.Total = model.Quantity * orderFromDb.Product.Price;

            _context.Orders.Update(orderFromDb);
            _context.Charts.Update(cart);
            await _context.SaveChangesAsync();
            
            return Ok();
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var orderFromDb = await _context.Orders.Include(i => i.Product).FirstOrDefaultAsync(x => x.Id == id);
            if (orderFromDb == null)
                return BadRequest("Order not found!");
            var chart = await _context.Charts.FirstOrDefaultAsync(x => x.Id == orderFromDb.ChartId);
            if (chart == null)
                return BadRequest();

            chart.Total -= orderFromDb.Quantity * orderFromDb.Product.Price;
            if (chart.Total < 0)
                chart.Total = 0;
            _context.Charts.Update(chart);

            _context.Orders.Remove(orderFromDb);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet]
        [Route("{userId}")]
        public async Task<IActionResult> GetChartNumbers(int userId)
        {
            var chart = await _context.Charts.Include(i => i.Orders).FirstOrDefaultAsync(x => x.UserId == userId);
            if(chart == null)
            {
                return Ok(0);
            }
            return Ok(chart.Orders.Count());
        }

        [HttpGet]
        [Route("chart/{userId}")]
        public async Task<IActionResult>GetUserCart(int userId)
        {
            var chart = await _context.Charts.Include(i => i.Orders)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(x => x.UserId == userId);
            if (chart == null)
                return BadRequest("Your chart is emty!");
            return Ok(chart);
        }
    }
}
