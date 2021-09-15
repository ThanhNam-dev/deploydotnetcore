using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMa.API.Entity
{
    public class AuthResult
    {
        public object Token { get; set; }
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
}
