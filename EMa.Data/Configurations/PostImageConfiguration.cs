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
    public class PostImageConfiguration : IEntityTypeConfiguration<PostImage>
    {
        public void Configure(EntityTypeBuilder<PostImage> builder)
        {
            builder.ToTable("PostImage");
            builder.HasKey(x => x.Id);
        }
    }
}
