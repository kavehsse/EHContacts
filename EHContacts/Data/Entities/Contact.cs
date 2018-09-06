namespace EHContacts.Data.Entities
{
    public class Contact
    {
        public enum ContactStatus
        {
            Inactive = 0,
            Active = 1
        };

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public ContactStatus Status { get; set; }

        public User User { get; set; }

    }
}
