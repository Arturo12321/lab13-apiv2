﻿using System;
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
    public class StudentsController : ControllerBase
    {
        private readonly SchoolContext _context;

        public StudentsController(SchoolContext context)
        {
            _context = context;
        }

        // GET: api/Students
        [HttpGet]

        // GET: api/Students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            if (_context.Students == null)
            {
                return NotFound();
            }
                return await _context.Students.Include(p => p.Grade).ToListAsync();
            // return await _context.Students.OrderByDescending(p => p.LastName)
            //     .ThenBy(p => p.FirstName)
            //     .Select(p => { p.FirstName,  p.LastName,  p.Email })
            //     .ToListAsync();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents2(string? FirstName,string? LastName,string? Email)
        {
            if (_context.Students == null)
            {
                return NotFound();
            }
            //return await _context.Students.Include(p => p.Grade).ToListAsync();
            return await _context.Students.Where(x=>x.FirstName.Contains(FirstName) || x.LastName.Contains(LastName) || x.Email.Contains(Email)) 
                .OrderByDescending(p => p.LastName).ToListAsync();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudentsGrade(string? FirstName, string? Name)
        {
            if (_context.Students == null)
            {
                return NotFound();
            }
            //return await _context.Students.Include(p => p.Grade).ToListAsync();
            return await _context.Students.Where(x => x.FirstName.Contains(FirstName) || x.Grade.Name.Contains(Name))
                .OrderByDescending(p => p.Grade.Name).ToListAsync();
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
          if (_context.Students == null)
          {
              return NotFound();
          }
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return student;
        }

        // PUT: api/Students/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, Student student)
        {
            if (id != student.StudentID)
            {
                return BadRequest();
            }

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
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

        // POST: api/Students
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
          if (_context.Students == null)
          {
              return Problem("Entity set 'SchoolContext.Students'  is null.");
          }
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudent", new { id = student.StudentID }, student);
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            if (_context.Students == null)
            {
                return NotFound();
            }
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentExists(int id)
        {
            return (_context.Students?.Any(e => e.StudentID == id)).GetValueOrDefault();
        }
    }
}
