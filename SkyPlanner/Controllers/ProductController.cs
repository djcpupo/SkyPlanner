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
        ///       "Phone": "7863052365",
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

                if (request.Phone.Length > 10)
                    return BadRequest("The Phone Number should be 10 digits");
                if (request.Name.Length > 200)
                    return BadRequest("The Name should be 200 or less characters");
                var product = _context.Product.FirstOrDefault(p => p.Name.ToLower() == request.Name.ToLower());
                if (product != null)
                    return BadRequest("There is another product with this Name: " + request.Name);

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
                return new BadRequestObjectResult("An error has occurred, please try again or contact the support team.");
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
                    return NotFound("There is no product with this ID: " + id.ToString());
                var hasOrders = _context.OrderLineItem.Any(o => o.ProductId == id);
                // Do not delete the product if has at least one order created
                if (hasOrders)
                    return BadRequest("This product has related orders and can't be deleted. Product ID: " + id);
                _context.Product.Remove(product);
                _context.SaveChanges();
                return new OkResult();
            }
            catch (System.Exception)
            {
                return new BadRequestObjectResult("An error has occurred, please try again or contact the support team.");
            }
        }
    }
}
