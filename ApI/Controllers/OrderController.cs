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
    public class OrderController : ControllerBase
    {
        private readonly APIContext _context;

        public OrderController(APIContext context)
        {
            _context = context;
        }

        //Get /api/order
        [HttpGet]
        [Route("api/[controller]/[action]")]
        public async Task<ActionResult<IEnumerable<Order>>> Get()
        {
            return await _context.Orders.OrderBy(x => x.Id).ToListAsync();
        }

        //Post /api/orders
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Authorize]
        public async Task<ActionResult<Order>> Create([FromForm] OrderDTO order)
        {
            var orderToCreate = new Order
            {
                Name = order.Name,
                Email = order.Email,
                Address = order.Address,
                Cart = order.Cart,
                Total = order.Total
            };
            _context.Orders.Add(orderToCreate);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Create), order);

        }
    }
}
