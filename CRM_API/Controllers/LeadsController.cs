using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Crm.Application.Interfaces;
using Crm.Application.DTO.Leads;

namespace CRM_API.Controllers
{
    [Authorize(AuthenticationSchemes = "Basic")]
    [Route("api/[controller]")]
    [ApiController]
    public class LeadsController : ControllerBase
    {
        private readonly ILeadsService _leadService;

        public LeadsController(ILeadsService leadService)
        {
            _leadService = leadService;
        }
        [HttpGet("all-leads")]
        public async Task<IActionResult> GetAllLeadsAsync([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var leads = await _leadService.GetAllLeadsAsync(pageNumber, pageSize);
            return Ok(leads);
        }

        [HttpGet("lead-info/{leadId}")]
        public async Task<IActionResult> GetLeadInfoByIdAsync(int leadId)
        {
            var lead = await _leadService.GetLeadByIdAsync(leadId);
            if (lead == null || !lead.Any())
            {
                return NotFound("No lead found");
            }
            return Ok(lead);
        }

        [HttpPost("add-leads")]
        public async Task<IActionResult> AddLeadsAsync([FromBody] AddLeadsDto requestLeads)
        {
            var leads = await _leadService.AddLeadAsync(requestLeads);
            if (leads == null || !leads.Any())
            {
                return BadRequest("Need to complete required fields");
            }
            return Created("", leads);
        }

    }
}
