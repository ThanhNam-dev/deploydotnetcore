using EMa.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMa.Data.Configurations
{
    public class AppUserConfiguration:IEntityTypeConfiguration<AppUser>
    {
        public void Configure (EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable("AppUsers");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ParentName).HasMaxLength(255);
        }
    }
}
