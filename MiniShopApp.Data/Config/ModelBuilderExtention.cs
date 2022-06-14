using Microsoft.EntityFrameworkCore;
using MiniShopApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniShopApp.Data.Config
{
    public static class ModelBuilderExtention
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category() { CategoryId = 1, Name = "Telefon", Description = "Ürünlerimiz son teknoloji ile üretilmektedir.", Url = "telefon" },
                new Category() { CategoryId = 2, Name = "Bilgisayar", Description = "Ürünlerimiz son teknoloji ile üretilmektedir.", Url = "bilgisayar" },
                new Category() { CategoryId = 3, Name = "Elektronik", Description = "Ürünlerimiz son teknoloji ile üretilmektedir.", Url = "elektronik" },
                new Category() { CategoryId = 4, Name = "Beyaz Eşya", Description = "Ürünlerimiz son teknoloji ile üretilmektedir.", Url = "beyaz-esya" }
                );

            modelBuilder.Entity<Product>().HasData(
                new Product {ProductId=1, Name = "Samsung S10", Price = 15000, Description = "Bu telefon çok iyi bir telefon.", Url = "samsung-s10", IsApproved = true, IsHome = true, ImageUrl = "1.png" },
                new Product {ProductId=2, Name = "Samsung S11", Price = 16000, Description = "Bu telefon çok iyi bir telefon.", Url = "samsung-s11", IsApproved = true, IsHome = true, ImageUrl = "2.png" },
                new Product {ProductId=3, Name = "Samsung S12", Price = 17000, Description = "Bu telefon çok iyi bir telefon.", Url = "samsung-s12", IsApproved = true, IsHome = true, ImageUrl = "3.png" },
                new Product {ProductId=4, Name = "Samsung S20", Price = 18000, Description = "Bu telefon çok iyi bir telefon.", Url = "samsung-s20", IsApproved = true, ImageUrl = "4.png" },
                new Product {ProductId=5, Name = "Xaomi Redmi 9 Pro", Price = 13000, Description = "Bu telefon çok iyi bir telefon.", Url = "xaomi-redmi-9-pro", IsApproved = true, ImageUrl = "5.png" },
                new Product {ProductId=6, Name = "Xaomi Redmi 10 Pro", Price = 14000, Description = "Bu telefon çok iyi bir telefon.", Url = "xaomi-redmi-10-pro", IsApproved = true, ImageUrl = "6.png" },
                new Product {ProductId=7, Name = "Xaomi Redmi 11 Pro", Price = 15000, Description = "Bu telefon çok iyi bir telefon.", Url = "xaomi-redmi-11-pro", IsApproved = true, ImageUrl = "7.png" },
                new Product {ProductId=8, Name = "Iphone XR", Price = 12000, Description = "Bu telefon çok iyi bir telefon.", Url = "iphone-xr", IsApproved = true, ImageUrl = "8.png" },
                new Product {ProductId=9, Name = "Iphone 11", Price = 13000, Description = "Bu telefon çok iyi bir telefon.", Url = "iphone-11", IsApproved = true, ImageUrl = "9.png" },
                new Product {ProductId=10, Name = "Iphone 12", Price = 14000, Description = "Bu telefon çok iyi bir telefon.", Url = "iphone-12", IsApproved = true, ImageUrl = "10.png" },
                new Product {ProductId=11, Name = "Iphone 13", Price = 15000, Description = "Bu telefon çok iyi bir telefon.", Url = "iphone-13", IsApproved = true, ImageUrl = "11.png" },
                new Product {ProductId=12, Name = "Iphone 13 Max", Price = 16000, Description = "Bu telefon çok iyi bir telefon.", Url = "iphone-13-max", IsApproved = true, ImageUrl = "12.png" },
                new Product {ProductId=13, Name = "Huawei Mate 20 Pro", Price = 20000, Description = "Bu telefon çok iyi bir telefon.", Url = "huawei-mate-20-pro", IsApproved = true, ImageUrl = "13.png" }
                );

            modelBuilder.Entity<ProductCategory>().HasData(
                new ProductCategory { ProductId=1, CategoryId=1},
                new ProductCategory { ProductId=1, CategoryId=2},
                new ProductCategory { ProductId=2, CategoryId=1},
                new ProductCategory { ProductId=2, CategoryId=2},
                new ProductCategory { ProductId=3, CategoryId=3},
                new ProductCategory { ProductId=3, CategoryId=4},
                new ProductCategory { ProductId=5, CategoryId=3},
                new ProductCategory { ProductId=6, CategoryId=1},
                new ProductCategory { ProductId=7, CategoryId=1},
                new ProductCategory { ProductId=8, CategoryId=1},
                new ProductCategory { ProductId=9, CategoryId=1},
                new ProductCategory { ProductId=10, CategoryId=1},
                new ProductCategory { ProductId=11, CategoryId=1},
                new ProductCategory { ProductId=12, CategoryId=1},
                new ProductCategory { ProductId=13, CategoryId=1}
                );
        }
    }
}
