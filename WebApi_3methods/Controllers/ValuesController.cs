using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi_3methods.Services;

namespace WebApi_3methods.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : Controller
    {
        [HttpPost]
        public ActionResult<IFormFile> GetFile(IFormFile uploadedFile)
        {
            //передаём файл в функцию, на основе этого отвечаем на запрос(Null--ok;NotNull--bad)
            var err = LoadFile.SetFile(uploadedFile);
            return err == null ? Ok() : BadRequest(err);
        }
        [HttpGet]
        public ActionResult GetResults(string? filename,
                                      DateTime? dateTimeMax, DateTime? dateTimeMin,
                                      double? averageValueMax, double? averageValueMin,
                                      double? averageTimeMax, double? averageTimeMin)
        {
            ResultProvider resultProvider = new ResultProvider();
            #region валидация
            if (!string.IsNullOrEmpty(filename))
                resultProvider.FilterFileName = filename;

            if (dateTimeMax != null && dateTimeMin != null)
                if (dateTimeMax < dateTimeMin)
                    return BadRequest($"максимальная дата({dateTimeMax}) не может быть меньше минимальной({dateTimeMin})");
                else
                {
                    resultProvider.FilterMinDateTime = (DateTime)dateTimeMin;
                    resultProvider.FilterMaxDateTime = (DateTime)dateTimeMax;
                }

            if (averageValueMax != null && averageValueMin != null)
                if (averageValueMax < averageValueMin)
                    return BadRequest($"максимальное среднее значение({averageValueMax}) не может быть меньше минимального({averageValueMin})");
                else
                {
                    resultProvider.FilterMinAverageValue = (double)averageValueMin;
                    resultProvider.FilterMaxAverageValue = (double)averageValueMax;
                }

            if (averageTimeMax != null && averageTimeMin != null)
                if (averageTimeMax < averageTimeMin)
                    return BadRequest($"максимальное среднее значение({averageTimeMax}) не может быть меньше минимального({averageTimeMin})");
                else
                {
                    resultProvider.FilterMinAverageTime = (double)averageTimeMin;
                    resultProvider.FilterMaxAverageTime = (double)averageTimeMax;
                }
            #endregion
            return Json(resultProvider.ReturnResults());
        }
    }
}
