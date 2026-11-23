using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CA_2.Data;
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
        public async Task<ActionResult<IEnumerable<PlayerQuestProgress>>> GetPlayerQuestProgress()
        {
            return await _context.PlayerQuestProgress.ToListAsync();
        }

        // GET: api/Progress/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PlayerQuestProgress>> GetPlayerQuestProgress(int id)
        {
            var playerQuestProgress = await _context.PlayerQuestProgress.FindAsync(id);

            if (playerQuestProgress == null)
            {
                return NotFound();
            }

            return playerQuestProgress;
        }

        // PUT: api/Progress/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlayerQuestProgress(int id, PlayerQuestProgress playerQuestProgress)
        {
            if (id != playerQuestProgress.Id)
            {
                return BadRequest();
            }

            _context.Entry(playerQuestProgress).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayerQuestProgressExists(id))
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
        public async Task<ActionResult<PlayerQuestProgress>> PostPlayerQuestProgress(PlayerQuestProgress playerQuestProgress)
        {
            _context.PlayerQuestProgress.Add(playerQuestProgress);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPlayerQuestProgress", new { id = playerQuestProgress.Id }, playerQuestProgress);
        }

        // DELETE: api/Progress/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayerQuestProgress(int id)
        {
            var playerQuestProgress = await _context.PlayerQuestProgress.FindAsync(id);
            if (playerQuestProgress == null)
            {
                return NotFound();
            }

            _context.PlayerQuestProgress.Remove(playerQuestProgress);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PlayerQuestProgressExists(int id)
        {
            return _context.PlayerQuestProgress.Any(e => e.Id == id);
        }
    }
}
