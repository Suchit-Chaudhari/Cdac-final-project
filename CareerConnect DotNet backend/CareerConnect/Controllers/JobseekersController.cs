using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CareerConnect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobSeekersController : ControllerBase
    {
        private readonly JobPortalContext _context;

        public JobSeekersController(JobPortalContext context)
        {
            _context = context;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<JobSeeker>>> GetJobSeekers()
        {
            return await _context.JobSeekers.ToListAsync();
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<JobSeeker>> GetJobSeeker(int id)
        {
            var jobSeeker = await _context.JobSeekers.FindAsync(id);

            if (jobSeeker == null)
            {
                return NotFound();
            }

            return jobSeeker;
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> PutJobSeeker(int id, JobSeeker jobSeeker)
        {
            if (id != jobSeeker.JobSeekerId)
            {
                return BadRequest();
            }

            jobSeeker.UpdatedAt = DateTime.Now;
            _context.Entry(jobSeeker).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobSeekerExists(id))
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

        [HttpPost("Create")]
        public async Task<ActionResult<JobSeeker>> PostJobSeeker(JobSeeker jobSeeker)
        {
            jobSeeker.CreatedAt = DateTime.Now;
            jobSeeker.UpdatedAt = DateTime.Now;
            _context.JobSeekers.Add(jobSeeker);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetJobSeeker", new { id = jobSeeker.JobSeekerId }, jobSeeker);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteJobSeeker(int id)
        {
            var jobSeeker = await _context.JobSeekers.FindAsync(id);
            if (jobSeeker == null)
            {
                return NotFound();
            }

            _context.JobSeekers.Remove(jobSeeker);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool JobSeekerExists(int id)
        {
            return _context.JobSeekers.Any(e => e.JobSeekerId == id);
        }
    }
}
