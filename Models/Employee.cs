using System.Collections.Generic;

namespace RestfulApi.Models
{
    public class Employee
    {
        public long Id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }

    }
}