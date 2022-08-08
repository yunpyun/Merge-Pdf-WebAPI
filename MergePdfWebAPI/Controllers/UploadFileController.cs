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
            return "result get UploadFile";
        }

        [HttpPost]
        public async Task<IActionResult> Upload()
        {
            List<IFormFile> files = Request.Form.Files.ToList();

            if (files.Count > 0)
            {
                try
                {
                    var docfiles = new List<string>();

                    foreach (var formFile in files)
                    {
                        if (formFile.Length > 0)
                        {
                            var filePath = Path.GetTempPath();
                            var fileName = filePath + Guid.NewGuid().ToString() + "-" + formFile.FileName;

                            using (var stream = System.IO.File.Create(fileName))
                            {
                                await formFile.CopyToAsync(stream);
                            }

                            docfiles.Add(fileName);
                        }
                    }

                    // запуск библиотеки
                    Merge merge = new Merge();
                    string res = merge.MergeDocs(docfiles);

                    foreach (var onefile in docfiles)
                    {
                        FileInfo fileInf = new FileInfo(onefile);

                        if (fileInf.Exists)
                        {
                            fileInf.Delete();
                        }
                    }

                    var mimeType = "application/pdf";

                    var fileStream = new FileStream(res, FileMode.Open, FileAccess.Read, FileShare.None, 
                        4096, FileOptions.DeleteOnClose);

                    return new Microsoft.AspNetCore.Mvc.FileStreamResult(fileStream, mimeType);
                }
                catch (Exception err)
                {
                    return StatusCode(500, err.Message);
                }
            }
            else
            {
                return NotFound("Входные файлы не найдены");
            }

        }
    }
}
