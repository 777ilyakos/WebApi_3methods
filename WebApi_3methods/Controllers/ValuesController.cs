using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            var err = Services.LoadFile.SetFile(uploadedFile);
            return err == null ? Ok() : BadRequest(err);
        }
        [HttpGet]
        public ActionResult GetResults(string? filename,
                                      DateTime? dateTimeStart, DateTime? dateTimeEnd,
                                      double? averageValueMax, double? averageValueMin,
                                      int? averageTimeMax, int? averageTimeMin)
        {

            return Json(dateTimeEnd);
            return Ok();
        }
    }
}
