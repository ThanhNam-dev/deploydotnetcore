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
    public class FriendListConfiguration : IEntityTypeConfiguration<FriendList>
    {
        public void Configure(EntityTypeBuilder<FriendList> builder)
        {
            builder.ToTable("FriendList");
            builder.HasKey(x => x.Id);
        }
    }
}
