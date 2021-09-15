using EMa.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMa.Data.Configurations
{
    public class UserQuizConfiguration : IEntityTypeConfiguration<UserQuiz>
    {
        public void Configure(EntityTypeBuilder<UserQuiz> builder)
        {
            builder.ToTable("UserQuiz");
            builder.HasKey(x => x.Id);
        }
    }
}
