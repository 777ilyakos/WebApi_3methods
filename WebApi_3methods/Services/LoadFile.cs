using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.FileIO;
using WebApi_3methods.Models;

namespace WebApi_3methods.Services
{
    public class LoadFile
    {

        /// <summary>
        /// проверяет файл на его наличие в базе<br/>
        /// если он уже есть: работаем с существующим, записи старого файла удаляются<br/>
        /// если его нет: создаём
        /// </summary>
        /// <param name="file">файл с которым мы будем работать</param>
        /// <returns>связанная запись статистики(Results) с текущим файлом</returns>
        static Models.Results FileAndResultsUpdate(in IFormFile file)
        {
            using (TaskdbContext db = new TaskdbContext())
            {
                
                string fileName = file.FileName;
                db.Files.Include(x => x.results).Include(y => y.Values).Load();
                IQueryable<Files> files = db.Files.Where(i => i.FileName == fileName);
                if (files.Count() > 0)
                {
                    //удаляем прошлые записи по файлу
                    if (files.First().Values.Count != 0)
                        db.Values.RemoveRange(files.First().Values);
                    db.SaveChanges();
                    //берём данные по файлу из базы
                    return files.First().results;

                }
                else
                {
                    //записываем файл в базу
                    Files file1 = new Files();
                    Models.Results results = new Models.Results();
                    file1.FileName = file.FileName;
                    file1.results = results;

                    db.Files.Add(file1);
                    db.SaveChanges();
                    return file1.results;
                }
            }
        }

        /// <summary>
        /// считываем значения из файла
        /// </summary>
        /// <param name="file">
        ///  .csv, в котором на каждой новой строке значение вида:
        ///{Дата и время в формате ГГГГ-ММ-ДД_чч-мм-сс};{Целочисленное значение времени в секундах};{Показатель в виде числа с плавающей запятой}
        ///
        ///Пример:
        ///2022-03-18_09-18-17;1744;1632,472
        ///</param>
        /// <param name="fileId">id файла в базе данных</param>
        /// <returns>лист значений файла</returns>
        static List<Values> FileParse(in IFormFile file, int fileId) 
        {
            //открываем файл и формируем из него лист значений
            List<Values> values = new();//лист значений
            Stream streamReaderFile = file.OpenReadStream();
            using (TextFieldParser fieldParser = new TextFieldParser(streamReaderFile))
            {
                fieldParser.TextFieldType = FieldType.Delimited;
                fieldParser.SetDelimiters(";");

                while (!fieldParser.EndOfData)
                {
                    string[]? fields = fieldParser.ReadFields();//поля одной строки
                    if(fields==null)
                        continue;

                    Values value = new();
                    value.FileId = fileId;
                    //дата
                    value.DateTime = ToDataTime(fields[0], new DateTime(2000, 01, 01), DateTime.Now);
                    //время
                    if (int.TryParse(fields[1], out int time) && (time >= 0))
                    {
                        value.Time = time;
                    }
                    else
                    {
                        throw new Exception($"{string.Join(';', fields)} не удалось выделить целочисленное значение времени в секундах (время не может быть меньше 0)");
                    }
                    //Показатель
                    if (double.TryParse(fields[2], out double score) && (score >= 0))
                    {
                        value.Value = score;
                    }
                    else
                    {
                        throw new Exception($"{string.Join(';', fields)} не удалось показатель в виде числа с плавающей запятой (Значение показателя не может быть меньше 0)");
                    }

                    values.Add(value);
                }
            }
            return values;
        }

        /// <summary>
        /// записывает файл в базу данных:<br/>
        /// <br/>
        /// его строки преобразовываются и записываются в Value<br/>
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
                result= FileAndResultsUpdate(file);
                fileId=result.FilesId;

                //открываем файл и формируем из него лист значений
                List<Values> values = FileParse(file,fileId);//лист значений
                Stream streamReaderFile = file.OpenReadStream();
                //вычисление всего необходимого для результата
                result.CountRecord = values.Count;
                if (result.CountRecord < 1 || result.CountRecord > 10000)
                {
                    return $"количество записей в файле не корректно (1..10000)";
                }

                result.AverageValue = values.Average(s => s.Value);
                result.AverageTime = values.Average(s => s.Time);
                result.AllTime = new(0, 0, values.Sum(s => s.Time));
                result.MinValue = values.Min(s => s.Value);
                result.MaxValue = values.Max(s => s.Value);
                result.MinDate = values.Min(s => s.DateTime);
                result.MaxDate = values.Max(s => s.DateTime);

                //записываем результат и значения в базу данных
                using (TaskdbContext db = new TaskdbContext())
                {
                    db.Results.Update(result);
                    db.Values.AddRange(values);
                    db.SaveChanges();
                }

            }
            catch (Exception e)
            {
                return e.Message;
            }
            return null;
        }

        /// <summary>
        /// преобразует строку в дату и проверяет на то находится ли дата в заданном диапазоне<br/>
        /// <br/>
        /// в случае неудачи преобразования или не попадания даты в диапазон выдаёт ошибку
        /// </summary>
        /// <param name="stringDateTime">строка требующая преобразования</param>
        /// <param name="minDate">минимальная дата для диапазона</param>
        /// <param name="maxDate">максимальная дата для диапазона</param>
        /// <param name="format">формат даты</param>
        /// <returns></returns>
        static DateTime ToDataTime(in string stringDateTime, in DateTime minDate, in DateTime maxDate, in string format = "yyyy-MM-dd_HH-mm-ss")
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
                    throw (new Exception($"{myDate.ToString("yyyy-MM-dd HH:mm:ss")} дата находится за пределами диапазона " +
                        $"\"{minDate.ToString("yyyy-MM-dd HH:mm:ss")}\"-\"{maxDate.ToString("yyyy-MM-dd HH:mm:ss")}\""));

                }
            }
            catch
            {
                throw (new Exception($"{stringDateTime} не удалось преобразовать в дату и время(DateTime) в соответствии с форматом {format}"));
            }
        }
    }
}
