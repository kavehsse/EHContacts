using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EHContacts.Data.Entities
{
    [Table("AspNetUsers")]
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

   
    }
}
