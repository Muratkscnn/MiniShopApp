using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniShopApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniShopApp.Data.Config
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.ProductId);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.DateAdded).HasDefaultValueSql("date('now')"); //SQLİTE
            //MSSQL
            //builder.Property(p => p.DateAdded).HasDefaultValueSql("getdate()");
        }
    }
}
