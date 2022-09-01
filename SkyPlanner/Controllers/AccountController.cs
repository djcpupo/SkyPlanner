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

        /// <summary>
        /// Gets the Account List.
        /// </summary>
        /// <returns>The Account list.</returns>
        // GET: api/Account
        [HttpGet]
        [Produces("application/json")]
        public IEnumerable<Account> Get()
        {
            var accounts = new List<Account>();
            accounts = _context.Account
                .Include("Contact")
                .OrderBy(a => a.Name)
                .ToList();
            return accounts;
        }

        /// <summary>
        /// Gets the Account by Id.
        /// </summary>
        /// <returns>The Account.</returns>
        // GET: api/Account/5
        [HttpGet("{id}")]
        [Produces("application/json")]
        public ActionResult Get(int id)
        {
            var account = new Account();
            account = _context.Account
                .Include("Contact")
                .FirstOrDefault(acc => acc.AccountId == id);
            if (account == null)
                return NotFound(id);
            return new OkObjectResult(account);
        }

        /// <summary>
        /// Gets the Account by a search term.
        /// </summary>
        /// <returns>The Accounts</returns>
        // GET: api/Account/5
        [HttpGet("{name}/Autocomplete")]
        [Produces("application/json")]
        public ActionResult GetAutocomplete(string name)
        {
            var accounts = _context.Account
                .Where(acc => acc.Name.ToLower().Contains(name.ToLower()))
                .Select(acc => acc.Name);
            return new OkObjectResult(accounts);
        }

        /// <summary>
        /// Gets the list of all orders by Account.
        /// </summary>
        /// <returns>The list of all orders.</returns>
        // GET: api/Account/5/Orders
        [HttpGet("{id}/Orders")]
        [Produces("application/json")]
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

        /// <summary>
        /// Add a new Account.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/Account
        ///     {        
        ///       "Name": "Name",
        ///       "Phone": "7863052365",
        ///       "Street": "Street",
        ///       "City": "City",
        ///       "State": "State",
        ///       "Zip": "Zip"
        ///     }
        /// </remarks>
        /// <returns>The new Account.</returns>
        // POST api/Account
        [HttpPost]
        [Produces("application/json")]
        public ActionResult Post([FromBody] Account request)
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
                var account = _context.Account.FirstOrDefault(p => p.Name.ToLower() == request.Name.ToLower());
                if (account != null)
                    return BadRequest("There is another account with this Name: " + request.Name);

                account = new Account
                {
                    Name = request.Name,
                    Phone = request.Phone,
                    Street = request.Street,
                    City = request.City,
                    State = request.State,
                    Zip = request.Zip,
                    Contact = new List<Contact>(),
                    Order = new List<Order>()
                };
                _context.Account.Add(account);
                _context.SaveChanges();
                return new OkObjectResult(account);

            }
            catch (System.Exception)
            {
                return new BadRequestObjectResult("An error has occurred, please try again or contact the support team.");
            }
        }

        /// <summary>
        /// Edit the Account.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/Account/2
        ///     {        
        ///       "Name": "Name",
        ///       "Phone": "7863052365",
        ///       "Street": "Street",
        ///       "City": "City",
        ///       "State": "State",
        ///       "Zip": "Zip"
        ///     }
        /// </remarks>
        /// <returns>The Account.</returns>
        // POST api/Account
        [HttpPut("{id}")]
        [Produces("application/json")]
        public ActionResult Put([FromBody] Account request, int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new BadRequestObjectResult(ModelState);
                }
                var tempAccount = _context.Account.FirstOrDefault(a => a.AccountId == id);
                if (tempAccount == null)
                    return NotFound("There is not accounts with this Id: " + id.ToString());
                if (request.Phone.Length > 10)
                    return BadRequest("The Phone Number should be 10 digits");
                if (request.Name.Length > 200)
                    return BadRequest("The Name should be 200 or less characters");
                var account = _context.Account.FirstOrDefault(p => p.Name.ToLower() == request.Name.ToLower() && p.AccountId != id);
                if (account != null)
                    return BadRequest("There is another account with this Name: " + request.Name);

                tempAccount.Name = request.Name;
                tempAccount.Phone = request.Phone;
                tempAccount.Street = request.Street;
                tempAccount.City = request.City;
                tempAccount.State = request.State;
                tempAccount.Zip = request.Zip;
                tempAccount.Contact = new List<Contact>();
                tempAccount.Order = new List<Order>();

                _context.Account.Update(tempAccount);
                _context.SaveChanges();
                return new OkObjectResult(tempAccount);

            }
            catch (System.Exception)
            {
                return new BadRequestObjectResult("An error has occurred, please try again or contact the support team.");
            }
        }

        /// <summary>
        /// Add a new Contact.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/Account/1/Contact
        ///     {        
        ///       "Name": "Name",
        ///       "Phone": "7863052365"
        ///     }
        /// </remarks>
        /// <returns>The new Contact.</returns>
        // POST api/Account/1/Contact
        [HttpPost("{id}/Contact")]
        [Produces("application/json")]
        public ActionResult PostContact([FromBody] Contact request, int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new BadRequestObjectResult(ModelState);
                }

                var account = _context.Account.FirstOrDefault(p => p.AccountId == id);
                if (account == null)
                    return NotFound("There is no account with this ID: " + id.ToString());

                var contact = _context.Contact.FirstOrDefault(c => c.Name.ToLower() == request.Name.ToLower() && c.AccountId == account.AccountId);
                if (contact != null)
                    return BadRequest("There is another contact in this account with this Name: " + request.Name);

                if(request.Phone.Length > 10)
                    return BadRequest("The Phone Number should be 10 digits");

                if (request.Name.Length > 200)
                    return BadRequest("The Name should be 200 or less characters");

                contact = new Contact
                {
                    Name = request.Name,
                    Phone = request.Phone,
                    AccountId = id
                };
                _context.Contact.Add(contact);
                _context.SaveChanges();
                return new OkObjectResult(contact);

            }
            catch (System.Exception)
            {
                return new BadRequestObjectResult("An error has occurred, please try again or contact the support team.");
            }
        }

        /// <summary>
        /// Edit Contact.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/Account/1/Contact
        ///     {        
        ///       "Name": "Name",
        ///       "Phone": "7863052365",
        ///       "AccountId": 1,
        ///       "ContactId": 1
        ///     }
        /// </remarks>
        /// <returns>The Contact.</returns>
        // PUT api/Account/1/Contact
        [HttpPut("{id}/Contact")]
        [Produces("application/json")]
        public ActionResult PutContact([FromBody] Contact request, int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new BadRequestObjectResult(ModelState);
                }

                var tempContact = _context.Contact.FirstOrDefault(p => p.ContactId == id);
                if (tempContact == null)
                    return NotFound("There is no contact with this ID: " + id.ToString());

                var contact = _context.Contact.FirstOrDefault(c => c.Name.ToLower() == request.Name.ToLower() && c.AccountId == tempContact.AccountId && c.ContactId != id);
                if (contact != null)
                    return BadRequest("There is another contact in this account with this Name: " + request.Name);

                if (request.Phone.Length > 10)
                    return BadRequest("The Phone Number should be 10 digits");

                if (request.Name.Length > 200)
                    return BadRequest("The Name should be 200 or less characters");

                tempContact.Name = request.Name;
                tempContact.Phone = request.Phone;

                _context.Contact.Update(tempContact);
                _context.SaveChanges();
                return new OkObjectResult(tempContact);

            }
            catch (System.Exception)
            {
                return new BadRequestObjectResult("An error has occurred, please try again or contact the support team.");
            }
        }

        /// <summary>
        /// Get the Contact by Id.
        /// </summary>
        /// <returns>Action Result</returns>
        // Get api/Account/5/Contact
        [HttpGet("{id}/Contact")]
        [Produces("application/json")]
        public ActionResult GetContact(int id)
        {
            try
            {
                var contact = new Contact();
                contact = _context.Contact
                    .FirstOrDefault(pro => pro.ContactId == id);
                if (contact == null)
                    return NotFound("There is no contact with this ID: " + id.ToString());
                return new OkObjectResult(contact);
            }
            catch (System.Exception)
            {
                return new BadRequestObjectResult("An error has occurred, please try again or contact the support team.");
            }
        }

        /// <summary>
        /// Delete the Contact by Id.
        /// </summary>
        /// <returns>Action Result</returns>
        // DELETE api/Account/5/Contact
        [HttpDelete("{id}/Contact")]
        [Produces("application/json")]
        public ActionResult DeleteContact(int id)
        {
            try
            {
                var contact = new Contact();
                contact = _context.Contact
                    .FirstOrDefault(pro => pro.ContactId == id);
                if (contact == null)
                    return NotFound("There is no contact with this ID: " + id.ToString());
                _context.Contact.Remove(contact);
                _context.SaveChanges();
                return new OkResult();
            }
            catch (System.Exception)
            {
                return new BadRequestObjectResult("An error has occurred, please try again or contact the support team.");
            }
        }

        /// <summary>
        /// Delete the Account by Id.
        /// </summary>
        /// <returns>Action Result</returns>
        // DELETE api/Account/5
        [HttpDelete("{id}")]
        [Produces("application/json")]
        public ActionResult Delete(int id)
        {
            try
            {
                var account = new Account();
                account = _context.Account
                    .Include("Contact")
                    .FirstOrDefault(pro => pro.AccountId == id);
                if (account == null)
                    return NotFound("There is no account with this ID: " + id.ToString());
                var hasOrders = _context.Order.Any(o => o.AccountId == id);
                // Do not delete the Account if has at least one order created
                if (hasOrders)
                    return BadRequest("This account has related orders and can't be deleted. Account ID: " + id);
                foreach (var item in account.Contact)
                {
                    _context.Contact.Remove(item);
                }
                _context.Account.Remove(account);
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
