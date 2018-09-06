using EHContacts.Models;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EHContacts.IntegrationTests
{
    public class ContactsApi : IClassFixture<TestServerFixture>
    {
        private const string USERNAME = "hyoung@evh.com";
        private const string PASSWORD = "P@ssw0rd!";
        private readonly Contact _testContact;


        public class BearerToken
        {
            public string token;
            public DateTime expiration;
        }
        private readonly TestServerFixture _fixture;

        public ContactsApi(TestServerFixture fixture)
        {
            _fixture = fixture;
            _testContact = new Contact
            {
                FirstName = "joe",
                LastName = "thomas",
                Email = "joethomas@evh.com",
                PhoneNumber = "334-554-9089",
                Status = ContactViewModel.ContactStatus.Active
            };
        }

        private async Task<BearerToken> CreateToken(string username, string password)
        {
            LoginViewModel login = new LoginViewModel();
            login.Username = username;
            login.Password = password;
            var response = await _fixture.Client.PostAsync("/api/v1/accounts/createtoken", new StringContent(JsonConvert.SerializeObject(login), UnicodeEncoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            var bearerToken = JsonConvert.DeserializeObject<BearerToken>(jsonString);

            return bearerToken;
        }

        private async Task Authenticate()
        {
            if (_fixture.Client.DefaultRequestHeaders.Authorization == null)
            {
                var bearerToken = await CreateToken(USERNAME, PASSWORD);
                _fixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken.token);
            }
        }
        [Fact]
        public async Task CreateToken_ValidAccount_True()
        {
            var bearerToken = await CreateToken(USERNAME, PASSWORD);
            Assert.True(bearerToken != null && !String.IsNullOrEmpty(bearerToken.token));
        }

        [Fact]
        public async Task CreateToken_InvalidAccount_ThowsException()
        {
            HttpRequestException ex = await Assert.ThrowsAsync<HttpRequestException>(() => CreateToken("invalid", "invalid"));
        }
        [Fact]
        public async Task GetContact_ValidContact_True()
        {
            await Authenticate();

            var contact = await createContact(_testContact);

            if (contact != null)
            {

                var response = await _fixture.Client.GetAsync($"/api/v1/contacts/{contact.Id}");
                response.EnsureSuccessStatusCode();
                var jsonString = await response.Content.ReadAsStringAsync();
                contact = JsonConvert.DeserializeObject<Contact>(jsonString);

                _testContact.AssertEquals(contact);

                if (contact != null)
                {
                    await deletecontact(contact.Id);
                }
            }

        }

        [Fact]
        public async Task GetContact_InvalidContact_NotFound()
        {
            await Authenticate();
            var response = await _fixture.Client.GetAsync("/api/v1/contacts/0");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        private async Task<Contact> createContact(Contact contact)
        {
            await Authenticate();
            var response = await _fixture.Client.PostAsync("/api/v1/contacts", new StringContent(JsonConvert.SerializeObject(contact), UnicodeEncoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();

            contact = JsonConvert.DeserializeObject<Contact>(jsonString);
            return contact;


        }
        [Fact]
        public async Task CreateContact_NoEmail_ThrowsException()
        {
            Contact contact = (Contact) _testContact.Clone();
            contact.Email = "";
            HttpRequestException ex = await Assert.ThrowsAsync<HttpRequestException>(() => createContact(contact));

        }

        [Fact]
        public async Task CreateContact_ValidContact_True()
        {
            var contact = await createContact(_testContact);
            _testContact.AssertEquals(contact);

            if (contact != null)
            {
                await deletecontact(contact.Id);
            }

        }

        private async Task deletecontact(int id)
        {
            await Authenticate();
            var response = await _fixture.Client.DeleteAsync($"/api/v1/contacts/{id}");
            response.EnsureSuccessStatusCode();


        }

        [Fact]
        public async Task DeleteContact_InvalidContact_NotFound()
        {
            await Authenticate();
            var response = await _fixture.Client.DeleteAsync("/api/v1/contacts/0");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeleteContact_ValidContact_True()
        {
            await Authenticate();

            var contact = await createContact(_testContact);

            if (contact != null)
            {
                var response = await _fixture.Client.DeleteAsync($"/api/v1/contacts/{contact.Id}");
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }

        }

        [Fact]
        public async Task UpdateContact_InvalidContact_NotFound()
        {
            Contact contact = (Contact)_testContact.Clone();
            contact.Id = 0;
            await Authenticate();
            var response = await _fixture.Client.PutAsync($"/api/v1/contacts/{contact.Id}", new StringContent(JsonConvert.SerializeObject(contact), UnicodeEncoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task UpdateContact_ValidContact_True()
        {
            await Authenticate();

            var contact = await createContact(_testContact);

            if (contact != null)
            {
                contact.Email = "test@test.com";
                var response = await _fixture.Client.PutAsync($"/api/v1/contacts/{contact.Id}", new StringContent(JsonConvert.SerializeObject(contact), UnicodeEncoding.UTF8, "application/json"));

                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

                response = await _fixture.Client.GetAsync($"/api/v1/contacts/{contact.Id}");
               
                var jsonString = await response.Content.ReadAsStringAsync();
                var getcontact = JsonConvert.DeserializeObject<Contact>(jsonString);

                contact.AssertEquals(getcontact);

                if (contact != null)
                {
                    await deletecontact(contact.Id);
                }
            }

        }
    }
}
