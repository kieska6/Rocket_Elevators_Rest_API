using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestfulApi.Models;
// using System.Data.Entity;

namespace RestfulApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InterventionsController : ControllerBase
    {
        private readonly InterventionContext _context;

        public InterventionsController(InterventionContext context)
        {
            _context = context;
        }

        // GET: api/Interventions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Intervention>>> GetInterventions()
        {
            return await _context.interventions.Where(intervention => intervention.started_At == null && intervention.status == "Pending").ToListAsync();
        }
       

        // GET: api/Interventions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Intervention>> GetIntervention(long id)
        {
            var intervention = await _context.interventions.FindAsync(id);

            if (intervention == null)
            {
                return NotFound();
            }

            return intervention;
        }
         // GET: api/Interventions/status
        [HttpGet("status")]
        public async Task<List<Intervention>> GetListIntervention(string status)
        {
            // var offlinestatus= new[] {"Offline", "Intervention"};
            var intervention = await _context.interventions.Where(x => x.started_At == null || x.status == "Pending" ).ToListAsync();
            return intervention;
        }
        


        // PUT: api/Interventions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkId=2123754
        [HttpPut("{id}/{status}")]
        public async Task<IActionResult> CompletedInterventions([FromRoute] long Id, [FromRoute] string status)
        {
            // lets get our intervention before update
            var update = await _context.interventions.FindAsync(Id);
            // verify the status from the route and handle the response
            if (status == "InProgress")
            {

                // verify if the Id in the route is the same in the Body
                if (update == null)
                {
                    return Content("Wrong Id or records dosen't exist ! please check and try again");
                }
                // verify the status if is already modified 
                if (update.status == "InProgress")
                {
                    return Content("The actual status of the intervention is already InProgress since :" + update.started_At + "! ");
                }
                else if (update.status == "Completed")
                {
                    return Content("The actual status of the intervention is Completed ! End date (UTC):  " + update.finish_At);
                }
                else if (update.status == "Pending")
                {
                    // update date and status
                    update.started_At = DateTime.UtcNow;
                    update.status = "InProgress";
                }

                else
                {
                    // send message to help the user
                    return Content("Please insert a valId status the request adress : InProgress and Tray again please !  ");
                }
                // update and save
                _context.interventions.Update(update);
                await _context.SaveChangesAsync();
                // confirmation message
                return Content("Intervention: " + update.Id + ", status has been changed to: " + update.status);
            }
            // 2 case completed

            else if (status == "completed")
            {
                // verify if the Id in the route is the same in the Body
                if (update == null)
                {
                    return Content("Wrong Id or records dosen't exist ! please check and try again");
                }
                // verify the status if is already modified 
                if (update.status == "Completed")
                {
                    return Content("The actual status of the intervention is already Completed ! End date:  " + update.finish_At);
                }
                if (update.status == "Pending")
                {
                    return Content("The actual intervention is not started yet: Pending ! please try InProgress to start the intervention");
                }
                else if (update.status == "InProgress")
                {
                    // update date and status
                    update.finish_At = DateTime.UtcNow;
                    update.status = "Completed";
                }

                else
                {
                    // send message to help the user
                    return Content("Please insert a valId status the request adress : completed and Tray again please !  ");
                }
                // update and save
                _context.interventions.Update(update);
                await _context.SaveChangesAsync();
                // confirmation message
                return Content("Intervention: " + update.Id + ", status has been changed to: " + update.status);
            }

            // help the user to specify the endpoints
            else return Content("something wrong! please use this route format: [api/Intervention/{Id}/InProgress] or  [api/Intervention/{Id}/completed]");


        }

        // POST: api/Interventions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkId=2123754
        [HttpPost]
        public async Task<ActionResult<Intervention>> PostIntervention(Intervention intervention)
        {
            _context.interventions.Add(intervention);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetIntervention), new { Id = intervention.Id }, intervention);
        }

        // DELETE: api/Interventions/5
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteIntervention(long Id)
        {
            var intervention = await _context.interventions.FindAsync(Id);
            if (intervention == null)
            {
                return NotFound();
            }

            _context.interventions.Remove(intervention);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InterventionExists(long Id)
        {
            return _context.interventions.Any(e => e.Id == Id);
        }
    }
}
