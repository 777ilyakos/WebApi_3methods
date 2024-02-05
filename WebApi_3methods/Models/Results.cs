namespace WebApi_3methods.Models
{
    public class Results
    {
        public int Id { get; set; }
        public TimeSpan AllTime { get; set; }
        public DateTime MinDate { get; set; }
        public DateTime MaxDate { get; set; }
        public int AverageTime { get; set; }
        public int AverageValue {  get; set; }
        public int MinValue {  get; set; }
        public int MaxValue {  get; set; }
        public int CountRecord { get; set; }

        public int FilesId { get; set; }
        public Files File { get; set; }
    }
}
