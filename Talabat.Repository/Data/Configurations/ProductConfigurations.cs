using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Repository.Data.Configurations
{
    public class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);   
            builder.Property(p => p.Description).IsRequired();   
            builder.Property(p => p.PictureUrl).IsRequired();
            builder.Property(p => p.Price).IsRequired().HasColumnType("decimal(18,2)");

            builder.HasOne(p => p.Brand)
                   .WithMany()
                   .HasForeignKey(p => p.BrandId);

            builder.HasOne(p => p.Type)
                  .WithMany()
                  .HasForeignKey(p => p.TypeId);
        }
    }
}
