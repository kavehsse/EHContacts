using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EHContacts.Data.Entities;
using EHContacts.Models;
using EHContacts.Data;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace EHContacts.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ContactsController : ControllerBase
    {

        private readonly IDBRepository _repository;
        private readonly ILogger<ContactsController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        

        public ContactsController(IDBRepository repository, ILogger<ContactsController> logger, UserManager<User> userManager, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _userManager = userManager;
            _mapper = mapper;
        }

        private string GetUserName()
        {
            // return "hyoung@evh.com";
            return User.Identity.Name;
        }
        // GET: api/Contacts
        [HttpGet]
        public IEnumerable<ContactViewModel> GetContacts()
        {
            var username = GetUserName();
            _logger.LogInformation($"Get all contacts for user {username}");
            var contacts = _repository.GetAllContacts(username);
            return _mapper.Map<IEnumerable<Contact>, IEnumerable<ContactViewModel>>(contacts);
        }

        // GET: api/Contacts/5
        [HttpGet("{id}")]
        public IActionResult GetContact([FromRoute] int id)
        {
            var username = GetUserName();
            _logger.LogInformation($"Get contact {id} for user {username}");
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Model state is invalid");
                return BadRequest(ModelState);
            }

            var contact = _repository.GetContact(username, id);

            if (contact == null)
            {
                _logger.LogError($"Contact {id} for user {username} not found");
                return NotFound();
            }

            return Ok(_mapper.Map<Contact, ContactViewModel>(contact));
        }

        // PUT: api/Contacts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContact([FromRoute] int id, [FromBody] ContactViewModel model)
        {
            var username = GetUserName();
            _logger.LogInformation($"Update contact ({model.ToString()}) for user {username}");
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError($"Model state is invalid");
                    return BadRequest(ModelState);
                }
                var contact = _mapper.Map<ContactViewModel, Contact>(model);
                if (id != contact.Id)
                {
                    _logger.LogError($"Contact {id} doesn't match request body.");
                    return BadRequest();
                }

                Contact updatecontact = _repository.GetContact(username, id);
                if (updatecontact == null)
                {
                    _logger.LogError($"Contact {id} for user {username} not found");
                    return NotFound();
                }

                var currentUser = await _userManager.FindByNameAsync(username);
                contact.User = currentUser;
                _repository.UpdateContact(contact);
                _repository.Save();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to update contact ({model.ToString()}) for user {username}, ex : {ex}");
            }

            return BadRequest("Failed to update contact");
        }

        // POST: api/Contacts
        [HttpPost]
        public async Task<IActionResult> PostContact([FromBody] ContactViewModel model)
        {
            var username = GetUserName();
            _logger.LogInformation($"Create new contact ({model.ToString()}) for user {username}");
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Model state is invalid");
                return BadRequest(ModelState);
            }

            var contact = _mapper.Map<ContactViewModel, Contact>(model);
            var currentUser = await _userManager.FindByNameAsync(username);
            contact.User = currentUser;
            contact.Id = 0;
            _repository.AddContact(contact);
            _repository.Save();

            return CreatedAtAction("GetContact", new { id = contact.Id }, _mapper.Map<Contact, ContactViewModel>(contact));
        }

        // DELETE: api/Contacts/5
        [HttpDelete("{id}")]
        public IActionResult DeleteContact([FromRoute] int id)
        {
            var username = GetUserName();
            _logger.LogInformation($"Delete contact {id} for user {username}");
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Model state is invalid");
                return BadRequest(ModelState);
            }

            Contact contact = _repository.GetContact(username, id);
            if (contact == null)
            {
                _logger.LogError($"Contact {id} for user {username} not found");
                return NotFound();
            }

            _repository.DeleteContact(contact);
            _repository.Save();

            return Ok(_mapper.Map<Contact, ContactViewModel>(contact));
        }


    }
}