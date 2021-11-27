
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
    public class BuildingsController : ControllerBase
    {
        private readonly BuildingContext _context;

        private readonly BatteryContext _batContext;
        private readonly ColumnContext _colContext;
        private readonly ElevatorContext _eleContext;

        public BuildingsController(BuildingContext context, BatteryContext batContext,ColumnContext colContext,ElevatorContext eleContext)
        {
            _context = context;
            _batContext = batContext;
            _colContext = colContext;
            _eleContext = eleContext;
        }

        // GET: api/Buildings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Building>>> GetBuildings()
        {
            return await _context.buildings.ToListAsync();
        }

        // GET: api/Buildings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Building>> GetBuilding(long id)
        {
            var building = await _context.buildings.FindAsync(id);

            if (building == null)
            {
                return NotFound();
            }

            return building;
        }
        //  GET: api/Buildings/BuildingIntervention
        [HttpGet("BuildingIntervention")]
        public async Task<List<Building>> GetListBuilding(long id)
        {
            List<Building> BuildingInterventions = new List<Building>();

            var buildingsList = await _context.buildings.ToListAsync();

            foreach(Building building in buildingsList)
            {   
                var batterylist = await _batContext.batteries.ToListAsync();

                foreach(Battery battery in batterylist)
                {
                    if (battery.building_id == building.Id && !BuildingInterventions.Contains(building))
                    { 
                        if (battery.status == "Intervention")
                        {
                            BuildingInterventions.Add(building);     
                        } else
                            {
                                var columnlist = await _colContext.columns.ToListAsync();

                                foreach(Column column in columnlist)
                                {
                                    if (column.battery_id == battery.Id && !BuildingInterventions.Contains(building))
                                    {
                                        if (column.status == "Intervention")
                                        {
                                            BuildingInterventions.Add(building);
                                        } else 
                                            {
                                                var elevatorlist = await _eleContext.elevators.ToListAsync();

                                                foreach(Elevator elevator in elevatorlist)
                                                {
                                                    if (elevator.column_id == battery.Id && !BuildingInterventions.Contains(building))
                                                    {
                                                        if (elevator.status == "Intervention")
                                                        {
                                                            BuildingInterventions.Add(building);
                                                        }
                                                    }
                                                }
                                            }                                                        
                                    } 
                                }
                            }
                    }
                }
            }
            return BuildingInterventions;
        }

        // PUT: api/Buildings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBuilding(long id, Building building)
        {
            if (id != building.Id)
            {
                return BadRequest();
            }

            _context.Entry(building).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BuildingExists(id))
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

        // POST: api/Buildings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Building>> PostBuilding(Building building)
        {
            _context.buildings.Add(building);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBuilding), new { id = building.Id }, building);
        }

        // DELETE: api/Buildings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBuilding(long id)
        {
            var building = await _context.buildings.FindAsync(id);
            if (building == null)
            {
                return NotFound();
            }

            _context.buildings.Remove(building);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BuildingExists(long id)
        {
            return _context.buildings.Any(e => e.Id == id);
        }
    }
}
