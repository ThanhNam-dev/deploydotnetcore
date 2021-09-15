using Microsoft.AspNetCore.Identity;
using System;

namespace EMa.Data.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public string ParentName { get; set; }
        public DateTimeOffset ParentAge { get; set; }
        public string ChildName { get; set; }
        public DateTimeOffset ChildBirth { get; set; }
        public bool ChildGender { get; set; } // 0 Nam 1 Nu
        public string IdentityCard { get; set; }
        public string Address { get; set; }
        public string UrlAvatar { get; set; }
        public int Code { get; set; }
    }
}
