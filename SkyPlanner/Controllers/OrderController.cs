using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkyPlanner.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace SkyPlanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Order
        [HttpGet]
        public IEnumerable<Order> Get()
        {
            var orders = new List<Order>();
            orders = _context.Order
                .Include("OrderLineItem")
                .Include("OrderLineItem.Product")
                .Include("Account")
                .ToList();
            return orders;
        }

        // GET api/Order/5
        [HttpGet("{id}")]
        public Order Get(int id)
        {
            var order = new Order();
            order = _context.Order
                .Include("OrderLineItem")
                .Include("OrderLineItem.Product")
                .Include("Account")
                .FirstOrDefault(pro => pro.OrderId == id);
            return order;
        }

        // POST api/Order
        [HttpPost]
        public ActionResult Post([FromBody] Order request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new BadRequestObjectResult(ModelState);
                }
                var account = _context.Account.FirstOrDefault(acc => acc.AccountId == request.AccountId);
                if (account != null && request.OrderLineItem.Any())
                {
                    var subtotal = request.OrderLineItem.Sum(li => li.Quantity * li.UnitPrice);
                    var tax = subtotal * 7 / 100;
                    var total = subtotal + tax;
                    if (subtotal == request.Subtotal && tax == request.Tax && total == request.Total)
                    {
                        var order = new Order
                        {
                            AccountId = request.AccountId,
                            CreatedDate = DateTime.Now,
                            OrderId = request.OrderId,
                            Subtotal = request.Subtotal,
                            Tax = request.Tax,
                            Total = request.Total,
                            OrderLineItem = new List<OrderLineItem>()
                        };
                        foreach (var item in request.OrderLineItem)
                        {
                            order.OrderLineItem.Add(new OrderLineItem
                            {
                                OrderId = order.OrderId,
                                Quantity = item.Quantity,
                                UnitPrice = item.UnitPrice,
                                ProductId = item.ProductId
                            });
                        }
                        _context.Order.Add(order);
                        _context.SaveChanges();
                        return new OkObjectResult(order);
                    }
                }
                return new BadRequestResult();
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }

        // DELETE api/Order/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var order = new Order();
                order = _context.Order
                    .Include("OrderLineItem")
                    .FirstOrDefault(pro => pro.OrderId == id);
                if (order == null)
                    return NotFound(id);
                foreach (var item in order.OrderLineItem)
                {
                    _context.OrderLineItem.Remove(item);
                }
                _context.Order.Remove(order);
                _context.SaveChanges();
                return new OkResult();
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }
    }
}
