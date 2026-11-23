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
    public class ProgressController : ControllerBase
    {
        private readonly EftContext _context;

        public ProgressController(EftContext context)
        {
            _context = context;
        }

        // GET: api/Progress
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Quest>>> GetEftItems()
        {
            return await _context.EftItems.ToListAsync();
        }

        // GET: api/Progress/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Quest>> GetEftItem(int id)
        {
            var eftItem = await _context.EftItems.FindAsync(id);

            if (eftItem == null)
            {
                return NotFound();
            }

            return eftItem;
        }

        // PUT: api/Progress/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEftItem(int id, Quest quest)
        {
            if (id != quest.Id)
            {
                return BadRequest();
            }

            _context.Entry(quest).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EftItemExists(id))
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

        // POST: api/Progress
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Quest>> PostEftItem(Quest quest)
        {
            _context.EftItems.Add(quest);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEftItem", new { id = quest.Id }, quest);
        }

        // DELETE: api/Progress/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEftItem(int id)
        {
            var eftItem = await _context.EftItems.FindAsync(id);
            if (eftItem == null)
            {
                return NotFound();
            }

            _context.EftItems.Remove(eftItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EftItemExists(int id)
        {
            return _context.EftItems.Any(e => e.Id == id);
        }
    }
}
