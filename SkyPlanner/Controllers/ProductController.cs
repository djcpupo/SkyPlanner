using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SkyPlanner.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public ActionResult Get(int id)
        {
            var product = new Product();
            product = _context.Product
                .FirstOrDefault(pro => pro.ProductId == id);
            if (product == null)
                return NotFound(id);
            return new OkObjectResult(product);
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
            products = name != "-*-" ? _context.Product
                .Where(pro => pro.Name.ToLower().Contains(name.ToLower()))
                .OrderBy(p => p.Name)
                .ToList() : _context.Product
                .OrderBy(p => p.Name)
                .ToList();
            return products;
        }

        /// <summary>
        /// Add a new Product.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/Product
        ///     {        
        ///       "name": "Name",
        ///       "Phone": "PhoneNumber",
        ///       "Price": 100
        ///     }
        /// </remarks>
        /// <returns>The new Product.</returns>
        // POST api/Product
        [HttpPost]
        [Produces("application/json")]
        public ActionResult Post([FromBody] Product request) 
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new BadRequestObjectResult(ModelState);
                }
                var product = _context.Product.FirstOrDefault(p=>p.Name.ToLower()==request.Name.ToLower());
                if (product != null)
                    return new BadRequestResult();

                product = new Product
                {
                    Name = request.Name,
                    Phone = request.Phone,
                    Price = request.Price
                };
                _context.Product.Add(product);
                _context.SaveChanges();
                return new OkObjectResult(product);
            }
            catch (System.Exception)
            {
                return new BadRequestResult();
            }
        }

        /// <summary>
        /// Delete the Product by Id.
        /// </summary>
        /// <returns>Action Result</returns>
        // DELETE api/Product/5
        [HttpDelete("{id}")]
        [Produces("application/json")]
        public ActionResult Delete(int id) 
        {
            try
            {
                var product = new Product();
                product = _context.Product
                    .FirstOrDefault(pro => pro.ProductId == id);
                if (product == null)
                    return NotFound(id);
                var hasOrders = _context.OrderLineItem.Any(o => o.ProductId == id);
                // Do not delete the product if has at least one order created
                if(hasOrders)
                    return BadRequest(id);
                _context.Product.Remove(product);
                _context.SaveChanges();
                return new OkResult();
            }
            catch (System.Exception)
            {
                return new BadRequestResult();
            }
        }
    }
}
