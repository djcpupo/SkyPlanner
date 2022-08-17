using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkyPlanner.Entities;
using System.Collections.Generic;
using System.Linq;

namespace SkyPlanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Account
        [HttpGet]
        public IEnumerable<Account> Get()
        {
            var accounts = new List<Account>();
            accounts = _context.Account
                .Include("Contacts")
                .OrderBy(a => a.Name)
                .ToList();
            return accounts;
        }

        // GET api/Account/5
        [HttpGet("{id}")]
        public Account Get(int id)
        {
            var account = new Account();
            account = _context.Account
                .Include("Contacts")
                .FirstOrDefault(acc => acc.AccountId == id);
            return account;
        }

        // GET api/Account/5/Orders
        [HttpGet("{id}/Orders")]
        public IEnumerable<Order> GetOrders(int id)
        {
            var orders = new List<Order>();
            orders = _context.Order
                .Include("OrderLineItem")
                .Include("OrderLineItem.Product")
                .OrderByDescending(o => o.CreatedDate)
                .Where(acc => acc.AccountId == id)
                .ToList();
            return orders;
        }
    }
}
