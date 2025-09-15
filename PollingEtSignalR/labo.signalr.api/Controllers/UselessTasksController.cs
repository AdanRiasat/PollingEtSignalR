using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using labo.signalr.api.Data;
using labo.signalr.api.Models;

namespace labo.signalr.api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UselessTasksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UselessTasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UselessTask>>> GetAll()
        {
            return await _context.UselessTasks.ToListAsync();
        }

        [HttpPost("{taskText}")]
        public async Task<ActionResult<UselessTask>> Add(string taskText)
        {
            UselessTask uselessTask = new UselessTask() {
                Completed = false,
                Text = taskText
            };
            _context.UselessTasks.Add(uselessTask);
            await _context.SaveChangesAsync();

            return Ok(uselessTask);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Complete(int id)
        {
            UselessTask? task = await _context.FindAsync<UselessTask>(id);
            if (task != null)
            {
                task.Completed = true;
                await _context.SaveChangesAsync();
                return NoContent();
            }
            return NotFound();
        }
    }
}
