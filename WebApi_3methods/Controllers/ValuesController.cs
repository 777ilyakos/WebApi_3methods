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
            var err = Services.task.SetFile(uploadedFile);
            return err == null ? Ok() : BadRequest(err);
        }
    }
}
