using Crm.Application.DTO;
using Crm.Application.DTO.Leads;

namespace Crm.Application.Interfaces
{
    public interface ILeadsService
    {
        Task<PaginatedResponse<LeadsDto>> GetAllLeadsAsync(int pageNumber, int pageSize);
        Task<IEnumerable<LeadsDto>> GetLeadByIdAsync(int id);
        Task<IEnumerable<LeadsDto>> SearchLeadsAsync(string search);
        Task<string> AddLeadAsync(AddLeadsDto lead);
        Task<string> UpdateLeadAsync(int leadId, UpdateLeadsDto update);
        //Task DeleteLeadAsync(int id);
    }
}
