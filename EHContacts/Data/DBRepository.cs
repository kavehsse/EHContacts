using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EHContacts.Data.Entities;
using EHContacts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EHContacts.Data
{
    public class DBRepository : IDBRepository
    {
        private readonly DBContext _ctx;
        private readonly ILogger<DBRepository> _logger;

        public DBRepository(DBContext ctx, ILogger<DBRepository> logger)
        {
            _ctx = ctx;
            _logger = logger;
        }


        public void AddContact(Contact newContact)
        {

            _ctx.Add(newContact);
        }

        public void UpdateContact(Contact contact)
        {

            var contactToUpdate = _ctx.Contacts.Find(contact.Id);
            contactToUpdate.FirstName = contact.FirstName;
            contactToUpdate.LastName = contact.LastName;
            contactToUpdate.Email = contact.Email;
            contactToUpdate.PhoneNumber = contact.PhoneNumber;
            contactToUpdate.Status = contact.Status;
            _ctx.Update(contactToUpdate);

        }


        public IEnumerable<Contact> GetAllContacts(string username)
        {

            return _ctx.Contacts
                       .Where(o => o.User.UserName == username).OrderBy(c => c.LastName).ThenBy(c => c.FirstName)
                       .ToList();

        }


        public Contact GetContact(string username, int id)
        {
            return _ctx.Contacts
                       .Where(o => o.Id == id && o.User.UserName == username)
                       .FirstOrDefault();
        }

        public void DeleteContact(Contact contact)
        {

            if (contact != null)
            {
                _ctx.Remove(contact);
            }

        }
        public bool ContactExists(int id)
        {
            return _ctx.Contacts.Any(e => e.Id == id);
        }
        public bool Save()
        {
            return _ctx.SaveChanges() > 0;
        }
    }
}
