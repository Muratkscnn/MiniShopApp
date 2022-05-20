using Microsoft.EntityFrameworkCore;
using MiniShopApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniShopApp.Data.Concrete.EfCore
{
    public static class SeedDatabase
    {
        public static void Seed()
        {
            var context = new MiniShopContext();
            if (context.Database.GetPendingMigrations().Count()==0)
            {
                if (context.Categories.Count()==0)
                {
                    context.Categories.AddRange(Categories);
                }
                if (context.Products.Count()==0)
                {
                    context.Products.AddRange(Products);
                }
                if (context.ProductCategories.Count()==0)
                {
                    context.ProductCategories.AddRange(ProductCategories);
                }
                context.SaveChanges();
            }
        }

        private static Category[] Categories =
        {
            new Category() {Name="Telefon",Description="Ürünlerimiz son teknoloji ile üretilmektedir.",Url="telefon"},
            new Category() {Name="Bilgisayar",Description="Ürünlerimiz son teknoloji ile üretilmektedir.",Url="bilgisayar"},
            new Category() {Name="Elektronik",Description="Ürünlerimiz son teknoloji ile üretilmektedir.",Url="elektronik"},
            new Category() {Name="Beyaz Eşya",Description="Ürünlerimiz son teknoloji ile üretilmektedir.",Url="beyaz-esya"}
        };

        private static Product[] Products =
        {
            new Product {Name="Samsung S10", Price=15000, Description="Bu telefon çok iyi bir telefon.", Url="samsung-s10", IsApproved=true, IsHome=true},
            new Product {Name="Samsung S11", Price=16000, Description="Bu telefon çok iyi bir telefon.", Url="samsung-s11", IsApproved=true, IsHome=true},
            new Product {Name="Samsung S12", Price=17000, Description="Bu telefon çok iyi bir telefon.", Url="samsung-s12", IsApproved=true, IsHome=true},
            new Product {Name="Samsung S20", Price=18000, Description="Bu telefon çok iyi bir telefon.", Url="samsung-s20", IsApproved=true, IsHome=true},

            new Product {Name="Xaomi Redmi 9 Pro", Price=13000, Description="Bu telefon çok iyi bir telefon.", Url="xaomi-redmi-9-pro", IsApproved=true, IsHome=true},
            new Product {Name="Xaomi Redmi 10 Pro", Price=14000, Description="Bu telefon çok iyi bir telefon.", Url="xaomi-redmi-10-pro", IsApproved=true, IsHome=true},
            new Product {Name="Xaomi Redmi 11 Pro", Price=15000, Description="Bu telefon çok iyi bir telefon.", Url="xaomi-redmi-11-pro", IsApproved=true, IsHome=true},

            new Product {Name="Iphone XR", Price=12000, Description="Bu telefon çok iyi bir telefon.", Url="iphone-xr", IsApproved=true, IsHome=true},
            new Product {Name="Iphone 11", Price=13000, Description="Bu telefon çok iyi bir telefon.", Url="iphone-11", IsApproved=true, IsHome=true},
            new Product {Name="Iphone 12", Price=14000, Description="Bu telefon çok iyi bir telefon.", Url="iphone-12", IsApproved=true, IsHome=true},
            new Product {Name="Iphone 13", Price=15000, Description="Bu telefon çok iyi bir telefon.", Url="iphone-13", IsApproved=true, IsHome=true},
            new Product {Name="Iphone 13 Max", Price=16000, Description="Bu telefon çok iyi bir telefon.", Url="iphone-13-max", IsApproved=true, IsHome=true},

            new Product {Name="Huawei Mate 20 Pro", Price=20000, Description="Bu telefon çok iyi bir telefon.", Url="huawei-mate-20-pro", IsApproved=true, IsHome=true}
        };

        private static ProductCategory[] ProductCategories=
        {
            new ProductCategory() { Product = Products[0], Category=Categories[0] },
            new ProductCategory() { Product = Products[0], Category=Categories[2] },

            new ProductCategory() { Product = Products[1], Category=Categories[0] },
            new ProductCategory() { Product = Products[1], Category=Categories[2] },

            new ProductCategory() { Product = Products[2], Category=Categories[0] },
            new ProductCategory() { Product = Products[2], Category=Categories[2] },

            new ProductCategory() { Product = Products[3], Category=Categories[0] },
            new ProductCategory() { Product = Products[3], Category=Categories[2] },

            new ProductCategory() { Product = Products[4], Category=Categories[0] },
            new ProductCategory() { Product = Products[4], Category=Categories[2] },

            new ProductCategory() { Product = Products[5], Category=Categories[1] },
            new ProductCategory() { Product = Products[6], Category=Categories[1] },
            new ProductCategory() { Product = Products[7], Category=Categories[1] },
            new ProductCategory() { Product = Products[8], Category=Categories[1] },
            new ProductCategory() { Product = Products[9], Category=Categories[1] },
            new ProductCategory() { Product = Products[10], Category=Categories[1] },
            new ProductCategory() { Product = Products[11], Category=Categories[1] },
            new ProductCategory() { Product = Products[12], Category=Categories[1] }
        };
    }
}
