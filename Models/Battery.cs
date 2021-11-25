using System.Collections.Generic;

namespace RestfulApi.Models
{
    public class Battery
    {
        public long Id { get; set; }
        public string status { get; set; }
        public virtual ICollection<Building> buildings { get; } = new HashSet<Building>();

    }
}