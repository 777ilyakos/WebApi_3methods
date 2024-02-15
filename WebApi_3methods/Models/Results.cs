namespace WebApi_3methods.Models
{
    public class Results
    {
        public int Id { get; set; }
        public TimeSpan AllTime { get; set; }
        public DateTime MinDate { get; set; }
        public DateTime MaxDate { get; set; }
        public double AverageTime { get; set; }
        public double AverageValue {  get; set; }
        public double MinValue {  get; set; }
        public double MaxValue {  get; set; }
        public int CountRecord { get; set; }

        public int FilesId { get; set; }
        public Files Files { get; set; }
    }
}
