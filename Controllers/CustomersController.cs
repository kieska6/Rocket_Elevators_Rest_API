using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestfulApi.Models;
namespace RestfulApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly CustomerContext _context;
        public CustomersController(CustomerContext context)
        {
            _context = context;
        }
        // // GET: api/Customers
        // [HttpGet]
        // public async Task<ActionResult<IEnumerable<Customer>>> GetCustomer()
        // {
        //     return await _context.customers.ToListAsync();
        // }
        // GET: api/Customers/emailValidation
        [HttpGet("emailValidation")]
        public async Task<ActionResult<bool>> EmailsConfirmation(string company_email)
        {
            var customerList = await _context.customers.ToListAsync();
            foreach (Customer customer in customerList)
            {          
                if (customer.company_email == company_email)
                {
                    return true;
                }
            }
            return false;
        }
        // GET: api/Customers/5
        [HttpGet("{company_email}")]
        public async Task<ActionResult<Customer>> GetCustomer(string company_email)
        {
            var customer = await _context.customers.Where(x => x.company_email == company_email).ToListAsync();
            if (customer == null)
            {
                return NotFound();
            }
            return customer[0];
        }
      // GET: api/customer/6
        [HttpGet("customerId/{company_email}")]
        public IEnumerable<long> GetCustomerId(string company_email)
        {
            var customer = _context.customers.Where(m => m.company_email == company_email).Select( x => x.Id).ToList();
            return customer;
        }
     // PUT: api/Customers/5
        [HttpPut("InProgresStatus/{id}")]
        public async Task<IActionResult> UpdateCustomer(int id)
        {
            var pendingInt = _context.customers.Find(id);
            if (pendingInt.company_email.Contains("@") )
            {
                return BadRequest();
            }
            else
            {
                pendingInt.company_email = "InProgress";
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }
        // POST: api/Customers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            _context.customers.Add(customer);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer);
        }
        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(long id)
        {
            var customer = await _context.customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            _context.customers.Remove(customer);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        private bool CustomerExists(long id)
        {
            return _context.customers.Any(e => e.Id == id);
        }
    }
}