using Microsoft.AspNetCore.Cors;
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
    //[EnableCors("MyAllowSpecificOrigins")]
    public class OrderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }


        /// <summary>
        /// Gets the Order List.
        /// </summary>
        /// <returns>The Order list.</returns>
        // GET: api/Order
        [HttpGet]
        [Produces("application/json")]
        public IEnumerable<Order> Get()
        {
            var orders = new List<Order>();
            orders = _context.Order
                .Include("OrderLineItem")
                .Include("OrderLineItem.Product")
                .Include("Account")
                .OrderByDescending(o => o.CreatedDate)
                .ThenByDescending(o => o.OrderId)
                .ToList();
            return orders;
        }

        /// <summary>
        /// Gets the Order by Id.
        /// </summary>
        /// <returns>The Order.</returns>
        // GET api/Order/5
        [HttpGet("{id}")]
        [Produces("application/json")]
        public ActionResult Get(int id)
        {
            var order = new Order();
            order = _context.Order
                .Include("OrderLineItem")
                .Include("OrderLineItem.Product")
                .Include("Account")
                .FirstOrDefault(pro => pro.OrderId == id);
            if(order == null)
                return NotFound(id);
            return new OkObjectResult(order);
        }


        /// <summary>
        /// Add a new Order.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/Order
        ///     {        
        ///       "orderId": 0,
        ///       "accountId": 1,
        ///       "createdDate": "2022-08-17T15:50:38.230Z",
        ///       "subtotal": 0,
        ///       "tax": 0,
        ///       "total": 0,
        ///       "orderLineItem": [
        ///         {
        ///             "orderLineItemId": 0,
        ///             "orderId": 0,
        ///             "productId": 0,
        ///             "quantity": 0,
        ///             "unitPrice": 0
        ///         }
        ///       ]
        ///     }
        /// </remarks>
        /// <returns>The new Order.</returns>
        // POST api/Order
        [HttpPost]
        [Produces("application/json")]
        public ActionResult Post([FromBody] Order request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new BadRequestObjectResult(ModelState);
                }
                var account = _context.Account.FirstOrDefault(acc => acc.AccountId == request.AccountId);
                if (account == null)
                    return BadRequest("There is no account with this ID: " + request.AccountId.ToString());
                if(!request.OrderLineItem.Any())
                    return BadRequest("You must add at least one product to the order.");
                var subtotal = request.OrderLineItem.Sum(li => li.Quantity * li.UnitPrice);
                decimal tax = decimal.Round((subtotal * (decimal)0.07), 2);
                var total = subtotal + tax;
                if (subtotal != request.Subtotal || tax != request.Tax || total != request.Total)
                    return BadRequest("Please correct your order Subtotal, Taxes or Total amount.");
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
                    var lineItem = _context.Product.Where(p=>p.ProductId==item.ProductId).FirstOrDefault();
                    if (lineItem == null)
                        return BadRequest("There is no product with this ID: " + item.ProductId.ToString());
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
            catch (Exception)
            {
                return new BadRequestObjectResult("An error has occurred, please try again or contact the support team.");
            }
        }


        /// <summary>
        /// Delete the Order by Id.
        /// </summary>
        /// <returns>Action Result</returns>
        // DELETE api/Order/5
        [HttpDelete("{id}")]
        [Produces("application/json")]
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
                return new BadRequestObjectResult("An error has occurred, please try again or contact the support team.");
            }
        }
    }
}
