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
            return "result get CountPage";
        }

        [HttpPost]
        public async Task<IActionResult> PageCounting()
        {
            IFormFile file = Request.Form.Files.FirstOrDefault();

            if (file != null) 
            {
                try 
                {
                    var filePath = Path.GetTempPath();
                    var fileName = filePath + file.FileName;

                    using (var stream = System.IO.File.Create(fileName))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // запуск библиотеки
                    Merge merge = new Merge();
                    int res = merge.CountPages(fileName);

                    return StatusCode(200, res);
                }
                catch (Exception err) 
                {
                    return StatusCode(500, err.Message);
                }
            }
            else
            {
                return BadRequest("file is null");
            }
        }
    }
}
