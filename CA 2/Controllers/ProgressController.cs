using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CA_2.Data;
using CA_2.DTOs;
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
        
        // HTTP PUT: api/me/quests/0
        [HttpPut("{questId:int}")]
        
        // From body converts the request to convert to our DTO
        public async Task<IActionResult> UpdateProgress(int questId, [FromBody] UpdateQuestProgressDto dto)
        {
            // Get the user's id from identity claims
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (userId is null)
                return Unauthorized();
            
            // Define an array of statuses the API accepts
            var validStatuses = new[]
            {
                "NotStarted",
                "InProgress",
                "Completed"
            };
            if (!validStatuses.Contains(dto.status))
            {
                return BadRequest("Invalid status");
            }
            
            // Retrieve quest progress for a specific user
            var progress = await repository.GetAsync(userId, questId);
            
            if (progress is null)
                return NotFound("Quest does not exist.");
            
            // Update the progress, SaveChangesAsync is tracked for changes
            progress.Status =  dto.status;
            
            // Update completion date. If quest is completed store the time, if not remove timestamp
            progress.CompletedAt = dto.status == "Completed" ? DateTime.UtcNow : null;
            
            // Persist the changes
            await repository.UpdateAsync(progress);
            
            // Return 200 with updated progress, create a response object using Dto constructor
            return Ok(new QuestProgressDto(
                progress.QuestId,
                progress.Quest.Title,
                progress.Status,
                progress.CompletedAt
            ));

        }
        
        // HTTP DELETE: api/me/quests/0
        [HttpDelete("{questId:int}")]

        public async Task<IActionResult> DeleteProgress(int questId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (userId is null)
                return Unauthorized();
            
            var progress = await repository.GetAsync(userId, questId);
            
            if (progress is null)
                return NotFound("Quest does not exist.");
            
            // Delete operation on players quest progress
            await repository.DeleteAsync(progress);
            
            // Return No Content, indicates deletion was successful
            return NoContent();
        }
        

        
    }
}
