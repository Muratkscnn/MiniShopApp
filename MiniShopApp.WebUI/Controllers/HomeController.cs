using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniShopApp.Business.Abstract;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MiniShopApp.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private IProductService _productService;
        public HomeController(IProductService productService)
        {
            _productService = productService;
        }
        public IActionResult Index()
        {
            return View(_productService.GetHomePageProducts());
        }
    }
}
