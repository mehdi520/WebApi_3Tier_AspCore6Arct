using System;
using System.Collections.Generic;

namespace AlphaApp.Repositories.Entities
{
    public partial class UCountry
    {
        public int CountryId { get; set; }
        public string Name { get; set; } = null!;
        public string? Flag { get; set; }
        public bool IsActive { get; set; }
    }
}
