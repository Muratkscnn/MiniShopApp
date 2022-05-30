using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MiniShopApp.WebUI.EmailServices;
using MiniShopApp.WebUI.Identity;
using MiniShopApp.WebUI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniShopApp.WebUI.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private IEmailSender _emailSender;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login(string ReturnUrl=null) 
        {
            return View(
                   new LoginModel()
                   {
                       ReturnUrl = ReturnUrl
                   }
                );
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user==null)
            {
                ModelState.AddModelError("", "Böyle Bir kullanıcı bulunamadı");
                return View(model);
            }
            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError("", "Hesabınız onaylı değil! Lütfen mail adresinizi kontrol ederek onay işlemlerini tamamlayınız.");
                return View(model);
            }
            var result = await _signInManager.PasswordSignInAsync(user,model.Password,true,false);
            if (result.Succeeded)
            {
                return Redirect(model.ReturnUrl ?? "~/");
            }
            ModelState.AddModelError("", "Kullanıcı adı ya da parola hatalı!");
            return View(model); 
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("~/");
        }

        public IActionResult Register() 
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = new User()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)//Create işlemi başarılıysa
            {
                //Mail onay işlemleri
                //TOKEN işlemleri
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                
                var url = Url.Action("ConfirmEmail", "Account", new
                {
                    userId = user.Id,
                    token = code
                });
                //mail gönderme işlemleri
                await _emailSender.SendEmailAsync(model.Email, "MiniShopApp Hesap Onaylama", $"Lütfen email hesabınızı onaylamak için <a href='https://localhost:5001{url}'>tıklayınız.</a>");
                return RedirectToAction("Login", "Account");
            }
            CreateMessage("Bir sorun oluştu, lütfen tekrar deneyiniz", "danger");
            return View(model);
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId==null || token == null)
            {
                return View();
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user!=null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    CreateMessage("Hesabınız onaylanmıştır","success");
                    return View();
                }
            }
            CreateMessage("Hesabınız onaylanamadı. Lütfen bilgileri kontrol ederek, yeniden deneyiniz!","warning");
            return View();
        }


        private void CreateMessage(string message, string alertType)
        {
            var msg = new AlertMessage()
            {
                Message = message,
                AlertType = alertType
            };
            TempData["Message"] = JsonConvert.SerializeObject(msg);
        }
    }
}
