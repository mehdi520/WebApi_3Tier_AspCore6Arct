using System;
using System.Collections.Generic;

namespace AlphaApp.Repositories.Entities
{
    public partial class UAddress
    {
        public int AddressId { get; set; }
        public int UserId { get; set; }
        public int CountryId { get; set; }
    }
}
