using EHContacts.Models;
using System;
using Xunit;

namespace EHContacts.IntegrationTests
{
    public class Contact : ContactViewModel, ICloneable
    {
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public void AssertEquals(Contact contact)
        {
            Assert.True(contact != null && string.Compare(contact.FirstName, FirstName, true) == 0
              && string.Compare(contact.LastName, LastName, true) == 0
              && string.Compare(contact.Email, Email, true) == 0
              && string.Compare(contact.PhoneNumber, PhoneNumber) == 0 &&
              contact.Status == Status);
        }
    }
}
