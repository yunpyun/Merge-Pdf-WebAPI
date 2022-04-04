﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Net.Http;
using System.Web.Http;

namespace MergePdfWebAPI.Controllers
{
    public class UploadFileController : ApiController
    {
        [Route("api/docfile")]
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
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            return result;
        }
    }
}