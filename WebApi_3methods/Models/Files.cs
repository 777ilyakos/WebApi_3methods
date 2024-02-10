namespace WebApi_3methods.Models
{
    public class Files
    {
        public int Id { get; set; }
        public string FileName { get; set; }

        public virtual List<Values> Values { get; set; } = new();

        public Results results { get; set; }
    }
}
