using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentApi.Repository.Contracts;

namespace StudentApi.Repository
{
    public interface IStudentRepository
	{
		Task<List<Student>> GetAllStudents();

		Task<Student?> GetStudent(int id);

		Task<bool> SaveStudent(Student student);

		Task<bool> DeleteStudent(int id);
	}
}
