using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using StudentApi.Repository.Contracts;

namespace StudentApi.Business.Services
{
    public interface IStudentService
	{
		Task<KeyValuePair<HttpStatusCode, List<Student>>> GetAllStudents();

		Task<KeyValuePair<HttpStatusCode, Student?>>  GetStudent(int id);

		Task<KeyValuePair<HttpStatusCode, bool>>  SaveStudent(Student student);

		Task<KeyValuePair<HttpStatusCode, bool>> DeleteStudent(int id);
	}
}
