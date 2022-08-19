using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SkyPlanner.Entities;

namespace SkyPlanner.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Order.Include(o => o.Account);
            return View(await applicationDbContext.ToListAsync());
        }
        

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["AccountId"] = new SelectList(_context.Account, "AccountId", "AccountId");
            return View();
        }        
    }
}
