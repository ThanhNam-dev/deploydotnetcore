using System;

namespace EMa.Data.ViewModel
{
    public class UpdateProfileViewModel
    {
        public Guid Id { get; set; }
        public string ParentName { get; set; }
        public DateTimeOffset ParentAge { get; set; }
        public string ChildName { get; set; }
        public DateTimeOffset ChildBirth { get; set; }
        public bool ChildGender { get; set; } // 0 Nam 1 Nu
        public string IdentityCard { get; set; }
        public string Address { get; set; }
        public string UrlAvatar { get; set; }
    }
}
