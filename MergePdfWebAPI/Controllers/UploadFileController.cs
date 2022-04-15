using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using System.Diagnostics;
using System.IO;
using MergePdfLib;

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
            HttpFileCollection uploadFiles = httpRequest.Files;
            var docfiles = new List<string>();

            if (httpRequest.Files.Count > 0)
            {
                for (int i = 0; i < uploadFiles.Count; i++)
                {
                    HttpPostedFile postedFile = uploadFiles[i];
                    var filePath = HttpContext.Current.Server.MapPath("~/" + postedFile.FileName);
                    postedFile.SaveAs(filePath);
                    docfiles.Add(filePath);
                }

                // запуск библиотеки
                Merge merge = new Merge();
                string res = merge.MergeDocs(docfiles);
                result = Request.CreateResponse(HttpStatusCode.Created, res);
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            return result;
        }
    }
}