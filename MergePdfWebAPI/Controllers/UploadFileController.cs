﻿using Microsoft.AspNetCore.Http;
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
                    var filePath = Path.GetTempPath();
                    var fileName = filePath + formFile.FileName;

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

            var mimeType = "application/pdf";

            var fileStream = new FileStream(res, FileMode.Open);

            return new Microsoft.AspNetCore.Mvc.FileStreamResult(fileStream, mimeType);
        }
    }
}
