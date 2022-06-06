using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniShopApp.Core
{
    public static class JobManager
    {
        //Burada genel olarak tanımlamak istediğimiz, her yerde ihtiyaç duyma ihtimalimiz olan metotlar yazılacak.
        public static string MakeUrl(string url)
        {
            //Kendisine gelen string değerin içindeki;
            //1) Türkçe karakterlerin yerine latin alfabesindeki karşılıklarını koyacak
            //2) Boşlukların yerine - işareti koyacak
            //3) Nokta (.)'ları kaldıracak.

            url = url.Replace("I", "i");
            url = url.Replace("İ", "i");
            url = url.Replace("ı", "i");
            
            url = url.ToLower();

            url=url.Replace("ö", "o");
            url=url.Replace("ğ", "g");
            url=url.Replace("ş", "s");
            url=url.Replace("ü", "u");
            url=url.Replace("ç", "c");
            
            url=url.Replace(" ", "-");
            url=url.Replace(".", "");

            return url;
        }
        public static string UploadImage(IFormFile file, string url)
        {
            var extension = Path.GetExtension(file.FileName);
            var randomName = $"{url}-{Guid.NewGuid()}{extension}";
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            return randomName;
        }

        public static string CreateMessage(string title, string message, string alertType)
        {
            var msg = new AlertMessage()
            {
                Title = title,
                Message = message,
                AlertType = alertType
            };
            return  JsonConvert.SerializeObject(msg);
        }
    }
}
