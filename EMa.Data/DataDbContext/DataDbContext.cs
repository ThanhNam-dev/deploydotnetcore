using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using EMa.Data.Configurations;
using EMa.Data.Entities;
using EMa.Data.Extensions;

namespace EMa.Data.DataContext
{
    public partial class DataDbContext : IdentityDbContext<AppUser, AppRole, Guid> 
    {
        public DataDbContext()
        {
        }

        public DataDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
            //Configure using Fluent API 

            modelBuilder.ApplyConfiguration(new AppUserConfiguration());
            modelBuilder.ApplyConfiguration(new AppRoleConfiguration());

            modelBuilder.ApplyConfiguration(new BLogConfiguration());
            modelBuilder.ApplyConfiguration(new FriendListConfiguration());
            modelBuilder.ApplyConfiguration(new FriendShipConfiguration());
            modelBuilder.ApplyConfiguration(new ImageConfiguration());
            modelBuilder.ApplyConfiguration(new LessionConfiguration());
            modelBuilder.ApplyConfiguration(new LessionQuizConfiguration());
            modelBuilder.ApplyConfiguration(new MessageConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationConfiguration());
            modelBuilder.ApplyConfiguration(new PostCommentConfiguration());
            modelBuilder.ApplyConfiguration(new PostConfiguration());
            modelBuilder.ApplyConfiguration(new PostImageConfiguration());
            modelBuilder.ApplyConfiguration(new PostLikeConfiguration());
            modelBuilder.ApplyConfiguration(new QuizConfiguration());
            modelBuilder.ApplyConfiguration(new QuizTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RoomConfiguration());
            modelBuilder.ApplyConfiguration(new UserQuizConfiguration());


            modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("AppUserClaims");
            modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("AppUserRoles").HasKey(x => new { x.UserId, x.RoleId });
            modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("AppUserLogins").HasKey(x => x.UserId);

            modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("AppRoleClaims");
            modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("AppUserTokens").HasKey(x => x.UserId);

            //Data seeding
            modelBuilder.Seed();
        }
        public DbSet<AppRole> AppRoles { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<FriendList> FriendLists { get; set; }
        public DbSet<FriendShip> FriendShips { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Lession> Lessions { get; set; }
        public DbSet<LessionQuiz> LessionQuizzes { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostComment> PostComments { get; set; }
        public DbSet<PostImage> PostImages { get; set; }
        public DbSet<PostLike> PostLikes { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<QuizType> QuizTypes { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<UserQuiz> UserQuizzes { get; set; }

    }
}
