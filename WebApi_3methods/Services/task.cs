using Microsoft.VisualBasic;
using Microsoft.VisualBasic.FileIO;
using WebApi_3methods.Models;

namespace WebApi_3methods.Services
{
    public class task
    {
        static TaskdbContext db = new TaskdbContext();
        /// <summary>
        /// записывает файл в базу данных:
        /// 
        /// его стоки преобразовываются и записываются в Value,
        /// всю статистику в Result;
        /// 
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
            int fileId=0;//id файла в базе данных
            try 
            {
                using (TaskdbContext db = new TaskdbContext())
                {
                    
                    string fileName=file.FileName;
                    var files = db.Files.Where(i => i.FileName == fileName);
                    if (files.Count() > 0)
                    {
                        fileId = files.First().Id;
                    }
                    else
                    {
                        //клац клац
                    }
                }
                    //открываем файл
                    Stream streamReaderFile = file.OpenReadStream();
                using (TextFieldParser fieldParser = new TextFieldParser(streamReaderFile))
                {
                    fieldParser.TextFieldType= FieldType.Delimited;
                    fieldParser.SetDelimiters(";");
                    while (!fieldParser.EndOfData)
                    {
                        //клац клац
                    }
                }
            }
            catch (Exception e) 
            {
                return e.Message;
            }
            return null;
        }
    }
}
