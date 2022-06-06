using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MiniShopApp.Business.Abstract;
using MiniShopApp.WebUI.Identity;
using MiniShopApp.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniShopApp.WebUI.Controllers
{
    [Authorize]
    public class CardController : Controller
    {
        private ICardService _cardService;
        private UserManager<User> _userManager;

        public CardController(ICardService cardService, UserManager<User> userManager)
        {
            _cardService = cardService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var card = _cardService.GetCardByUserId(_userManager.GetUserId(User));
            var model = new CardModel()
            {
                CardId = card.Id,
                CardItems = card.CardItems.Select(x => new CardItemModel()
                {
                    CardItemId=x.Id,
                    ProductName=x.Product.Name,
                    ProductId=x.ProductId,
                    Price=(double)x.Product.Price,
                    ImageUrl=x.Product.ImageUrl,
                    Quantity=x.Quantity
                }).ToList()

            };
            return View(model);
        }
    }
}
