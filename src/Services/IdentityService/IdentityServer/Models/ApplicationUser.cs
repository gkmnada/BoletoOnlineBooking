﻿using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
