using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Crm.Application.Interfaces;
using Crm.Application.DTO.Clients;
using Crm.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace CRM_API.Controllers
{
    [Authorize(AuthenticationSchemes = "Basic", Policy ="ApiKey")]
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientsController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet("all-clients")]
        public async Task<IActionResult> GetAllClients(
            [FromQuery] int pageNumber,
            [FromQuery] int pageSize,
            [FromQuery] bool ascending,
            [FromQuery] bool sortByRecentlyAdded,
            [FromQuery] string? industryType = null,
            [FromQuery] string? leadSource = null
            )
        {
            var filters = new ClientFilters
            {
                IndustryType = industryType,
                LeadSource = leadSource
            };

            var result = await _clientService.GetAllClientAsync(ascending, sortByRecentlyAdded, filters, pageNumber, pageSize);

            return Ok(result);
        }

        [HttpGet("client-info/{clientId}")]
        public async Task<IActionResult> GetClientInfoById(int clientId)
        {
            var client = await _clientService.GetClientInfoById(clientId);
            if (client == null || !client.Any()) { return NotFound("No client found"); }
            return Ok(client);
        }

        [HttpGet("all-archive-clients")]
        public async Task<IActionResult> GetAllArchiveClients()
        {
            var clients = await _clientService.GetAllArchiveClientAsync();
            if (clients == null || !clients.Any()) { return NotFound("No archieve clients"); }
            return Ok(clients);
        }

        [HttpGet("search-client")]
        public async Task<IActionResult> SearchClientByName([FromQuery] string? name)
        {
            var clients = await _clientService.SearchClientAsync(name);
            if (clients == null || !clients.Any()) 
            { 
                return NotFound(new { Message = $"No records found on the search '{name}'"}); 
            }
            return Ok(clients);
        }

        /*[HttpGet("filter-by-industry")]
        public async Task<IActionResult> GetClientByIndustry([FromQuery] string industryType)
        {
            var clients = await _clientService.GetClientByIndustryAsync(industryType);
            if (clients == null) { return NotFound(); }
            return Ok(clients);
        }*/

        [HttpPost("add-client")]
        public async Task<IActionResult> AddClient([FromBody] AddClientsDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  // Will return validation error details if the DTO is not valid.
            }
            var response = await _clientService.AddClientAsync(request);
            if (response == null) { return BadRequest(new {Message = $"Unable to add client please check your data"}); }
            return Created("",response);
        }

        [HttpPost("add-notes-to-client/{clientId}")]

        public async Task<IActionResult> AddCommentsToClient(int clientId, [FromBody] string content)
        {
            var response = await _clientService.AddCommentsToClientAsync(clientId, content);
            if (response == null) { return BadRequest(new {Message = $"Unable to add comment please check your added notes"}); }
            return Created("",response);        
        }

        [HttpPut("update-client/{clientId}")]
        public async Task<IActionResult> UpdateClient(int clientId, [FromBody] UpdateClientsDto request)
        {
            if (request == null) { return BadRequest(); }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  // Will return validation error details if the DTO is not valid.
            }
            var response = await _clientService.UpdateClientAsync(clientId, request);
            if (response == null) { return BadRequest(new {Message = $"Unable to update client"}); }

            if (response == "Invalid company address format. Expected format: 'ZipCode, City, StateProvince, Country'")
            {
                return BadRequest(new { Message = response });
            }
            return Ok(response);
        }

        [HttpPut("is-archived-client")]
        public async Task<IActionResult> ArchivedClient([FromQuery]bool isArchived,[FromQuery]List<int> clientId)
        {
            var response = await _clientService.IsArchivedClientAsync(isArchived, clientId);
            if (response == null) { return BadRequest(response); }
            return Ok(response);
        }

        /*[HttpPut("unarchive-client/{clientId}")]
        public async Task<IActionResult> UnarchivedClient(int clientId)
        {
            var response = await _clientService.UnarchivedClientAsync(clientId);

            if (response == "Client not found") { return NotFound(response); }
            return Ok(response);
        }*/

    }
}
