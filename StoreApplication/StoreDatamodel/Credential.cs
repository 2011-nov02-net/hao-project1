using System;
using System.Collections.Generic;

#nullable disable

namespace StoreDatamodel
{
    public partial class Credential
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
