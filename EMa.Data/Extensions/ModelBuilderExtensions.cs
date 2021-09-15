using System;
using EMa.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EMa.Data.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            // Any guid
            var roleId = new Guid("8D04DCE2-969A-435D-BBA4-DF3F325983DC");
            var adminId = new Guid("69BD714F-9576-45BA-B5B7-F00649BE00DE");
            
            modelBuilder.Entity<AppRole>().HasData(new AppRole
            {
                Id = roleId,
                Name = "Admin",
                NormalizedName = "ADMIN",
                Description = "Administrator role"
            });

            var hasher = new PasswordHasher<AppUser>();

            modelBuilder.Entity<AppUser>().HasData(new AppUser
            {
                Id = adminId,
                UserName = "0968354148",
                NormalizedUserName = "0968354148",
                Email = "ema@gmail.com",
                NormalizedEmail = "EMA@GMAIL.COM",
                ChildName = "Ema Baby",
                EmailConfirmed = true,
                PhoneNumber = "0968354148",
                PasswordHash = hasher.HashPassword(null, "Abc123!@#"),
                SecurityStamp = string.Empty,
                ParentName = "Administrator",
                UrlAvatar = "/upload/avatar/admin1.jpg",
                Address = "Đà Nẵng",
                Code = 050620
            });

            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
            {
                RoleId = roleId,
                UserId = adminId
            });

        }
    }
}