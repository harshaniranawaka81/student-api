using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace StudentApi.Business.Contracts
{
    public class ErrorDetails : IErrorDetails
    {
        public int Code { get; set; }
        public string? Message { get; set; }
    }
}
