namespace WebApi_3methods.Models
{
    public class Values
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public int Time { get; set; }
        public double Value { get; set; }

        public int FileId {  get; set; }
        public Files File { get; set; } 
    }
}
