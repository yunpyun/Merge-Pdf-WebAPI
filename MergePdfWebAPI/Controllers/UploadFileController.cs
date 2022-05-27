using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web;
using MergePdfLib;
using Microsoft.AspNetCore.Hosting;

namespace MergePdfWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadFileController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "result get";
        }

        [HttpPost]
        public async Task<IActionResult> Upload(List<IFormFile> files)
        {
            var docfiles = new List<string>();

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var filePath = Path.GetTempFileName().Replace(".tmp", ".pdf");

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                    docfiles.Add(filePath);
                }
            }

            // запуск библиотеки
            Merge merge = new Merge();
            string res = merge.MergeDocs(docfiles);

            string fileName = "sample.pdf";
            var mimeType = "application/octet-stream";

            //Stream fileStream = new FileStream(res, FileMode.Create);

            //return new FileStreamResult(fileStream, mimeType)
            //{
            //    FileDownloadName = fileName
            //};

            // отладка
            return StatusCode(200, res);
        }
    }
}
