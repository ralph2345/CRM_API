using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crm.Domain.Entities;

namespace Crm.Domain.Interfaces
{
    public interface IGlobalSearchRepository
    {
        Task<(List<Clients> clients, List<Users> users/*, List<TaskDetails> task, List<LeadTbl> leads*/)> SearchAsync(string search);
    }
}
