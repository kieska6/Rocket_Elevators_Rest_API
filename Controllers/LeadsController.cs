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
    public class LeadsController : ControllerBase
    {
        private readonly LeadContext _context;
        private readonly CustomerContext _cusContext;

        public LeadsController(LeadContext context,CustomerContext cusContext)
        {
            _context = context;
            _cusContext = cusContext;
        }

        // GET: api/Leads
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lead>>> GetLeads()
        {
            return await _context.leads.ToListAsync();
        }

        // GET: api/Leads/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Lead>> GetLead(long id)
        {
            var lead = await _context.leads.FindAsync(id);

            if (lead == null)
            {
                return NotFound();
            }

            return lead;
        }

                // GET: api/Leads/customerleads
        [HttpGet("CustomerLeads")]
        public async Task<ActionResult<IEnumerable<Lead>>> RecentLeads()
        {   
            var customerlist = await _cusContext.customers.ToListAsync();
            List<string> customeremaillist = new List<string>();

            foreach (Customer customer in customerlist)
            {
                customeremaillist.Add(customer.company_email);
            }

            var leadlist = await _context.leads.ToListAsync();
            List<Lead> RecentLeadsList = new List<Lead>();

            foreach (Lead lead in leadlist)
            {
                DateTime now = DateTime.Now;
                TimeSpan elapsed = now.Subtract(lead.created_at);
                int daysAgo = (int)elapsed.TotalDays;

                if (daysAgo < 30)
                {
                    // check if email is not part of company_email, if so =>
                    if (!customeremaillist.Contains(lead.email))
                    {
                    RecentLeadsList.Add(lead);
                    }
                } 
            }
            return RecentLeadsList;
        }


        // PUT: api/Leads/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLead(long id, Lead lead)
        {
            if (id != lead.Id)
            {
                return BadRequest();
            }

            _context.Entry(lead).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeadExists(id))
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

        // POST: api/Leads
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Lead>> PostLead(Lead lead)
        {
            _context.leads.Add(lead);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLead), new { id = lead.Id }, lead);
        }

        // DELETE: api/Leads/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLead(long id)
        {
            var lead = await _context.leads.FindAsync(id);
            if (lead == null)
            {
                return NotFound();
            }

            _context.leads.Remove(lead);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LeadExists(long id)
        {
            return _context.leads.Any(e => e.Id == id);
        }
    }
}
