using System.Collections.Generic;
using EHContacts.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EHContacts.Data
{
    public interface IDBRepository
    {

        IEnumerable<Contact> GetAllContacts(string username);
        Contact GetContact(string username, int id);
        void AddContact(Contact newContact);
        void UpdateContact(Contact newContact);

        void DeleteContact(Contact contact);
        bool ContactExists(int id);
        bool Save();
      
    }
}