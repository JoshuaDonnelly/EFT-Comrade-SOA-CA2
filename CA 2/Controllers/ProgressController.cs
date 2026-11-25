using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CA_2.Data;
using CA_2.Models;
using CA_2.Repositories;
using Microsoft.AspNetCore.Authorization;
using NuGet.Protocol.Core.Types;

namespace CA_2.Controllers
{
    [Authorize]
    [Route("api/me/quests")]
    [ApiController]
    public class ProgressController(IQuestProgressRepository repository) : ControllerBase
    {
 
        // GET: api/me/quests/0
        [HttpGet("{questId:int}")]
        public async Task<IActionResult> GetProgress(int questId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId is null)
                return Unauthorized();

            var progress = await repository.GetAsync(userId, questId);

            if (progress is null)
                return NotFound();

            return Ok(new
            {
                progress.QuestId,
                QuestTitle = progress.Quest.Title,
                progress.Status,
                progress.CompletedAt
            });
        }
       
        // POST: api/Quests/Progress
        // Method to read quests and retrieve player progress
        [HttpPost("{questId:int}")]
        public async Task<IActionResult> StartQuest(int questId)
        {
            // Get the authenticated User's ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized();
            
            // Check for existing progress on a quest
            var existingProgress = await repository.GetAsync(userId, questId);

            if (existingProgress is not null)
                return Conflict("Quest is already tracked");
            
            // Verify the quest exists
            if (!await repository.QuestExistsAsync(questId))
                return NotFound("Quest does not exist.");
            
            // Create the progress record
            var progress = new PlayerQuestProgress
            {
                UserId = userId,
                QuestId = questId,
                Status = "NotStarted"
            };
            
            await repository.AddAsync(progress);

            return CreatedAtAction(
                nameof(GetProgress),
                new { questId },
                new
                {
                    progress.QuestId,
                    progress.Status,
                    progress.CompletedAt
                });
        }
        

        
    }
}
