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
    public class AccountsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Accounts
        public async Task<IActionResult> Index()
        {
              return View(await _context.Account.ToListAsync());
        }

        // GET: Accounts/Create
        public IActionResult Create()
        {
            return View();
        }

        // GET: Accounts/Edit/{id}
        public IActionResult Edit(string id)
        {
            ViewBag.Id = id;
            return View();
        }

        // GET: Accounts/CreateContact/{id}
        public IActionResult CreateContact(string id)
        {
            ViewBag.Id = id;
            return View();
        }

        // GET: Accounts/EditContact/{id}
        public IActionResult EditContact(int id)
        {
            var contact = _context.Contact.FirstOrDefault(c => c.ContactId == id);
            ViewBag.Id = id.ToString();
            ViewBag.AccountId = contact.AccountId.ToString();
            return View();
        }
    }
}
