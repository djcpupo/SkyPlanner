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


        /// <summary>
        /// Gets the Product List.
        /// </summary>
        /// <returns>The Product list.</returns>
        // GET: api/Product
        [HttpGet]
        [Produces("application/json")]
        public IEnumerable<Product> Get()
        {
            var products = new List<Product>();
            products = _context.Product
                .OrderBy(p => p.Name)
                .ToList();
            return products;
        }


        /// <summary>
        /// Gets the Product by Id.
        /// </summary>
        /// <returns>The Product.</returns>
        // GET api/Product/5
        [HttpGet("{id}")]
        [Produces("application/json")]
        public Product Get(int id)
        {
            var product = new Product();
            product = _context.Product
                .FirstOrDefault(pro => pro.ProductId == id);
            return product;
        }


        /// <summary>
        /// Filter the Product List by name.
        /// </summary>
        /// <returns>The Product list.</returns>
        // GET api/Product/5
        [HttpGet("{name}/Search")]
        [Produces("application/json")]
        public IEnumerable<Product> GetByName(string name)
        {
            var products = new List<Product>();
            products = _context.Product
                .Where(pro => pro.Name.ToLower().Contains(name.ToLower()))
                .OrderBy(p => p.Name)
                .ToList();
            return products;
        }
    }
}
