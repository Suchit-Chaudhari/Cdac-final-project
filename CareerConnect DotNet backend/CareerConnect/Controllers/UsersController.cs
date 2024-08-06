// Controllers/UsersController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CareerConnect.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CareerConnect.DTO;

[Route("api/users")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly JobPortalContext _context;

    public UsersController(JobPortalContext context)
    {
        _context = context;
    }

    // GET: api/users/getall
    [HttpGet("getall")]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        return await _context.Users.ToListAsync();
    }

    // GET: api/users/get/{id}
    [HttpGet("get/{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        return user;
    }

    // POST: api/users/register
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationDto registrationDto)
    {
        if (registrationDto == null)
        {
            return BadRequest("Invalid data.");
        }

        var user = new User
        {
            Email = registrationDto.Email,
            Password = registrationDto.Password,
            Role = registrationDto.Role,
            ProfilePicture = registrationDto.ProfilePicture,  // ProfilePicture can be null
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        if (registrationDto.Role == "JobSeeker")
        {
            if (registrationDto.JobSeeker == null)
            {
                return BadRequest("JobSeeker data is required for JobSeeker role.");
            }

            var jobSeeker = new JobSeeker
            {
                FirstName = registrationDto.JobSeeker.FirstName,
                LastName = registrationDto.JobSeeker.LastName,
                Description = registrationDto.JobSeeker.Description,
                Projects = registrationDto.JobSeeker.Projects,
                Skills = registrationDto.JobSeeker.Skills,
                Resume = registrationDto.JobSeeker.Resume,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                User = user
            };

            _context.JobSeekers.Add(jobSeeker);
            user.JobSeeker = jobSeeker;
        }
        else if (registrationDto.Role == "Employer")
        {
            if (registrationDto.Employer == null)
            {
                return BadRequest("Employer data is required for Employer role.");
            }

            var employer = new Employer
            {
                CompanyName = registrationDto.Employer.CompanyName,
                CompanyDescription = registrationDto.Employer.CompanyDescription,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                User = user
            };

            _context.Employers.Add(employer);
            user.Employer = employer;
        }
        else
        {
            return BadRequest("Invalid role.");
        }

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok();
    }


    // POST: api/users/signin
    [HttpPost("signin")]
    public async Task<IActionResult> SignIn([FromBody] UserLoginDto signInDto)
    {
        if (signInDto == null)
        {
            return BadRequest("Invalid data.");
        }

        // Find user by email
        var user = await _context.Users
            .Include(u => u.JobSeeker)
            .Include(u => u.Employer)
            .FirstOrDefaultAsync(u => u.Email == signInDto.Email && u.Password == signInDto.Password);

        if (user == null)
        {
            return Unauthorized("Invalid email or password.");
        }

        // Create response DTO
        var responseDto = new UserSignInResponseDto
        {
            UserId = user.UserId,
            Email = user.Email,
            ProfilePictureUrl = user.ProfilePicture,
            Role = user.Role
        };

        return Ok(responseDto);
    }

    // POST: api/users/upload-profile-picture/{id}
    [HttpPost("upload-profile-picture/{id}")]
    public async Task<IActionResult> UploadProfilePicture(int id, IFormFile profilePicture)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        if (profilePicture != null && profilePicture.Length > 0)
        {
            // Define the directory and file paths
            var uploadsDirectory = Path.Combine("uploads", "profile_pictures");
            var filePath = Path.Combine(uploadsDirectory, profilePicture.FileName);

            // Check if the directory exists, if not, create it
            if (!Directory.Exists(uploadsDirectory))
            {
                Directory.CreateDirectory(uploadsDirectory);
            }

            // Save the file to the specified path
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await profilePicture.CopyToAsync(stream);
            }

            // Update the user's profile picture path
            user.ProfilePicture = filePath;
            user.UpdatedAt = DateTime.Now;

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        return Ok();
    }


    // PUT: api/users/update/{id}
    [HttpPut("update/{id}")]
    public async Task<IActionResult> PutUser(int id, User user)
    {
        if (id != user.UserId)
        {
            return BadRequest();
        }

        _context.Entry(user).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UserExists(id))
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

    // DELETE: api/users/delete/{id}
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        _context.Users.Remove(user);

        if (user.Role == "JobSeeker")
        {
            var jobSeeker = await _context.JobSeekers.FindAsync(user.JobSeeker.JobSeekerId);
            if (jobSeeker != null)
            {
                _context.JobSeekers.Remove(jobSeeker);
            }
        }
        else if (user.Role == "Employer")
        {
            var employer = await _context.Employers.FindAsync(user.Employer.EmployerId);
            if (employer != null)
            {
                _context.Employers.Remove(employer);
            }
        }

        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool UserExists(int id)
    {
        return _context.Users.Any(e => e.UserId == id);
    }
}
