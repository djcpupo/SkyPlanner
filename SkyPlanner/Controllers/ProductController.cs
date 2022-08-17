using Microsoft.AspNetCore.Mvc;
using SkyPlanner.Entities;
using System.Collections.Generic;
using System.Linq;

namespace SkyPlanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Product
        [HttpGet]
        public IEnumerable<Product> Get()
        {
            var products = new List<Product>();
            products = _context.Product
                .ToList();
            return products;
        }

        // GET api/Product/5
        [HttpGet("{id}")]
        public Product Get(int id)
        {
            var product = new Product();
            product = _context.Product
                .FirstOrDefault(pro => pro.ProductId == id);
            return product;
        }
    }
}
