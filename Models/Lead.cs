using System;

namespace RestfulApi.Models
{
    
    public class Lead
    {
        public long Id { get; set; }
        public DateTime created_at { get; set; }
        public string email { get; set; }
    }
}