﻿using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using WebApi_3methods.Models;
namespace WebApi_3methods.Services
{
    public class ResultProvider
    {
        private List<Models.Results> _results;
        /// <summary>
        /// фильтр (по имени файла умеет работать с регулярными выражениями) <br/>
        /// фильтр применяется сразу
        /// </summary>
        public string FilterFileName
        {
            set
            {
                string pattern = value.ToLower();
                _results = _results.Where(q => q.Files.FileName.ToLower().Contains(pattern)).ToList();
            }
        }
        /// <summary>
        /// фильтр (задаёт минимальную дату для запуска первой операции) <br/>
        /// фильтр применяется сразу
        /// </summary>
        public DateTime FilterMinDateTime
        {
            set
            {
                _results = _results.Where(q => q.MinDate >= value).ToList();
            }
        }
        /// <summary>
        /// фильтр (задаёт максимальную дату для запуска первой операции) <br/>
        /// фильтр применяется сразу
        /// </summary>
        public DateTime FilterMaxDateTime
        {
            set
            {
                _results = _results.Where(q => q.MinDate <= value).ToList();
            }
        }
        /// <summary>
        /// фильтр (задаёт максимальный средний показатель) <br/>
        /// фильтр применяется сразу
        /// </summary>
        public double FilterMaxAverageValue
        {
            set
            {
                _results = _results.Where(q => q.AverageValue <= value).ToList();
            }
        }
        /// <summary>
        /// фильтр (задаёт минимальный средний показатель) <br/>
        /// фильтр применяется сразу
        /// </summary>
        public double FilterMinAverageValue
        {
            set
            {
                _results = _results.Where(q => q.AverageValue >= value).ToList();
            }
        }
        /// <summary>
        /// фильтр (задаёт максимальное среднее время) <br/>
        /// фильтр применяется сразу
        /// </summary>
        public double FilterMaxAverageTime
        {
            set
            {
                _results = _results.Where(q => q.AverageTime <= value).ToList();
            }
        }
        /// <summary>
        /// фильтр (задаёт минимальное среднее время) <br/>
        /// фильтр применяется сразу
        /// </summary>
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

        /// <summary>
        /// возращает отфильтрованый результат
        /// </summary>
        /// <returns>отсортированый список</returns>
        public List<Models.Results> ReturnResults()
        {
            return _results.ToList();
        }
    }
}
