using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MiniShopApp.Business.Abstract;
using MiniShopApp.Core;
using MiniShopApp.Entity;
using MiniShopApp.WebUI.Identity;
using MiniShopApp.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderItem = MiniShopApp.Entity.OrderItem;

namespace MiniShopApp.WebUI.Controllers
{
    [Authorize]
    public class CardController : Controller
    {
        private ICardService _cardService;
        private UserManager<User> _userManager;
        private IOrderService _orderService;

        public CardController(ICardService cardService, UserManager<User> userManager, IOrderService orderService)
        {
            _cardService = cardService;
            _userManager = userManager;
            _orderService = orderService;
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

        public IActionResult GetOrders()
        {
            var userId = _userManager.GetUserId(User);
            var orders = _orderService.GetOrders(userId);
            var orderListModel = new List<OrderListModel>();
            OrderListModel orderModel;
            foreach (var order in orders)
            {
                orderModel = new OrderListModel();
                orderModel.Id = order.Id;
                orderModel.FirstName = order.FirstName;
                orderModel.LastName = order.LastName;
                orderModel.OrderNumber = order.OrderNumber;
                orderModel.Adress = order.Adress;
                orderModel.OrderDate = order.OrderDate;
                orderModel.City = order.City;
                orderModel.Phone = order.Phone;
                orderModel.Email = order.Email;
                orderModel.OrderState = order.OrderState;
                orderModel.PaymentType = order.PaymentType;
                orderModel.OrderItems = order.OrderItems.Select(x => new OrderItemModel()
                {
                    OrtemItemId = x.Id,
                    Name = x.Product.Name,
                    Price = (double)x.Price,
                    Quantity=x.Quantity,
                    ImageUrl=x.Product.ImageUrl
                }).ToList();
                orderListModel.Add(orderModel);
            }
            

            return View("orders",orderListModel);
        }
       
        [HttpPost]
        public IActionResult AddToCard(int ProductId,int Quantity)
        {
            var userId = _userManager.GetUserId(User);
            _cardService.AddToCard(userId,ProductId,Quantity);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult DeleteFromCard(int productId)
        {
            var userId = _userManager.GetUserId(User);
            _cardService.DeleteFromCard(userId, productId);
            return RedirectToAction("Index");
            
        }
        [HttpGet]
        public IActionResult CheckOut()
        {
            var userId = _userManager.GetUserId(User);
            var card = _cardService.GetCardByUserId(userId);
            OrderModel orderModel = new OrderModel();
            orderModel.CardModel = new CardModel()
            {
                CardId = card.Id,
                CardItems = card.CardItems.Select(x => new CardItemModel()
                {
                    CardItemId = x.Id,
                    ProductId = x.ProductId,
                    ProductName = x.Product.Name,
                    Price = (double)x.Product.Price,
                    ImageUrl = x.Product.ImageUrl,
                    Quantity = x.Quantity
                }).ToList()
            };
            
            return View(orderModel);
        }
        public IActionResult CheckOut(OrderModel orderModel)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                var card = _cardService.GetCardByUserId(userId);
                orderModel.CardModel = new CardModel()
                {
                    CardId = card.Id,
                    CardItems = card.CardItems.Select(x=> new CardItemModel()
                    {
                        CardItemId = x.Id,
                        ProductId = x.ProductId,
                        ProductName = x.Product.Name,
                        Price = (double)x.Product.Price,
                        ImageUrl = x.Product.ImageUrl,
                        Quantity = x.Quantity
                    }).ToList()
                };
                //Ödeme işlemine başla
                var payment = PaymentProcess(orderModel);
                if (payment.Status=="success")
                {
                    SaveOrder(orderModel,payment,userId);
                    _cardService.ClearCard(userId);
                    TempData["Message"] = JobManager.CreateMessage("Bilgi", "Ödemeniz gerçekleşti", "success");
                    return Redirect("~/");
                }
                else
                {
                    TempData["Message"] = JobManager.CreateMessage("Dikkat", payment.ErrorMessage, "danger");
                }
            }
            return View(orderModel);
        }

        private void SaveOrder(OrderModel orderModel, Payment payment, string userId)
        {
            var order = new Order()
            {
                OrderNumber = new Random().Next(100000000, 999999999).ToString(),
                OrderState = EnumOrderState.Completed,
                PaymentType = EnumPaymentType.CreditCard,
                PaymentId = payment.PaymentId,
                ConversationId = payment.ConversationId,
                OrderDate = DateTime.Now,
                FirstName = orderModel.FirstName,
                LastName = orderModel.LastName,
                UserId = userId,
                Adress = orderModel.Adress,
                City = orderModel.City,
                Phone = orderModel.Phone,
                Email = orderModel.Email,
                Note = orderModel.Note,

            };
            order.OrderItems = new List<OrderItem>();
            foreach (var item in orderModel.CardModel.CardItems)
            {
                var orderItem = new OrderItem()
                {
                    Price = item.Price,
                    Quantity = item.Quantity,
                    ProductId = item.ProductId
                };
                order.OrderItems.Add(orderItem);
            }
            _orderService.Create(order);
        }

        private Payment PaymentProcess(OrderModel orderModel)
        {
            Options options = new Options();
            options.ApiKey = "sandbox-kOoqgIAufIzyIjfnRgeP7KtlA7oOOD8f";
            options.SecretKey = "sandbox-gtXjzmxs7QJhBhjfecmis0TPwEdKIWgH";
            options.BaseUrl = "https://sandbox-api.iyzipay.com";

            CreatePaymentRequest request = new CreatePaymentRequest();
            request.Locale = Locale.TR.ToString();
            request.ConversationId = new Random().Next(100000000,999999999).ToString();
            request.Price = orderModel.CardModel.TotalPrice().ToString();
            request.PaidPrice = orderModel.CardModel.TotalPrice().ToString();
            request.Currency = Currency.TRY.ToString();
            request.Installment = 1;
            request.BasketId = "B67832";
            request.PaymentChannel = PaymentChannel.WEB.ToString();
            request.PaymentGroup = PaymentGroup.PRODUCT.ToString();

            PaymentCard paymentCard = new PaymentCard();
            paymentCard.CardHolderName = orderModel.CardName;
            paymentCard.CardNumber = orderModel.CardNumber;
            paymentCard.ExpireMonth = orderModel.ExpirationMonth;
            paymentCard.ExpireYear = orderModel.ExpirationYear;
            paymentCard.Cvc = orderModel.Cvc;
            paymentCard.RegisterCard = 0;
            request.PaymentCard = paymentCard;

            Buyer buyer = new Buyer();
            buyer.Id = "BY789";
            buyer.Name = orderModel.FirstName;
            buyer.Surname = orderModel.LastName;
            buyer.GsmNumber = orderModel.Phone;
            buyer.Email = orderModel.Email;
            buyer.IdentityNumber = "74300864791";
            buyer.LastLoginDate = "2015-10-05 12:43:35";
            buyer.RegistrationDate = "2013-04-21 15:12:09";
            buyer.RegistrationAddress = orderModel.Adress;
            buyer.Ip = "85.34.78.112";
            buyer.City = orderModel.City;
            buyer.Country = "Turkey";
            buyer.ZipCode = "34732";
            request.Buyer = buyer;

            Address shippingAddress = new Address();
            shippingAddress.ContactName = "Jane Doe";
            shippingAddress.City = "Istanbul";
            shippingAddress.Country = "Turkey";
            shippingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            shippingAddress.ZipCode = "34742";
            request.ShippingAddress = shippingAddress;

            Address billingAddress = new Address();
            billingAddress.ContactName = "Murat Kuşcan";
            billingAddress.City = "Istanbul";
            billingAddress.Country = "Turkey";
            billingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            billingAddress.ZipCode = "34742";
            request.BillingAddress = billingAddress;

            List<BasketItem> basketItems = new List<BasketItem>();

            foreach (var item in orderModel.CardModel.CardItems)
            {
                BasketItem BasketItem = new BasketItem();
                BasketItem.Id = item.ProductId.ToString();
                BasketItem.Name = item.ProductName;
                BasketItem.Category1 = "General";
                BasketItem.ItemType = BasketItemType.PHYSICAL.ToString();
                BasketItem.Price = (item.Quantity*item.Price).ToString();
                basketItems.Add(BasketItem);

            }


            request.BasketItems = basketItems;

            Payment payment = Payment.Create(request, options);
            return payment;
        }

        public static bool Luhn(string digits)
        {
            return digits.All(char.IsDigit) && digits.Reverse()
                .Select(c => c - 48)
                .Select((thisNum, i) => i % 2 == 0
                    ? thisNum
                    : ((thisNum *= 2) > 9 ? thisNum - 9 : thisNum)
                ).Sum() % 10 == 0;
        }

    }
}
