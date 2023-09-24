using System;
using System.Collections.Generic;

namespace AlphaApp.Repositories.Entities
{
    public partial class UUser
    {
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
    }
}
