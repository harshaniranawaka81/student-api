using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text.Json;
using StudentApi.Business.Contracts;
using StudentApi.Business.Services;
using StudentApi.Controllers;
using StudentApi.Repository;
using StudentApi.Repository.Contracts;

namespace StudentApi.Test
{
	public class StudentControllerTest
	{
		private IStudentRepository _studentRepository;
		private IStudentService _studentService;
		private StudentController _studentController;

		[SetUp]
		public void Setup()
		{

		}

		[Test]
		public async Task GetAllStudents_HaveRecords_ReturnsOKStatus()
		{
			//Arrange
			var contextOptions = new DbContextOptionsBuilder<StudentDbContext>()
				.UseInMemoryDatabase(databaseName: "StudentDbInMemory")
				.Options;

			using var context = new StudentDbContext(contextOptions);

			context.Database.EnsureDeleted();
			context.Database.EnsureCreated();

			context.AddRange(
				new Student { StudentId = 1, StudentName = "Harshani" },
				new Student { StudentId = 2, StudentName = "Viraj" },
				new Student { StudentId = 3, StudentName = "Duneesha" },
				new Student { StudentId = 4, StudentName = "Sachithra" },
				new Student { StudentId = 5, StudentName = "Test" });

			context.SaveChanges();

			_studentRepository = new StudentRepository(context);
			_studentService = new StudentService(_studentRepository);

			var logger = new Mock<ILogger<StudentController>>();
			_studentController = new StudentController(logger.Object, _studentService);

			//Act
			var result = await _studentController.GetAllStudents();

			//Assert
			Assert.IsInstanceOf(typeof(OkObjectResult), result);
			Assert.IsInstanceOf(typeof(List<Student>), ((OkObjectResult)result).Value);
			Assert.AreEqual(5, ((List<Student>)((OkObjectResult)result).Value).Count);
		}

		[Test]
		public async Task GetAllStudents_NoRecords_ReturnsNoContentStatus()
		{
			//Arrange
			var contextOptions = new DbContextOptionsBuilder<StudentDbContext>()
				.UseInMemoryDatabase(databaseName: "StudentDbInMemory")
				.Options;

			using var context = new StudentDbContext(contextOptions);

			context.Database.EnsureDeleted();
			context.Database.EnsureCreated();

			_studentRepository = new StudentRepository(context);
			_studentService = new StudentService(_studentRepository);

			var logger = new Mock<ILogger<StudentController>>();
			_studentController = new StudentController(logger.Object, _studentService);

			//Act
			var result = await _studentController.GetAllStudents();

			//Assert
			Assert.IsInstanceOf(typeof(NoContentResult), result);
		}

		[Test]
		public async Task GetAllStudents_Error_ReturnsBadRequestStatus()
		{
			//Arrange
			var studentRepository = new Mock<IStudentRepository>();
			var studentService = new Mock<IStudentService>();

			var expectedResult = new KeyValuePair<HttpStatusCode, List<Student>>(HttpStatusCode.BadRequest, new List<Student>());

			studentService.Setup(s => s.GetAllStudents()).ReturnsAsync(expectedResult);

			var logger = new Mock<ILogger<StudentController>>();
			var studentController = new StudentController(logger.Object, studentService.Object);

			//Act
			var result = await studentController.GetAllStudents();

			//Assert
			Assert.IsInstanceOf(typeof(BadRequestObjectResult), result);
		}



		[Test]
		public async Task GetStudent_ValidId_ReturnsOKStatus()
		{
			//Arrange
			var contextOptions = new DbContextOptionsBuilder<StudentDbContext>()
				.UseInMemoryDatabase(databaseName: "StudentDbInMemory")
				.Options;

			using var context = new StudentDbContext(contextOptions);

			context.Database.EnsureDeleted();
			context.Database.EnsureCreated();

			context.AddRange(
				new Student { StudentId = 1, StudentName = "Harshani" },
				new Student { StudentId = 5, StudentName = "Test" });

			context.SaveChanges();

			_studentRepository = new StudentRepository(context);
			_studentService = new StudentService(_studentRepository);

			var logger = new Mock<ILogger<StudentController>>();
			_studentController = new StudentController(logger.Object, _studentService);

			//Act
			var result = await _studentController.GetStudent(1);

			//Assert
			Assert.IsInstanceOf(typeof(OkObjectResult), result);
			Assert.IsInstanceOf(typeof(Student), ((OkObjectResult)result).Value);
			Assert.AreEqual("Harshani", ((Student)((OkObjectResult)result).Value).StudentName);
		}

		[Test]
		public async Task GetStudent_InvalidStudentId_ReturnsNotFoundStatus()
		{
			//Arrange
			var contextOptions = new DbContextOptionsBuilder<StudentDbContext>()
				.UseInMemoryDatabase(databaseName: "StudentDbInMemory")
				.Options;

			using var context = new StudentDbContext(contextOptions);

			context.Database.EnsureDeleted();
			context.Database.EnsureCreated();

			_studentRepository = new StudentRepository(context);
			_studentService = new StudentService(_studentRepository);

			var logger = new Mock<ILogger<StudentController>>();
			_studentController = new StudentController(logger.Object, _studentService);

			//Act
			var result = await _studentController.GetStudent(1);

			//Assert
			Assert.IsInstanceOf(typeof(NotFoundResult), result);
		}

		[Test]
		public async Task GetStudent_Error_ReturnsBadRequestStatus()
		{
			//Arrange
			var studentRepository = new Mock<IStudentRepository>();
			var studentService = new Mock<IStudentService>();

			var studentId = 1;

			var expectedResult = new KeyValuePair<HttpStatusCode, Student>(HttpStatusCode.BadRequest, new Student(){StudentId = 10, StudentName = "Test10"});

			studentService.Setup(s => s.GetStudent(studentId)).ReturnsAsync(expectedResult);

			var logger = new Mock<ILogger<StudentController>>();
			var studentController = new StudentController(logger.Object, studentService.Object);

			//Act
			var result = await studentController.GetStudent(studentId);

			//Assert
			Assert.IsInstanceOf(typeof(BadRequestObjectResult), result);
		}

		[Test]
		public async Task SaveStudent_ValidStudentObject_ReturnsCreatedStatus()
		{
			//Arrange
			var contextOptions = new DbContextOptionsBuilder<StudentDbContext>()
				.UseInMemoryDatabase(databaseName: "StudentDbInMemory")
				.Options;

			using var context = new StudentDbContext(contextOptions);

			context.Database.EnsureDeleted();
			context.Database.EnsureCreated();

			context.AddRange(
				new Student { StudentId = 1, StudentName = "Harshani" },
				new Student { StudentId = 5, StudentName = "Test" });

			context.SaveChanges();

			_studentRepository = new StudentRepository(context);
			_studentService = new StudentService(_studentRepository);

			var logger = new Mock<ILogger<StudentController>>();
			_studentController = new StudentController(logger.Object, _studentService);

			//Act
			var result = await _studentController.SaveStudent(new Student(){StudentId = 2, StudentName = "Viraj"});

			//Assert
			Assert.IsInstanceOf(typeof(CreatedResult), result);
			Assert.AreEqual(true, (((CreatedResult)result).Value));
		}

		[Test]
		public async Task SaveStudent_InvalidStudentId_ReturnsNoContentStatus()
		{
			//Arrange
			var studentRepository = new Mock<IStudentRepository>();
			var studentService = new Mock<IStudentService>();

			var student = new Student() { StudentId = 2, StudentName = "Viraj" };

			var expectedResult = new KeyValuePair<HttpStatusCode, bool>(HttpStatusCode.NoContent, false);

			studentService.Setup(s => s.SaveStudent(student)).ReturnsAsync(expectedResult);

			var logger = new Mock<ILogger<StudentController>>();
			var studentController = new StudentController(logger.Object, studentService.Object);

			//Act
			var result = await studentController.SaveStudent(student);

			//Assert
			Assert.IsInstanceOf(typeof(NoContentResult), result);
		}

		[Test]
		public async Task SaveStudent_Error_ReturnsBadRequestStatus()
		{
			//Arrange
			var studentRepository = new Mock<IStudentRepository>();
			var studentService = new Mock<IStudentService>();

			var student = new Student() { StudentId = 2, StudentName = "Viraj" };

			var expectedResult = new KeyValuePair<HttpStatusCode, bool>(HttpStatusCode.BadRequest, false);

			studentService.Setup(s => s.SaveStudent(student)).ReturnsAsync(expectedResult);

			var logger = new Mock<ILogger<StudentController>>();
			var studentController = new StudentController(logger.Object, studentService.Object);

			//Act
			var result = await studentController.SaveStudent(student);

			//Assert
			Assert.IsInstanceOf(typeof(BadRequestObjectResult), result);
		}




		[Test]
		public async Task DeleteStudent_ValidId_ReturnsNoContentStatus()
		{
			//Arrange
			var contextOptions = new DbContextOptionsBuilder<StudentDbContext>()
				.UseInMemoryDatabase(databaseName: "StudentDbInMemory")
				.Options;

			using var context = new StudentDbContext(contextOptions);

			context.Database.EnsureDeleted();
			context.Database.EnsureCreated();

			context.AddRange(
				new Student { StudentId = 1, StudentName = "Harshani" },
				new Student { StudentId = 5, StudentName = "Test" });

			context.SaveChanges();

			_studentRepository = new StudentRepository(context);
			_studentService = new StudentService(_studentRepository);

			var logger = new Mock<ILogger<StudentController>>();
			_studentController = new StudentController(logger.Object, _studentService);

			//Act
			var result = await _studentController.DeleteStudent(5);

			//Assert
			Assert.IsInstanceOf(typeof(NoContentResult), result);
		}

		[Test]
		public async Task DeleteStudent_InvalidStudentId_ReturnsNotFoundStatus()
		{
			//Arrange
			var contextOptions = new DbContextOptionsBuilder<StudentDbContext>()
				.UseInMemoryDatabase(databaseName: "StudentDbInMemory")
				.Options;

			using var context = new StudentDbContext(contextOptions);

			context.Database.EnsureDeleted();
			context.Database.EnsureCreated();

			context.AddRange(
				new Student { StudentId = 1, StudentName = "Harshani" },
				new Student { StudentId = 5, StudentName = "Test" });

			context.SaveChanges();

			_studentRepository = new StudentRepository(context);
			_studentService = new StudentService(_studentRepository);

			var logger = new Mock<ILogger<StudentController>>();
			_studentController = new StudentController(logger.Object, _studentService);

			//Act
			var result = await _studentController.DeleteStudent(10);

			//Assert
			Assert.IsInstanceOf(typeof(NotFoundResult), result);
		}

		[Test]
		public async Task DeleteStudent_Error_ReturnsBadRequestStatus()
		{
			//Arrange
			var studentRepository = new Mock<IStudentRepository>();
			var studentService = new Mock<IStudentService>();

			var studentId = 1;

			var expectedResult = new KeyValuePair<HttpStatusCode, bool>(HttpStatusCode.BadRequest, false);

			studentService.Setup(s => s.DeleteStudent(studentId)).ReturnsAsync(expectedResult);

			var logger = new Mock<ILogger<StudentController>>();
			var studentController = new StudentController(logger.Object, studentService.Object);

			//Act
			var result = await studentController.DeleteStudent(studentId);

			//Assert
			Assert.IsInstanceOf(typeof(BadRequestObjectResult), result);
		}
	}
}