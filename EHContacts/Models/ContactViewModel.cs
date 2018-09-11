using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EHContacts.Models
{
    public class ContactViewModel
    {
        public enum ContactStatus
        {
            Inactive = 0,
            Active = 1
        };

        public int Id { get; set; }
        [Required(ErrorMessage = "Contact first name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Contact last name is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Contact email is required.")]
        [EmailAddress(ErrorMessage = "Contact email address is invalid.")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Contact phone number is invalid")]
        [Required(ErrorMessage = "Contact phone number is required.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Contact status is required.")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ContactStatus Status { get; set; }

        public override string ToString()
        {
            return $"FirstName: {FirstName}, LastName: {LastName}, Email: {Email}, PhoneNumber : {PhoneNumber}, Status: {Status}";
        }

    }
}
