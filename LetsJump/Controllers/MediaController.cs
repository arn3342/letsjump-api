using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using LetsJump.DataAccess.Tools;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using static LetsJump.DataAccess.Tools.FileValidation;

namespace LetsJump.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        public class Media
        {
            public string base64 { get; set; }
        }
        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<ActionResult<List<string>>> Upload(Media[] files)
        {
            List<string> mediaURI = new List<string>();
            //string test = MyHttpContext.AppBaseUrl;
            var baseUrl = $"{this.Request.Scheme}://{this.Request.Host.Value.ToString()}{this.Request.PathBase.Value.ToString()}";
            foreach (var file in files)
            {
                string storageDir = Directory.GetCurrentDirectory() + "\\storage";
                string uploadDir = Directory.GetCurrentDirectory() + "\\storage\\uploads";

                FileValidation.CheckDir(storageDir, uploadDir);
                var dir = Directory.Exists(uploadDir) ? Directory.CreateDirectory(uploadDir) : null;
                var fileName = Path.GetRandomFileName() + ".jpg";
                string filePath = (Path.Combine(uploadDir, fileName));
                await System.IO.File.WriteAllBytesAsync(filePath, Convert.FromBase64String(file.base64));
                mediaURI.Add(baseUrl + "/storage/uploads/" + fileName);

                return mediaURI;
            }
            return mediaURI.Count > 0 ? StatusCode(200, mediaURI) : StatusCode(413, "Media not supported or media size is greater than 55M.");
        }        
    }
}