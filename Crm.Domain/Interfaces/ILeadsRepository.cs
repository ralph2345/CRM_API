using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crm.Domain.Entities;

namespace Crm.Domain.Interfaces
{
    public interface ILeadsRepository
    {
        Task <(IEnumerable<LeadTbl>,int)> GetAllLeadsAsync(int pageNumber, int pageSize);
        Task<LeadTbl> GetLeadByIdAsync(int id);
        Task<IEnumerable<LeadTbl>> SearchLeadAsync(string search);
        Task AddLeadAsync(LeadTbl lead, DealTbl deals, Payment payment);
        Task UpdateLeadAsync(LeadTbl lead);
        //Task DeleteLeadAsync(int id);
    }
}
