using Microsoft.EntityFrameworkCore;
using EHContacts.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace EHContacts.Models
{
    public class DBContext : IdentityDbContext<User>
    {
        public DBContext (DbContextOptions<DBContext> options)
            : base(options)
        {
        }

        public DbSet<EHContacts.Data.Entities.Contact> Contacts { get; set; }
    }
}
