using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using lab13_api.Models;

namespace lab13_api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private readonly SchoolContext _context;

        public EnrollmentsController(SchoolContext context)
        {
            _context = context;
        }

        // GET: api/Enrollments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Enrollment>>> GetEnrollments()
        {
          if (_context.Enrollments == null)
          {
              return NotFound();
          }
            return await _context.Enrollments.ToListAsync();
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Enrollment>>> GetEnrollments2(string? FirstName, string? Name)
        {
            if (_context.Enrollments == null)
            {
                return NotFound();
            }
            return await _context.Enrollments.Include(p => p.Student).Include(p => p.Course)
                .Where(p => p.Student.FirstName.Contains(FirstName) || p.Course.Name.Contains(Name))
                .OrderByDescending(p => p.Course.Name)
                .ToListAsync();
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Enrollment>>> GetEnrollments3(string? Name, string? LastName)
        {
            if (_context.Enrollments == null)
            {
                return NotFound();
            }
            return await _context.Enrollments.Include(p => p.Student).Include(p => p.Course)
                .Where(p => p.Course.Name.Contains(Name))
                .OrderByDescending(p => p.Course.Name).OrderByDescending(p => p.Student.LastName)
                .ToListAsync();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Enrollment>>> GetEnrollments4(string? FirstName, string? Name)
        {
            if (_context.Enrollments == null)
            {
                return NotFound();
            }
            return await _context.Enrollments.Include(p => p.Student).Include(p => p.Course)
                .Where(p => p.Student.FirstName.Contains(FirstName) || p.Course.Name.Contains(Name))
                .OrderByDescending(p => p.Course.Name)
                .ToListAsync();
        }

        // GET: api/Enrollments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Enrollment>> GetEnrollment(int id)
        {
          if (_context.Enrollments == null)
          {
              return NotFound();
          }
            var enrollment = await _context.Enrollments.FindAsync(id);

            if (enrollment == null)
            {
                return NotFound();
            }

            return enrollment;
        }

        // PUT: api/Enrollments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEnrollment(int id, Enrollment enrollment)
        {
            if (id != enrollment.EnrollmentID)
            {
                return BadRequest();
            }

            _context.Entry(enrollment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EnrollmentExists(id))
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

        // POST: api/Enrollments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Enrollment>> PostEnrollment(Enrollment enrollment)
        {
          if (_context.Enrollments == null)
          {
              return Problem("Entity set 'SchoolContext.Enrollments'  is null.");
          }
            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEnrollment", new { id = enrollment.EnrollmentID }, enrollment);
        }

        // DELETE: api/Enrollments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnrollment(int id)
        {
            if (_context.Enrollments == null)
            {
                return NotFound();
            }
            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment == null)
            {
                return NotFound();
            }

            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EnrollmentExists(int id)
        {
            return (_context.Enrollments?.Any(e => e.EnrollmentID == id)).GetValueOrDefault();
        }
    }
}
