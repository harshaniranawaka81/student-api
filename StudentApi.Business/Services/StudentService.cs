using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudentApi.Repository;
using StudentApi.Repository.Contracts;

namespace StudentApi.Business.Services
{
    public class StudentService : IStudentService
	{
		private readonly IStudentRepository _studentRepository;

		public StudentService(IStudentRepository studentRepository)
		{
			_studentRepository = studentRepository;
		}

		public async Task<KeyValuePair<HttpStatusCode, List<Student>>> GetAllStudents()
		{
			var result = await _studentRepository.GetAllStudents();

			if (result.Count > 0)
			{
				return new KeyValuePair<HttpStatusCode, List<Student>>(HttpStatusCode.OK, result);
			}
			else
			{
				return new KeyValuePair<HttpStatusCode, List<Student>>(HttpStatusCode.NoContent, result);
			}
		}

		public async Task<KeyValuePair<HttpStatusCode, Student?>> GetStudent(int id)
		{
			if (id == 0)
			{
				return new KeyValuePair<HttpStatusCode, Student?>(HttpStatusCode.BadRequest, null);
			}

			var result = await _studentRepository.GetStudent(id);

			if (result != null)
			{
				return new KeyValuePair<HttpStatusCode, Student?>(HttpStatusCode.OK, result);
			}
			else
			{
				return new KeyValuePair<HttpStatusCode, Student?>(HttpStatusCode.NotFound, result);
			}
		}

		public async Task<KeyValuePair<HttpStatusCode, bool>> SaveStudent(Student student)
		{
			var isSaved = await _studentRepository.SaveStudent(student);

			if (isSaved)
			{
				return new KeyValuePair<HttpStatusCode, bool>(HttpStatusCode.Created, isSaved);
			}
			else
			{
				return new KeyValuePair<HttpStatusCode, bool>(HttpStatusCode.NoContent, isSaved);
			}
		}

		public async Task<KeyValuePair<HttpStatusCode, bool>> DeleteStudent(int id)
		{
			if (id == 0)
			{
				return new KeyValuePair<HttpStatusCode, bool>(HttpStatusCode.BadRequest, false);
			}

			var isDeleted = await _studentRepository.DeleteStudent(id);

			if (isDeleted)
			{
				return new KeyValuePair<HttpStatusCode, bool>(HttpStatusCode.NoContent, isDeleted);
			}
			else
			{
				return new KeyValuePair<HttpStatusCode, bool>(HttpStatusCode.NotFound, isDeleted);
			}
		}
	}
}
