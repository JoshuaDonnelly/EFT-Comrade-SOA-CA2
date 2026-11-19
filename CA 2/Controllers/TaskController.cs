using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CA_2.Models;

namespace CA_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly EftContext _context;

        public TaskController(EftContext context)
        {
            _context = context;
        }

        // GET: api/Task
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EftItemTasks>>> GetEftItemTasks()
        {
            return await _context.EftItemTasks.ToListAsync();
        }

        // GET: api/Task/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EftItemTasks>> GetEftItemTasks(int id)
        {
            var eftItemTasks = await _context.EftItemTasks.FindAsync(id);

            if (eftItemTasks == null)
            {
                return NotFound();
            }

            return eftItemTasks;
        }

        // PUT: api/Task/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEftItemTasks(int id, EftItemTasks eftItemTasks)
        {
            if (id != eftItemTasks.id)
            {
                return BadRequest();
            }

            _context.Entry(eftItemTasks).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EftItemTasksExists(id))
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

        // POST: api/Task
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EftItemTasks>> PostEftItemTasks(EftItemTasks eftItemTasks)
        {
            _context.EftItemTasks.Add(eftItemTasks);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEftItemTasks), new { id = eftItemTasks.id }, eftItemTasks);
        }

        // DELETE: api/Task/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEftItemTasks(int id)
        {
            var eftItemTasks = await _context.EftItemTasks.FindAsync(id);
            if (eftItemTasks == null)
            {
                return NotFound();
            }

            _context.EftItemTasks.Remove(eftItemTasks);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EftItemTasksExists(int id)
        {
            return _context.EftItemTasks.Any(e => e.id == id);
        }
    }
}
