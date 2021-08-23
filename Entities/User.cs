﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.Entities
{
    public class User : IdentityUser
    {
        public DateTimeOffset BirthDate { get; set; }
    }
}
