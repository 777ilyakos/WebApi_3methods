namespace WebApi_3methods.Models
{
    public class Files
    {
        public int Id { get; set; }
        public string FileName { get; set; }

        public List<Values> Values { get; set; } = new();

        public int ResultId { get; set; }
        public Results results { get; set; }
    }
}
