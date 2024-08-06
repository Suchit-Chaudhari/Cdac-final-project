﻿using CareerConnect.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CareerConnect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationsController : ControllerBase
    {
        private readonly JobPortalContext _context;

        public ApplicationsController(JobPortalContext context)
        {
            _context = context;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Application>>> GetAllApplications()
        {
            return await _context.Applications.ToListAsync();
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<Application>> GetApplicationById(int id)
        {
            var application = await _context.Applications.FindAsync(id);

            if (application == null)
            {
                return NotFound();
            }

            return application;
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateApplication(int id, Application application)
        {
            if (id != application.ApplicationId)
            {
                return BadRequest();
            }

            _context.Entry(application).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationExists(id))
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
        public async Task<ActionResult<Application>> CreateApplication(Application application)
        {
            _context.Applications.Add(application);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetApplicationById), new { id = application.ApplicationId }, application);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteApplication(int id)
        {
            var application = await _context.Applications.FindAsync(id);
            if (application == null)
            {
                return NotFound();
            }

            _context.Applications.Remove(application);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("GetByJobSeekerId/{jobSeekerId}")]
        public async Task<ActionResult<IEnumerable<Application>>> GetApplicationsByJobSeekerId(int jobSeekerId)
        {
            var applications = await _context.Applications
                .Where(a => a.JobSeekerId == jobSeekerId)
                .ToListAsync();

            if (applications == null || !applications.Any())
            {
                return NotFound();
            }

            return applications;
        }

        [HttpGet("GetByJobId/{jobId}")]
        public async Task<ActionResult<IEnumerable<Application>>> GetApplicationsByJobId(int jobId)
        {
            var applications = await _context.Applications
                .Where(a => a.JobId == jobId)
                .ToListAsync();

            if (applications == null || !applications.Any())
            {
                return NotFound();
            }

            return applications;
        }

        [HttpPut("UpdateStatus/{id}")]
        public async Task<IActionResult> UpdateApplicationStatus(int id, [FromBody] string status)
        {
            var application = await _context.Applications.FindAsync(id);

            if (application == null)
            {
                return NotFound();
            }

            application.Status = status;
            application.UpdatedAt = DateTime.Now;

            _context.Entry(application).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationExists(id))
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

        private bool ApplicationExists(int id)
        {
            return _context.Applications.Any(e => e.ApplicationId == id);
        }
    }
}
