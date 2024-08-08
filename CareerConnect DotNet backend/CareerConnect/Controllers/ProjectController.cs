using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CareerConnect.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CareerConnect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly JobPortalContext _context;

        public ProjectsController(JobPortalContext context)
        {
            _context = context;
        }

        // GET: api/projects/GetAll
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {
            return await _context.Projects.ToListAsync();
        }

        // GET: api/projects/GetById/{id}
        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            return project;
        }

        // GET: api/projects/GetByJobSeeker/{jobSeekerId}
        [HttpGet("GetByJobSeeker/{jobSeekerId}")]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjectsByJobSeekerId(int jobSeekerId)
        {
            var projects = await _context.Projects
                .Where(p => p.JobSeekerId == jobSeekerId)
                .ToListAsync();

            if (projects == null || !projects.Any())
            {
                return NotFound();
            }

            return Ok(projects);
        }

        // POST: api/projects/Create
        [HttpPost("Create")]
        public async Task<ActionResult<Project>> CreateProject(Project project)
        {
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProject", new { id = project.ProjectId }, project);
        }

        // PUT: api/projects/Update/{id}
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateProject(int id, Project project)
        {
            if (id != project.ProjectId)
            {
                return BadRequest();
            }

            
            _context.Entry(project).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
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

        // DELETE: api/projects/Delete/{id}
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.ProjectId == id);
        }
    }
}
