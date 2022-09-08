using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assessment.Models.ViewModel
{
    public class ServiceResponse
    {
        public string Status { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public object Data { get; set; }
    }

    public class AuthenticateResponse
    {
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
    }
}