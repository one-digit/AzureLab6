using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab6.Data;
using Lab6.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab6.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly StudentsDbContext _context;

        public StudentsController(StudentsDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get collection of Students.
        /// </summary>
        /// <returns>A collection of Students</returns>
        /// <response code="200">Returns a collection of Students</response>
        /// <response code="500">Internal error</response>      
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Student>>> Get()
        {
            return Ok(await _context.Students.ToListAsync());
        }

        /// <summary>
        /// Get a Student.
        /// </summary>
        /// <param id="id"></param>
        /// <returns>A Student</returns>
        /// <response code="201">Returns a collection of Students</response>
        /// <response code="400">If the id is malformed</response>      
        /// <response code="404">If the Student is null</response>      
        /// <response code="500">Internal error</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Student>> GetById(Guid id)
        {
            return Ok(await _context.Students.FindAsync(id));
        }

        /// <summary>
        /// Creates a Student.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Students
        ///     {
        ///        "FirstName": "FirstName",
        ///        "LastName": "LastName",
        ///        "Program": "Program"
        ///     }
        ///
        /// </remarks>
        /// <returns>A newly created Student</returns>
        /// <response code="201">Returns the newly created Student</response>
        /// <response code="400">If the Student is malformed</response>      
        /// <response code="500">Internal error</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Student>> CreateAsync([Bind("FirstName,LastName,Program")] StudentBase StudentBase)
        {
            Student Student = new Student
            {
                FirstName = StudentBase.FirstName,
                LastName = StudentBase.LastName,
                Program = StudentBase.Program
            };

            _context.Add(Student);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = Student.ID }, Student);
        }

        /// <summary>
        /// Updates a Student.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /Students
        ///     {
        ///        "FirstName": "FirstName",
        ///        "LastName": "LastName",
        ///        "Program": "Program"
        ///     }
        ///
        /// </remarks>
        /// <param id="id"></param>
        /// <returns>An upserted Student</returns>
        /// <response code="200">Returns the updated Student</response>
        /// <response code="201">Returns the newly created Student</response>
        /// <response code="400">If the Student or id is malformed</response>      
        /// <response code="404">If the Student or id is missing</response>      
        /// <response code="500">Internal error</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Student>> Update(Guid id, [Bind("FirstName,LastName,Program")] StudentBase StudentBase)
        {
            Student Student = new Student
            {
                FirstName = StudentBase.FirstName,
                LastName = StudentBase.LastName,
                Program = StudentBase.Program
            };

            Student dbStudent = await _context.Students.FindAsync(id);
            dbStudent.FirstName = Student.FirstName;
            dbStudent.LastName = Student.LastName;
            dbStudent.Program = Student.Program;

            _context.Update(dbStudent);
            await _context.SaveChangesAsync();

            return Ok(dbStudent);
        }

        /// <summary>
        /// Deletes a Student.
        /// </summary>
        /// <param id="id"></param>
        /// <response code="202">Student is deleted</response>
        /// <response code="400">If the id is malformed</response>      
        /// <response code="500">Internal error</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var Student = await _context.Students.FindAsync(id);
            _context.Students.Remove(Student);
            await _context.SaveChangesAsync();
            return Accepted();
        }

        private bool StudentExists(Guid id)
        {
            return _context.Students.Any(e => e.ID == id);
        }

    }
}
