using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EHContacts.Data.Entities;
using EHContacts.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace EHContacts.Data
{
    public class ContactsSeeder
    {
        private readonly DBContext _ctx;
        private readonly IHostingEnvironment _hosting;
        private readonly UserManager<User> _userManager;

        public ContactsSeeder(DBContext ctx,
          IHostingEnvironment hosting,
          UserManager<User> userManager)
        {
            _ctx = ctx;
            _hosting = hosting;
            _userManager = userManager;
        }

        public async Task Seed()
        {
            _ctx.Database.EnsureCreated();

            User user = await _userManager.FindByEmailAsync("hyoung@evh.com");

            if (user == null)
            {
                user = new User()
                {
                    FirstName = "Hannah",
                    LastName = "Young",
                    UserName = "hyoung@evh.com",
                    Email = "hyoung@evh.com"
                };

                var result = await _userManager.CreateAsync(user, "P@ssw0rd!");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Failed to create default user");
                }
            }

            if (!_ctx.Contacts.Any())
            {
                // Need to create sample data
                var filepath = Path.Combine(_hosting.ContentRootPath, "Data/ContactsSeed.json");
                var json = File.ReadAllText(filepath);
                var contacts = JsonConvert.DeserializeObject<IEnumerable<Contact>>(json);
                foreach (var contact in contacts)
                {
                    contact.User = user;
                }
                _ctx.Contacts.AddRange(contacts);
                _ctx.SaveChanges();

            }
        }
    }
}
