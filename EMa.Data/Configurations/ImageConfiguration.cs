using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace EMa.Data.Configurations
{
    public class ImageConfiguration : IEntityTypeConfiguration<EMa.Data.Entities.Image>
    {
        public void Configure(EntityTypeBuilder<EMa.Data.Entities.Image> builder)
        {
            builder.ToTable("Image");
            builder.HasKey(x => x.Id);
        }
    }
}
