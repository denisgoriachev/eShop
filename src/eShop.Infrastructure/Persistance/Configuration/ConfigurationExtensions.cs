using eShop.Application.Persistance;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Infrastructure.Persistance.Configuration
{
    public static class ConfigurationExtensions
    {
        public static EntityTypeBuilder<TEntity> IsUnique<TEntity>(this EntityTypeBuilder<TEntity> builder)
            where TEntity : class, IUnique
        {
            builder.HasKey(e => e.Id);

            return builder;
        }
    }
}
