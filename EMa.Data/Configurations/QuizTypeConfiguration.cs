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
    public class QuizTypeConfiguration : IEntityTypeConfiguration<QuizType>
    {
        public void Configure(EntityTypeBuilder<QuizType> builder)
        {
            builder.ToTable("QuizType");
            builder.HasKey(x => x.Id);
        }
    }
}
