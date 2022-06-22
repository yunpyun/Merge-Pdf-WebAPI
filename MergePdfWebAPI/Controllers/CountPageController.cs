using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using MergePdfLib;

namespace MergePdfWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountPageController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "result get";
        }

        [HttpPost]
        public int PageCounting(IFormFile file)
        {
            var filePath = Path.GetTempPath();
            var fileName = filePath + file.FileName;

            using (var stream = System.IO.File.Create(fileName))
            {
                file.CopyTo(stream);
            }

            // запуск библиотеки
            Merge merge = new Merge();
            int res = merge.CountPages(fileName);

            return res;
        }
    }
}
