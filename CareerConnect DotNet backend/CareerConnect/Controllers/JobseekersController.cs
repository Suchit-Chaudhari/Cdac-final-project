using CareerConnect.DTO;
using CareerConnect.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace CareerConnect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]  // Apply authorization to the entire controller
    public class JobSeekersController : ControllerBase
    {
        private readonly JobPortalContext _context;

        public JobSeekersController(JobPortalContext context)
        {
            _context = context;
        }

        [HttpGet("GetAll")]
        [Authorize(Roles = "Admin")]  // Only Admins can get all job seekers
        public async Task<ActionResult<IEnumerable<JobSeeker>>> GetJobSeekers()
        {
            return await _context.JobSeekers.ToListAsync();
        }

        [HttpGet("GetById/{id}")]
        [Authorize(Roles = "Admin,JobSeeker,Employer")]  // Admins can view any job seeker, JobSeekers can view their own profile
        public async Task<ActionResult<JobSeeker>> GetJobSeeker(int id)
        {
            var jobSeeker = await _context.JobSeekers.FindAsync(id);

            if (jobSeeker == null)
            {
                return NotFound();
            }

            // Check if the current user is allowed to access this job seeker's data
            var currentUser = User.Identity.Name; // Assuming the username is stored in the Name claim
            if (User.IsInRole("JobSeeker") && jobSeeker.User?.Email != currentUser)
            {
                return Forbid(); // Forbid access if the job seeker is trying to access another user's data
            }

            return jobSeeker;
        }

        [HttpPut("Update/{id}")]
        [Authorize(Roles = "JobSeeker")]  // Only JobSeekers can update their own profile
        public async Task<IActionResult> PutJobSeeker(int id, JobSeeker jobSeeker)
        {
            if (id != jobSeeker.JobSeekerId)
            {
                return BadRequest();
            }

            // Check if the current user is allowed to update this job seeker's data
            var currentUser = User.Identity.Name;
            if (jobSeeker.User?.Email != currentUser)
            {
                return Forbid(); // Forbid access if the job seeker is trying to update another user's data
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
        [Authorize(Roles = "Admin")]  // Only Admins can create job seekers
        public async Task<ActionResult<JobSeeker>> PostJobSeeker(JobSeekerDto jobSeekerDto)
        {
            var jobSeeker = new JobSeeker
            {
                FirstName = jobSeekerDto.FirstName,
                LastName = jobSeekerDto.LastName,
                Description = jobSeekerDto.Description,
                Skills = jobSeekerDto.Skills,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.JobSeekers.Add(jobSeeker);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetJobSeeker", new { id = jobSeeker.JobSeekerId }, jobSeeker);
        }

        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "Admin")]  // Only Admins can delete job seekers
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

        [HttpPost("UploadResume/{id}")]
        [Authorize(Roles = "JobSeeker")]  // Only JobSeekers can upload their own resumes
        public async Task<IActionResult> UploadResume(int id, IFormFile resumeFile)
        {
            var jobSeeker = await _context.JobSeekers.FindAsync(id);
            if (jobSeeker == null)
            {
                return NotFound();
            }

            // Check if the current user is allowed to upload a resume for this job seeker
            var currentUser = User.Identity.Name;
            if (jobSeeker.User?.Email != currentUser)
            {
                return Forbid(); // Forbid access if the job seeker is trying to upload a resume for another user
            }

            if (resumeFile != null && resumeFile.Length > 0)
            {
                var uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "uploads", "resumes");
                var fileName = $"{Guid.NewGuid()}_{resumeFile.FileName}";
                var filePath = Path.Combine(uploadsDirectory, fileName);

                if (!Directory.Exists(uploadsDirectory))
                {
                    Directory.CreateDirectory(uploadsDirectory);
                }

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await resumeFile.CopyToAsync(stream);
                }

                var resumeUrl = Path.Combine("uploads", "resumes", fileName);
                jobSeeker.Resume = resumeUrl;
                jobSeeker.UpdatedAt = DateTime.Now;

                _context.Entry(jobSeeker).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }

            return Ok();
        }

        [HttpGet("{id}/resume")]
        [Authorize(Roles = "Admin,JobSeeker")]  // Admins can view any resume, JobSeekers can view their own
        public async Task<IActionResult> GetJobSeekerResume(int id)
        {
            var jobSeeker = await _context.JobSeekers.FindAsync(id);
            if (jobSeeker == null)
            {
                return NotFound();
            }

            // Check if the current user is allowed to view this resume
            var currentUser = User.Identity.Name;
            if (User.IsInRole("JobSeeker") && jobSeeker.User?.Email != currentUser)
            {
                return Forbid(); // Forbid access if the job seeker is trying to view another user's resume
            }

            var resumePath = jobSeeker.Resume.Replace("\\", "/").Replace("/uploads/resumes/uploads/resumes/", "/uploads/resumes/");
            var baseUrl = $"{Request.Scheme}://{Request.Host}/";
            var fullResumeUrl = $"{baseUrl}{resumePath.TrimStart('/')}";
            var encodedResumeUrl = Uri.EscapeUriString(fullResumeUrl);

            return Ok(new { ResumeUrl = encodedResumeUrl });
        }

        private bool JobSeekerExists(int id)
        {
            return _context.JobSeekers.Any(e => e.JobSeekerId == id);
        }
    }
}
