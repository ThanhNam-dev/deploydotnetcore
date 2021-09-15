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
    
    public class FriendShipConfiguration : IEntityTypeConfiguration<FriendShip>
    {
        public void Configure(EntityTypeBuilder<FriendShip> builder)
        {
            builder.ToTable("FriendShip");
            builder.HasKey(x => x.Id);
        }
    }
}
