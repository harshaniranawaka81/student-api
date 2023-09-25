using Microsoft.AspNetCore.Mvc;
using System.Net;
using StudentApi.Business.Services;
using StudentApi.Repository;
using StudentApi.Repository.Contracts;

namespace StudentApi.Controllers
{
	/// <summary>
	/// API to handle student information
	/// </summary>
	[ApiController]
	[Route("[controller]")]
	public class StudentController : ControllerBase
	{
		private readonly ILogger<StudentController> _logger;
		private readonly IStudentService _studentService;

		/// <summary>
		/// Student controller 
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="studentService"></param>
		public StudentController(ILogger<StudentController> logger, IStudentService studentService)
		{
			_logger = logger;
			_studentService = studentService;
		}

		/// <summary>
		/// Get all student details
		/// </summary>
		/// <returns></returns>
		[HttpGet()]
		[ProducesResponseType(typeof(Student), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
		[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetAllStudents()
		{
			var result = await _studentService.GetAllStudents();

			return result.Key switch
			{
				HttpStatusCode.OK => Ok(result.Value),
				HttpStatusCode.NoContent => NoContent(),
				_ => BadRequest(result.Value)
			};
		}

		/// <summary>
		/// Get student details using the id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet("{id}")]
		[ProducesResponseType(typeof(Student), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetStudent(int id)
		{
			var result = await _studentService.GetStudent(id);

			return result.Key switch
			{
				HttpStatusCode.OK => Ok(result.Value),
				HttpStatusCode.NotFound => NotFound(),
				_ => BadRequest(result.Value)
			};
		}

		/// <summary>
		/// Save a new student in the database
		/// </summary>
		/// <param name="student"></param>
		/// <returns></returns>
		[HttpPost()]
		[ProducesResponseType(typeof(Student), StatusCodes.Status201Created)]
		[ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
		[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SaveStudent(Student student)
		{
			var result = await _studentService.SaveStudent(student);

			return result.Key switch
			{
				HttpStatusCode.Created => Created(String.Empty, result.Value),
				HttpStatusCode.NoContent => NoContent(),
				_ => BadRequest(result.Value)
			};
		}

		/// <summary>
		/// Delete a student from the database usun
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpDelete("{id}")]
		[ProducesResponseType(typeof(Student), StatusCodes.Status204NoContent)]
		[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> DeleteStudent(int id)
		{
			var result = await _studentService.DeleteStudent(id);

			return result.Key switch
			{
				HttpStatusCode.NoContent => NoContent(),
				HttpStatusCode.NotFound => NotFound(),
				_ => BadRequest(result.Value)
			};
		}

	}
}