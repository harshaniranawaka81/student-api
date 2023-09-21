using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudentApi.Repository.Contracts;

namespace StudentApi.Repository
{
    public class StudentRepository : IStudentRepository
	{
		private readonly StudentDbContext _applicationDbContext;

		public StudentRepository(StudentDbContext applicationDbContext)
		{
			_applicationDbContext = applicationDbContext;
		}

		public async Task<List<Student>> GetAllStudents()
		{
			return await _applicationDbContext.Students.ToListAsync();
		}

		public async Task<Student?> GetStudent(int id)
		{
			return await _applicationDbContext.Students.FirstOrDefaultAsync(s => s.StudentId == id);
		}

		public async Task<bool> SaveStudent(Student student)
		{
			await _applicationDbContext.Students.AddAsync(student);
			var rows = await _applicationDbContext.SaveChangesAsync();

			return rows > 0;
		}

		public async Task<bool> DeleteStudent(int id)
		{
			var student = await _applicationDbContext.Students.FirstOrDefaultAsync(s => s.StudentId == id);

			if (student == null) return false;

			_applicationDbContext.Students.Remove(student);
			await _applicationDbContext.SaveChangesAsync();

			return true;

		}
	}
}
