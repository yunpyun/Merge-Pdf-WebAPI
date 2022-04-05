using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using System.Diagnostics;
using System.IO;

namespace MergePdfWebAPI.Controllers
{
    public class UploadFileController : ApiController
    {
        public string Get()
        {
            return "result get";
        }

        public HttpResponseMessage Post()
        {
            HttpResponseMessage result;
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                var docfiles = new List<string>();
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    var filePath = HttpContext.Current.Server.MapPath("~/" + postedFile.FileName);
                    postedFile.SaveAs(filePath);
                    docfiles.Add(filePath);
                }
                result = Request.CreateResponse(HttpStatusCode.Created, docfiles);

                // запуск консольного приложения
                Process myProcess = new Process();
                myProcess.StartInfo.RedirectStandardOutput = true;
                myProcess.StartInfo.FileName = @"C:\Users\Выймова Елена\Desktop\Материалы для работы\ПП\GitHub Repositories\Merge-PDF\MergePdf\bin\Debug\MergePdf.exe";
                myProcess.StartInfo.Arguments = docfiles[0];
                myProcess.StartInfo.CreateNoWindow = true;
                myProcess.StartInfo.UseShellExecute = false;
                myProcess.Start();

                string output = myProcess.StandardOutput.ReadToEnd();
                myProcess.WaitForExit();
                var resultProcess = myProcess.ExitCode.ToString();
                result = Request.CreateResponse(HttpStatusCode.Created, resultProcess);

            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            return result;
        }
    }
}