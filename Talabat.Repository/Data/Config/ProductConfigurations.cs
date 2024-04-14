using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Repository.Data.Config
{
    internal class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(b => b.Name).IsRequired().HasMaxLength(100);
            builder.Property(b => b.PictureUrl).IsRequired();
            builder.Property(b => b.Description).IsRequired();
            builder.Property(b => b.Price).HasColumnType("decimal(18,2)");
            builder.HasOne(b => b.Brand).WithMany();
            builder.HasOne(b => b.Category).WithMany();
        }
    }
}
