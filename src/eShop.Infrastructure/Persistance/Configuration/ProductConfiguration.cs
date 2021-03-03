using eShop.Application.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Infrastructure.Persistance.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.IsUnique();

            builder.Property(e => e.SKU)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(1024);

            builder.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(4096);

            builder.Property(e => e.CreatedBy)
                .IsRequired()
                .HasMaxLength(128);

            builder.Property(e => e.CreatedAt)
                .IsRequired();

            builder.Property(e => e.UpdatedBy)
                .HasMaxLength(128);

            builder.Property(e => e.UpdatedAt);
        }
    }
}
