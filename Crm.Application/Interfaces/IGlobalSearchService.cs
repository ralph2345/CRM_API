using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crm.Application.DTO;
using Crm.Application.DTO.Clients;

namespace Crm.Application.Interfaces
{
    public interface IGlobalSearchService
    {
        Task<IEnumerable<GlobalSearchResultDto>> SearchAll(string search);
    }
}
