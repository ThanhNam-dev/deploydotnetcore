using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMa.Data.ViewModel
{
    public class UpdateFriendShipViewModel
    {
        public Guid Id { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReceipId { get; set; }
        public bool Confirmed { get; set; }
    }
}
