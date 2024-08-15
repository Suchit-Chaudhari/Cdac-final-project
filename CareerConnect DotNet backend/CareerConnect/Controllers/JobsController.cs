using CareerConnect.Models;
using CareerConnect.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace CareerConnect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]  // Apply authorization to the entire controller
    public class JobsController : ControllerBase
    {
        private readonly JobPortalContext _context;

        public JobsController(JobPortalContext context)
        {
            _context = context;
        }

        [HttpGet("GetAll")]
        [Authorize(Roles = "Admin,JobSeeker,Employer")]  // All roles can view all jobs
        public async Task<ActionResult<IEnumerable<Job>>> GetAllJobs()
        {
            return await _context.Jobs.ToListAsync();
        }

        [HttpGet("GetById/{id}")]
        [Authorize(Roles = "Admin,JobSeeker,Employer")]  // All roles can view job by ID
        public async Task<ActionResult<Job>> GetJobById(int id)
        {
            var job = await _context.Jobs.FindAsync(id);

            if (job == null)
            {
                return NotFound();
            }

            return job;
        }

        [HttpGet("GetByEmployerId/{employerId}")]
        [Authorize(Roles = "Admin,Employer")]  // Only Admins and Employers can view jobs by Employer ID
        public async Task<ActionResult<IEnumerable<Job>>> GetJobsByEmployerId(int employerId)
        {
            var jobs = await _context.Jobs
                .Where(j => j.EmployerId == employerId)
                .ToListAsync();

            if (jobs == null || !jobs.Any())
            {
                return NotFound();
            }

            return jobs;
        }

        [HttpGet("Filter")]
        [Authorize(Roles = "Admin,JobSeeker,Employer")]  // All roles can filter jobs
        public async Task<ActionResult<IEnumerable<Job>>> FilterJobs([FromQuery] JobFilterModel filters)
        {
            var query = _context.Jobs.AsQueryable();

            if (!string.IsNullOrEmpty(filters.JobTitle))
            {
                query = query.Where(j => j.JobTitle.Contains(filters.JobTitle));
            }

            if (!string.IsNullOrEmpty(filters.Location))
            {
                query = query.Where(j => j.Location.Contains(filters.Location));
            }

            if (!string.IsNullOrEmpty(filters.JobType))
            {
                query = query.Where(j => j.JobType.Contains(filters.JobType));
            }

            if (!string.IsNullOrEmpty(filters.ExperienceLevel))
            {
                query = query.Where(j => j.ExperienceLevel.Contains(filters.ExperienceLevel));
            }

            if (!string.IsNullOrEmpty(filters.Industry))
            {
                query = query.Where(j => j.Industry.Contains(filters.Industry));
            }

            if (filters.MinSalary.HasValue)
            {
                query = query.Where(j => j.Salary >= filters.MinSalary);
            }

            if (filters.MaxSalary.HasValue)
            {
                query = query.Where(j => j.Salary <= filters.MaxSalary);
            }

            var filteredJobs = await query.ToListAsync();

            return Ok(filteredJobs);
        }

        [HttpPost("Create")]
        [Authorize(Roles = "Admin,Employer")]  // Only Admins and Employers can create jobs
        public async Task<ActionResult<Job>> CreateJob([FromBody] JobCreateDto jobDto)
        {
            if (ModelState.IsValid)
            {
                var job = new Job
                {
                    EmployerId = jobDto.EmployerId,
                    JobTitle = jobDto.JobTitle,
                    JobDescription = jobDto.JobDescription,
                    Location = jobDto.Location,
                    Salary = jobDto.Salary,
                    JobType = jobDto.JobType,
                    ExperienceLevel = jobDto.ExperienceLevel,
                    Industry = jobDto.Industry,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                _context.Jobs.Add(job);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetJobById), new { id = job.JobId }, job);
            }
            return BadRequest(ModelState);
        }

        [HttpPut("Edit/{id}")]
        [Authorize(Roles = "Admin,Employer")]  // Only Admins and Employers can edit jobs
        public async Task<IActionResult> EditJob(int id, [FromBody] Job job)
        {
            if (id != job.JobId)
            {
                return BadRequest();
            }

            _context.Entry(job).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobExists(id))
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

        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "Admin,Employer")]  // Only Admins and Employers can delete jobs
        public async Task<IActionResult> DeleteJob(int id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job == null)
            {
                return NotFound();
            }

            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool JobExists(int id)
        {
            return _context.Jobs.Any(e => e.JobId == id);
        }
    }
}
