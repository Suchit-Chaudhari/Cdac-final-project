using CareerConnect.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace CareerConnect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]  // Apply authorization to the entire controller
    public class EmployersController : ControllerBase
    {
        private readonly JobPortalContext _context;

        public EmployersController(JobPortalContext context)
        {
            _context = context;
        }

        [HttpGet("GetAll")]
        [Authorize(Roles = "Admin,JobSeeker")]  // Only Admins and JobSeekers can view all employers
        public async Task<ActionResult<IEnumerable<Employer>>> GetEmployers()
        {
            return await _context.Employers.ToListAsync();
        }

        [HttpGet("GetById/{id}")]
        [Authorize(Roles = "Admin,JobSeeker,Employer")]  // All roles can view employer by ID
        public async Task<ActionResult<Employer>> GetEmployer(int id)
        {
            var employer = await _context.Employers.FindAsync(id);

            if (employer == null)
            {
                return NotFound();
            }

            return employer;
        }

        [HttpPut("Update/{id}")]
        [Authorize(Roles = "Admin,Employer")]  // Only Admins and Employers can update employer details
        public async Task<IActionResult> PutEmployer(int id, [FromBody] EmployerUpdateDto employerDto)
        {
            var employer = await _context.Employers.FindAsync(id);
            if (employer == null)
            {
                return NotFound();
            }

            // Update only the fields provided in the DTO
            if (!string.IsNullOrEmpty(employerDto.CompanyName))
            {
                employer.CompanyName = employerDto.CompanyName;
            }

            if (!string.IsNullOrEmpty(employerDto.CompanyDescription))
            {
                employer.CompanyDescription = employerDto.CompanyDescription;
            }

            employer.UpdatedAt = DateTime.Now;

            _context.Entry(employer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployerExists(id))
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
        [Authorize(Roles = "Admin,Employer")]  // Only Admins and Employers can create new employers
        public async Task<ActionResult<Employer>> PostEmployer(Employer employer)
        {
            employer.CreatedAt = DateTime.Now;
            employer.UpdatedAt = DateTime.Now;
            _context.Employers.Add(employer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployer", new { id = employer.EmployerId }, employer);
        }

        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "Admin")]  // Only Admins can delete employers
        public async Task<IActionResult> DeleteEmployer(int id)
        {
            var employer = await _context.Employers.FindAsync(id);
            if (employer == null)
            {
                return NotFound();
            }

            _context.Employers.Remove(employer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployerExists(int id)
        {
            return _context.Employers.Any(e => e.EmployerId == id);
        }
    }
}
