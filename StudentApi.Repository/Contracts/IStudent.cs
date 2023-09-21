using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentApi.Repository.Contracts
{
    public interface IStudent
    {
	    public int Id { get; set; }

		public int StudentId { get; set; }

        public string? StudentName { get; set; }
    }
}
