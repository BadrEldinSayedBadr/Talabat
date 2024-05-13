using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Identity
{
    public class AppUser : IdentityUser
    {

        public string DisplayName { get; set; }

        public Address Address { get; set; } //Navigational Property =>> ONE
    }
}
