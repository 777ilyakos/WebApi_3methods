using Microsoft.VisualBasic;
using Microsoft.VisualBasic.FileIO;
using System.Runtime.CompilerServices;
using WebApi_3methods.Models;

namespace WebApi_3methods.Services
{
    public class task
    {
        static TaskdbContext db = new TaskdbContext();
        /// <summary>
        /// записывает файл в базу данных:<br/>
        /// <br/>
        /// его стоки преобразовываются и записываются в Value<br/>
        /// всю статистику в Result<br/>
        /// <br/>
        /// если файл уже был ранее то перезаписываем
        /// </summary>
        /// <param name="file">
        ///  .csv, в котором на каждой новой строке значение вида:
        ///{Дата и время в формате ГГГГ-ММ-ДД_чч-мм-сс};{Целочисленное значение времени в секундах};{Показатель в виде числа с плавающей запятой}
        ///
        ///Пример:
        ///2022-03-18_09-18-17;1744;1632,472
        /// </param>
        /// <returns>возвращает ошибку или null если ошибок не было</returns>
        public static string SetFile(in IFormFile file)
        {
            Models.Results result; //статистика текущего файла
            int fileId;   //id файла в базе данных


            try
            {
                //узнаём fileId и создаём result
                using (TaskdbContext db = new TaskdbContext())
                {
                    string fileName = file.FileName;
                    var files = db.Files.Where(i => i.FileName == fileName);
                    if (files.Count() > 0)
                    {
                        fileId = files.First().Id;
                        result = files.First().results;
                    }
                    else
                    {
                        //записываем файл в базу
                        Files file1 = new Files();
                        result = new Models.Results();
                        file1.FileName = file.FileName;
                        result.File = file1;
                        file1.results = result;
                        db.Files.Add(file1);
                        files = db.Files.Where(i => i.FileName == fileName);
                        fileId = files.First().Id;
                        db.SaveChanges();
                    }
                }


                //открываем файл
                List<Values> values = new();//лист значений из файла
                Stream streamReaderFile = file.OpenReadStream();
                using (TextFieldParser fieldParser = new TextFieldParser(streamReaderFile))
                {
                    fieldParser.TextFieldType = FieldType.Delimited;
                    fieldParser.SetDelimiters(";");

                    while (!fieldParser.EndOfData)
                    {

                    }
                }


                db.Values.AddRange();
                db.SaveChanges();

            }
            catch (Exception e)
            {
                return e.Message;
            }
            return null;
        }
        /// <summary>
        /// празует строку в дату и проверяет на то находится ли дата в заданном диапозоне<br/>
        /// <br/>
        /// в случае неудачи преобразования или не поподания даты в диапозон выдаёт ошибку
        /// </summary>
        /// <param name="stringDateTime">строка требующая преобразования</param>
        /// <param name="minDate">минимальная дата для диапозона</param>
        /// <param name="maxDate">максимальная дата для диапозона</param>
        /// <param name="format">формат даты</param>
        /// <returns></returns>
        static DateTime ToDataTime(string stringDateTime, DateTime minDate, DateTime maxDate, string format= "yyyy-MM-dd_HH-mm-ss")
        {
            try
            {
                DateTime myDate = DateTime.ParseExact(stringDateTime, format, null);
                if (myDate < maxDate && myDate > minDate)
                {
                    return myDate;
                }
                else 
                {
                throw (new Exception($"{myDate.ToString("yyyy-MM-dd HH:mm:ss")} дата находится за пределами диапозона "+
                    $"\"{minDate.ToString("yyyy-MM-dd HH:mm:ss")}\"-\"{maxDate.ToString("yyyy-MM-dd HH:mm:ss")}\""));

                }
            }
            catch
            {
                throw (new Exception($"{stringDateTime} не удалось преобразовать в дату и время(DateTime) в сооттветствии с форматом {format}"));
            }


        }
    }
}
