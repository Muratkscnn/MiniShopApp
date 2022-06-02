using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniShopApp.Core
{
    public class AlertMessage
    {
        //Uygulamamızda çeşitli durumlarda ihtiyaç duyduğumuz uyarı mesajları için kullanılacak
        public string Title { get; set; }
        public string Message { get; set; }//Uyarı mesajımız
        public string AlertType { get; set; }//Uyarı tipimiz
    }
}
