using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CareerConnect.Models;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CareerConnect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]  // Apply authorization to the entire controller
    public class ProjectsController : ControllerBase
    {
        private readonly JobPortalContext _context;

        public ProjectsController(JobPortalContext context)
        {
            _context = context;
        }

        // GET: api/projects/GetAll
        [HttpGet("GetAll")]
        [Authorize(Roles = "Admin")]  // Only Admins can get all projects
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {
            return await _context.Projects.ToListAsync();
        }

        // GET: api/projects/GetById/{id}
        [HttpGet("GetById/{id}")]
        [Authorize(Roles = "Admin,JobSeeker")]  // Admins can view any project, JobSeekers can view their own projects
        public async Task<ActionResult<Project>> GetProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            // Check if the current user is allowed to access this project
            var currentUser = User.Identity.Name; // Assuming the username is stored in the Name claim
            if (User.IsInRole("JobSeeker") && project.JobSeeker.User.Email != currentUser)
            {
                return Forbid(); // Forbid access if the job seeker is trying to access another user's project
            }

            return project;
        }

        // GET: api/projects/GetByJobSeeker/{jobSeekerId}
        [HttpGet("GetByJobSeeker/{jobSeekerId}")]
        [Authorize(Roles = "Admin,JobSeeker")]  // Admins can view any job seeker's projects, JobSeekers can view their own projects
        public async Task<ActionResult<IEnumerable<Project>>> GetProjectsByJobSeekerId(int jobSeekerId)
        {
            var projects = await _context.Projects
                .Where(p => p.JobSeekerId == jobSeekerId)
                .ToListAsync();

            if (projects == null || !projects.Any())
            {
                return NotFound();
            }

            // Check if the current user is allowed to access these projects
            var currentUser = User.Identity.Name;
            var jobSeeker = await _context.JobSeekers.FindAsync(jobSeekerId);
            if (User.IsInRole("JobSeeker") && jobSeeker.User.Email != currentUser)
            {
                return Forbid(); // Forbid access if the job seeker is trying to access another user's projects
            }

            return Ok(projects);
        }

        // POST: api/projects/Create
        [HttpPost("Create")]
        [Authorize(Roles = "JobSeeker")]  // Only JobSeekers can create projects for themselves
        public async Task<ActionResult<Project>> CreateProject(Project project)
        {
            // Ensure the current user is the owner of the project they are creating
            var currentUser = User.Identity.Name;
            var jobSeeker = await _context.JobSeekers.FindAsync(project.JobSeekerId);
            if (jobSeeker.User.Email != currentUser)
            {
                return Forbid(); // Forbid access if the job seeker is trying to create a project for another user
            }

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProject", new { id = project.ProjectId }, project);
        }

        // PUT: api/projects/Update/{id}
        [HttpPut("Update/{id}")]
        [Authorize(Roles = "JobSeeker")]  // Only JobSeekers can update their own projects
        public async Task<IActionResult> UpdateProject(int id, Project project)
        {
            if (id != project.ProjectId)
            {
                return BadRequest();
            }

            // Ensure the current user is the owner of the project they are updating
            var currentUser = User.Identity.Name;
            var existingProject = await _context.Projects.FindAsync(id);
            if (existingProject.JobSeeker.User.Email != currentUser)
            {
                return Forbid(); // Forbid access if the job seeker is trying to update another user's project
            }

            _context.Entry(existingProject).State = EntityState.Modified;

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
        [Authorize(Roles = "JobSeeker,Admin")]  // JobSeekers can delete their own projects, Admins can delete any project
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            // Ensure the current user is the owner of the project they are deleting
            var currentUser = User.Identity.Name;
            if (User.IsInRole("JobSeeker") && project.JobSeeker.User.Email != currentUser)
            {
                return Forbid(); // Forbid access if the job seeker is trying to delete another user's project
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
