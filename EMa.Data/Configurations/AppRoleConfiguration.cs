using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EMa.Data.Entities;

namespace EMa.Data.Configurations
{
   public class AppRoleConfiguration:IEntityTypeConfiguration<AppRole>
    {
        public void Configure (EntityTypeBuilder<AppRole> builder)
        {
            builder.ToTable("AppRoles");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Description).HasMaxLength(256);
        }
    }
}
