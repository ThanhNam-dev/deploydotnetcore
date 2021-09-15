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
    public class LessionQuizConfiguration : IEntityTypeConfiguration<LessionQuiz>
    {
        public void Configure(EntityTypeBuilder<LessionQuiz> builder)
        {
            builder.ToTable("LessionQuiz");
            builder.HasKey(x => x.Id);
        }
    }
}
