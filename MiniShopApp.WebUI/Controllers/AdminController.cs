using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MiniShopApp.Business.Abstract;
using MiniShopApp.Business.Concrete;
using MiniShopApp.Core;
using MiniShopApp.Entity;
using MiniShopApp.WebUI.Identity;
using MiniShopApp.WebUI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MiniShopApp.WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService; 
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<User> _userManager;

        public AdminController(IProductService productService, ICategoryService categoryService, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _productService = productService;
            _categoryService = categoryService;
            _roleManager = roleManager;
            _userManager = userManager; 
        }

        public IActionResult UserList()
        {
            return View(_userManager.Users);
        }
        public IActionResult UserCreate()
        {
            var roles = _roleManager.Roles.Select(i => i.Name);
            ViewBag.Roles = roles;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UserCreate(UserDetailsModel model, string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                var user = new User()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserName = model.UserName,
                    Email = model.Email,
                    EmailConfirmed = model.EmailConfirmed
                };
                var result = await _userManager.CreateAsync(user, "Qwe123.");
                if (result.Succeeded)
                {
                    selectedRoles = selectedRoles ?? new string[] { };
                    await _userManager.AddToRolesAsync(user, selectedRoles);
                    return Redirect("~/admin/user/list");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                var roles = _roleManager.Roles.Select(i => i.Name);
                ViewBag.Roles = roles;
                return View(model);
            }
            var roles2 = _roleManager.Roles.Select(i => i.Name);
            ViewBag.Roles = roles2;
            return View(model);
        }
        public async Task<IActionResult> UserEdit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user!=null)
            {
                var selectedRoles = await _userManager.GetRolesAsync(user);
                var roles = _roleManager.Roles.Select(i => i.Name);
                ViewBag.Roles = roles;
                return View(new UserDetailsModel()
                {
                    UserId=user.Id,
                    FirstName=user.FirstName,
                    LastName=user.LastName,
                    UserName=user.UserName,
                    Email=user.Email,
                    EmailConfirmed=user.EmailConfirmed,
                    SelectedRoles=selectedRoles
                });
            }
            return Redirect("~/admin/user/list");
        }
        [HttpPost]
        public async Task<IActionResult> UserEdit(UserDetailsModel model, string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user!=null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.UserName = model.UserName;
                    user.Email = model.Email;
                    user.EmailConfirmed = model.EmailConfirmed;

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        var userRoles = await _userManager.GetRolesAsync(user);
                        selectedRoles = selectedRoles ?? new string[] { };
                        await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles).ToArray<string>());
                        await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles).ToArray<string>());
                        return Redirect("~/admin/user/list");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    var roles = _roleManager.Roles.Select(i => i.Name);
                    ViewBag.Roles = roles;
                    return View(model);
                }
                ModelState.AddModelError("", "Böyle bir kullanıcı yok!");
                var roles2 = _roleManager.Roles.Select(i => i.Name);
                ViewBag.Roles = roles2;
                return View(model);
            }
            var roles3 = _roleManager.Roles.Select(i => i.Name);
            ViewBag.Roles = roles3;
            return View(model);
        }
        public IActionResult RoleList()
        {
            return View(_roleManager.Roles);
        }


        public IActionResult RoleCreate()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RoleCreate(RoleModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleManager.CreateAsync(
                    new IdentityRole(model.Name)
                    );
                if (result.Succeeded)
                {
                    return RedirectToAction("RoleList");
                }

                //Geri kalan kısmına yarın devam edeceğiz.
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);  
        }

        public async Task<IActionResult> RoleEdit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            var members = new List<User>();
            var nonMembers = new List<User>();

            foreach (var user in _userManager.Users)
            {
                //if (await _userManager.IsInRoleAsync(user,role.Name))
                //{
                //    members.Add(user);
                //}
                //else
                //{
                //    nonMembers.Add(user);
                //}

                var list = await _userManager.IsInRoleAsync(user, role.Name) ? members : nonMembers;
                list.Add(user);
            }
            var model = new RoleDetails()
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> RoleEdit(RoleEditModel model)
        {
            if (ModelState.IsValid)
            {
                foreach (var userId in model.IdsToAdd ?? new string[] { })
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user!=null)
                    {
                        var result = await _userManager.AddToRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                }

                foreach (var userId in model.IdsToDelete ?? new string[] { })
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        var result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                }
            }
            return Redirect("/admin/role/" + model.RoleId);
        }
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult ProductList()
        {
            return View(_productService.GetAll());
        }

        public IActionResult ProductCreate()
        {
            ViewBag.Categories = _categoryService.GetAll();
            return View();
        }
        [HttpPost]
        public IActionResult ProductCreate(ProductModel model, int[] categoryIds, IFormFile file)
        {
            if (ModelState.IsValid && categoryIds.Length>0 && file!=null)
            {
                var url = JobManager.MakeUrl(model.Name);
                model.ImageUrl = JobManager.UploadImage(file, url);
                var product = new Product()
                {
                    Name = model.Name,
                    Url = url,
                    Price = model.Price,
                    Description = model.Description,
                    ImageUrl = model.ImageUrl,
                    IsApproved = model.IsApproved,
                    IsHome = model.IsHome 
                };
                _productService.Create(product, categoryIds);

                TempData["Message"]=JobManager.CreateMessage("Ürün Ekleme","Ürün ekleme işlemi başarıyla tamamlanmıştır.", "success");
                return RedirectToAction("ProductList");
            }
            //İşler yolunda gitmediyse

            if (categoryIds.Length>0)
            {
                model.SelectedCategories = categoryIds.Select(catId => new Category()
                {
                    CategoryId = catId
                }).ToList();
            }
            else
            {
                ViewBag.CategoryMessage = "Lütfen en az bir kategori seçiniz!";
            }

            if (file==null)
            {
                ViewBag.ImageMessage = "Lütfen bir resim seçiniz!";
            }
            ViewBag.Categories = _categoryService.GetAll();
            return View(model);

        }
        public IActionResult ProductEdit(int? id)
        {
            var entity = _productService.GetByIdWithCategories((int)id);
            var model = new ProductModel()
            {
                ProductId = entity.ProductId,
                Name = entity.Name,
                Url = entity.Url,
                Price = entity.Price,
                Description = entity.Description,
                ImageUrl = entity.ImageUrl,
                IsApproved = entity.IsApproved,
                IsHome = entity.IsHome,
                SelectedCategories = entity
                    .ProductCategories
                    .Select(i => i.Category)
                    .ToList()
            };
            ViewBag.Categories = _categoryService.GetAll();
            return View(model);
        }
        [HttpPost]
        public IActionResult ProductEdit(ProductModel model, int[] categoryIds)
        {
            //Aslında üçüncü bir parametremiz de olacak. (Create'te de olacak)
            //IFormFile tipinde resim.
            var entity = _productService.GetById(model.ProductId);
            entity.Name = model.Name;
            entity.Price = model.Price;
            entity.Url = model.Url;
            entity.Description = model.Description;
            entity.IsApproved = model.IsApproved;
            entity.IsHome = model.IsHome;
            entity.ImageUrl = model.ImageUrl;
            _productService.Update(entity, categoryIds);
            return RedirectToAction("ProductList");
        }

        public IActionResult ProductDelete(int productId)
        {
            var entity = _productService.GetById(productId);
            _productService.Delete(entity);
            TempData["Message"] = JobManager.CreateMessage("SİLME", $"{entity.Name} adlı ürün silindi!", "danger");
            return RedirectToAction("ProductList");
        }


    }
}
