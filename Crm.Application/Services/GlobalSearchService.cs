using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crm.Application.DTO;
using Crm.Application.DTO.Clients;
using Crm.Domain.Entities;
using Crm.Domain.Interfaces;
using Crm.Application.Interfaces;

namespace Crm.Application.Services
{
    public class GlobalSearchService : IGlobalSearchService
    {
        private readonly IGlobalSearchRepository _globalSearchRepository;

        public GlobalSearchService(IGlobalSearchRepository globalSearchRepository)
        {
            _globalSearchRepository = globalSearchRepository;
        }

        public async Task<IEnumerable<GlobalSearchResultDto>> SearchAll(string search)
        {
            var(clients, users) = await _globalSearchRepository.SearchAsync(search);

            var clientResults = clients.Select(client => new GlobalSearchResultDto
            {
                Type = "Client",
                FullName = $"{client.FirstName} {client.MiddleName} {client.LastName}",
                Email = client.Email,
                PhoneNumber = client.PhoneNumber,
                CompanyName = client.CompanyDetails.CompanyName
            });

            var userResults = users.Select(user => new GlobalSearchResultDto
            {
                Type = "User",
                FullName = $"{user.FirstName} {user.MiddleName} {user.LastName}",
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
            });  

            return clientResults.Concat(userResults);
        }
    }
}
