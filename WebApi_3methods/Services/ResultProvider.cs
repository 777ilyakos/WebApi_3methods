using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.Linq;
using System.Text.RegularExpressions;
using WebApi_3methods.Models;
namespace WebApi_3methods.Services
{
    public class ResultProvider
    {
        private List<Models.Results> _results;

        public string FilterFileName
        {
            set
            {
                string pattern = value;
                _results = _results.Where(q => Regex.Matches(q.Files.FileName, pattern, RegexOptions.IgnoreCase).Count > 0).ToList();
            }
        }
        public DateTime FilterMinDateTime
        {
            set
            {
                _results = _results.Where(q => q.MinDate >= value).ToList();
            }
        }
        public DateTime FilterMaxDateTime
        {
            set
            {
                _results = _results.Where(q => q.MinDate <= value).ToList();
            }
        }
        public double FilterMaxAverageValue
        {
            set
            {
                _results = _results.Where(q => q.AverageValue <= value).ToList();
            }
        }
        public double FilterMinAverageValue
        {
            set
            {
                _results = _results.Where(q => q.AverageValue >= value).ToList();
            }
        }
        public double FilterMaxAverageTime
        {
            set
            {
                _results = _results.Where(q => q.AverageTime <= value).ToList();
            }
        }
        public double FilterMinAverageTime
        {
            set
            {
                _results = _results.Where(q => q.AverageTime >= value).ToList();
            }
        }

        public ResultProvider()
        {
            using (TaskdbContext db = new TaskdbContext())
            {
                db.Files.Include(q => q.results).Load();
                _results = db.Results.ToList();
            }
        }
    }
}
