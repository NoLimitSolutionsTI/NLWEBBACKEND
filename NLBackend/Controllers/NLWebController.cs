using Microsoft.AspNetCore.Mvc;
using NLBackend.Models;
using NLBackend.Services;

namespace NLBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NLWebController : ControllerBase
    {
        private readonly NLWebService _service;

        public NLWebController(NLWebService service) =>
            _service = service;

        [HttpGet]
        public async Task<List<Contacts>> Get() =>
            await _service.GetContactsAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Contacts>> Get(string id)
        {
            var contact = await _service.GetContactAsync(id);

            if (contact is null)
            {
                return NotFound();
            }
            return contact;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Contacts newContact)
        {
            await _service.CreateAsync(newContact);

            return CreatedAtAction(nameof(Get), new { id = newContact.Id }, newContact);
        }
    }
}
