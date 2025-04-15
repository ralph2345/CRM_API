using Crm.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Crm.Application.Interfaces;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace CRM_API.Controllers
{
    [Authorize(AuthenticationSchemes = "Basic")]
    [Route("api/[controller]")]
    [ApiController]
    public class GlobalSearchController : ControllerBase
    {
        private readonly IGlobalSearchService _globalSearchService;

        public GlobalSearchController(IGlobalSearchService globalSearchService)
        {
            _globalSearchService = globalSearchService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string search)
        {
            var searchResult = await _globalSearchService.SearchAll(search);
            if (searchResult == null || !searchResult.Any()) 
            { 
                return NotFound(new { Message = $"No records found for the keyword: '{search}'" }); 
            }
            return Ok(searchResult);
        }
    }
}
