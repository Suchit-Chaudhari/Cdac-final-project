using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CareerConnect.Models;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CareerConnect.DTO;

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
        [Authorize(Roles = "Admin, JobSeeker")]  // Only Admins can get all projects
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {
            return await _context.Projects.ToListAsync();
        }

        // GET: api/projects/GetByJobSeeker/{jobSeekerId}
        // GET: api/projects/GetByJobSeeker/{jobSeekerId}
        [HttpGet("GetByJobSeeker/{jobSeekerId}")]
        [Authorize(Roles = "Admin,JobSeeker")]  // Admins and JobSeekers can view projects
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetProjectsByJobSeekerId(int jobSeekerId)
        {
            var projects = await _context.Projects
                .Where(p => p.JobSeekerId == jobSeekerId)
                .Select(p => new ProjectDto
                {
                    ProjectId = p.ProjectId,
                    ProjectName = p.ProjectName,
                    Description = p.Description, // Include the Description
                    Technologies = p.Technologies, // Include the Technologies
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    JobSeekerId = p.JobSeekerId
                })
                .ToListAsync();

            if (projects == null || !projects.Any())
            {
                return NotFound();
            }

            return Ok(projects);
        }


        // POST: api/projects/Create
        [HttpPost]
        [Authorize(Roles = "JobSeeker")]  // Only JobSeekers can create their own projects
        public IActionResult CreateProject([FromBody] ProjectDto projectDto)
        {
            if (ModelState.IsValid)
            {
                // Map the DTO to the Project entity
                var project = new Project
                {
                    ProjectName = projectDto.ProjectName,
                    Description = projectDto.Description,
                    Technologies = projectDto.Technologies,
                    StartDate = projectDto.StartDate,
                    EndDate = projectDto.EndDate,
                    JobSeekerId = projectDto.JobSeekerId
                };

                // Save the project to the database
                _context.Projects.Add(project);
                _context.SaveChanges();

                return Ok(project); // Or return a different response as needed
            }

            return BadRequest(ModelState);
        }

        // PUT: api/projects/Update/{id}
        [HttpPut("Update/{id}")]
        [Authorize(Roles = "JobSeeker")]  // Only JobSeekers can update their own projects
        public async Task<IActionResult> UpdateProject(int id, ProjectDto project)
        {
            if (id != project.ProjectId)
            {
                return BadRequest();
            }

            // Ensure the current user is the owner of the project they are updating
            var currentUser = User.Identity.Name;
            var existingProject = await _context.Projects.FindAsync(id);
            //if (existingProject.JobSeeker.User.Email != currentUser)
            //{
            //    return Forbid(); // Forbid access if the JobSeeker is trying to update another user's project
            //}

            // Update the project
            _context.Entry(existingProject).CurrentValues.SetValues(project);
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
            //if (User.IsInRole("JobSeeker") && project.JobSeeker.User.Email != currentUser)
            //{
            //    return Forbid(); // Forbid access if the JobSeeker is trying to delete another user's project
            //}

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpGet("GetByProjectId/{projectId}")]
        [Authorize(Roles = "Admin,JobSeeker")]  // Admins and JobSeekers can view project details
        public async Task<ActionResult<ProjectDto>> GetProjectById(int projectId)
        {
            var project = await _context.Projects
                .Where(p => p.ProjectId == projectId)
                .Select(p => new ProjectDto
                {
                    ProjectId = p.ProjectId,
                    ProjectName = p.ProjectName,
                    Description = p.Description,
                    Technologies = p.Technologies,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    JobSeekerId = p.JobSeekerId
                })
                .FirstOrDefaultAsync();

            if (project == null)
            {
                return NotFound();
            }

            return Ok(project);
        }


        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.ProjectId == id);
        }
    }
}