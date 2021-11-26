namespace RestfulApi.Models
{
    public class Building
    {
        
        public long Id { get; set; }
        
        public virtual Battery batteries { get; set; }
        public virtual Column columns { get; set; }
        public virtual Elevator elevators { get; set; }
        
    
        // public string status { get; set; }

    }
    
}