using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMa.Data.ViewModel
{
    public class UpdateFriendListViewModel
    {
        public Guid Id { get; set; }
        public Guid FriendId { get; set; }
        public Guid UserId { get; set; }
    }
}
