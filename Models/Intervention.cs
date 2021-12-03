using System.Collections.Generic;
using System;
namespace RestfulApi.Models
{
    public class Intervention
    {
        public long Id { get; set; }
        public string status { get; set; }
        public DateTime started_At { get; set; }
    }
}