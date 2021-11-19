using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LetsJump.DataAccess.Tools
{
    public static class FileValidation
    {
        public static void CheckDir(params string[] filePath)
        { 
            foreach (string path in filePath)
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
        }
        public static (bool IsValid, string FileName, string MediaDirectory, string Reason) Check(IFormFile file, int MaxSizeInMB, MediaType[] mediaTypes)
        {
            var fileSize = ConvertBytesToMegabytes(file.Length);
            if (fileSize <= MaxSizeInMB)
            {
                var supportedMediaTypes = string.Join(",", mediaTypes);
                supportedMediaTypes = supportedMediaTypes.ToLower();
                var fileType = file.ContentType.Split('/')[0];

                if (supportedMediaTypes.Contains(fileType))
                {
                    return (true, Path.GetRandomFileName(), fileType, null);
                }
                else
                {
                    //false media type not supported
                    return (false, null, null, "(" + file.FileName + ") Media type is not supported");
                }
            }
            else
            {
                //false max size exceeded
                return (false, null, null, "(" + file.FileName + ") Media size is greater than 55M");
            }
        }
        public enum MediaType
        {
            Image,
            Video,
            Executable,
            Audio
        }

        static double ConvertBytesToMegabytes(long bytes)
        {
            return (bytes / 1024f) / 1024f;
        }
    }
}
