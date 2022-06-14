using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MiniShopApp.Business.Abstract;
using MiniShopApp.Core;
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
        private ICardService _cardService;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IEmailSender emailSender, ICardService cardService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _cardService = cardService;
        }

        public IActionResult AccessDenied()
        {
            return View();
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
                ModelState.AddModelError("", "Böyle bir kullanıcı bulunamadı!");
                return View(model);
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError("", "Hesabınız onaylı değil! Lütfen mail adresiniz kontrol ederek, onay işlemlerini tamamlayınız.");
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

            TempData["Message"]= JobManager.CreateMessage("HATA","Bir sorun oluştu, lütfen tekrar deneyiniz", "danger");
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
                    //Card oluşturacağız
                    _cardService.InitializeCard(userId);
                    TempData["Message"]=JobManager.CreateMessage("","Hesabınız onaylanmıştır","success");
                    return View();
                }
            }

            TempData["Message"]=JobManager.CreateMessage("","Hesabınız onaylanamadı. Lütfen bilgileri kontrol ederek, yeniden deneyiniz!","warning");
            return View();
        }


        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("~/");
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (String.IsNullOrEmpty(email))
            {
                TempData["Message"]=JobManager.CreateMessage("","Lütfen email adresini yazınız!", "warning");
                return View();
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user==null)
            {
                TempData["Message"]=JobManager.CreateMessage("","Böyle bir mail adresi bulunamadı. Lütfen kontrol ederek yeniden deneyiniz.", "warning");
                return View();
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var url = Url.Action("ResetPassword", "Account", new
            {
                userId=user.Id,
                token = code
            });

            await _emailSender.SendEmailAsync(
                email,
                "MiniShopApp Reset Password",
                $"Parolanızı yeniden belirlemek için <a href='https://localhost:5001{url}'>tıklayınız.</a>"
                );
            TempData["Message"]=JobManager.CreateMessage("","Parola değiştirmeniz için gerekli link mail adresinize yollanmıştır, lütfen kontrol ederek yönergeleri takip ediniz!", "warning");
            return Redirect("~/");
        }

        public IActionResult ResetPassword(string userId, string token)
        {
            if (userId == null || token == null)
            {
                TempData["Message"]=JobManager.CreateMessage("","Geçersiz işlem!", "danger");
                return RedirectToAction("Index", "Home");
            }
            var model = new ResetPasswordModel() { Token = token };
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user==null)
            {
                TempData["Message"]=JobManager.CreateMessage("","Böyle bir kullanıcı bulunamadı!", "warning");
                return Redirect("~/");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                TempData["Message"]=JobManager.CreateMessage("","Şifre değiştirme işleminiz başarıyla gerçekleşti.", "success");
                return RedirectToAction("Login");
            }
            TempData["Message"]=JobManager.CreateMessage("","İşlem başarısız oldu, lütfen daha sonra tekrar deneyiniz!", "danger");
            return View(model);
        }

    }
}
