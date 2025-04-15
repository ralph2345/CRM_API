using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crm.Application.DTO;
using Crm.Application.DTO.Leads;

namespace Crm.Application.Interfaces
{
    public interface ILeadsService
    {
        Task<PaginatedResponse<LeadsDto>> GetAllLeadsAsync(int pageNumber, int pageSize);
        Task<IEnumerable<LeadsDto>> GetLeadByIdAsync(int id);
        Task<string> AddLeadAsync(AddLeadsDto lead);
        //Task UpdateLeadAsync(LeadsDto lead);
        //Task DeleteLeadAsync(int id);
    }
}
