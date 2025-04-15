using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crm.Application.DTO.Users;
using Crm.Application.DTO;
using Crm.Application.DTO.Clients;
using Crm.Domain.Entities;
using System.Diagnostics;
using Crm.Domain;

namespace Crm.Application.Interfaces
{
    public interface IClientService
    {
        Task<PaginatedResponse<ClientsDto>> GetAllClientAsync(bool ascending, bool sortByRecentlyAdded, ClientFilters filters, int pageNumber, int pageSize);
        Task<IEnumerable<ClientsDto>> GetAllArchieveClientAsync();
        Task<IEnumerable<ClientsDto>> GetClientInfoById(int clientId);
        Task<IEnumerable<ClientsDto>> SearchClientAsync(string? name);
        Task<string> AddClientAsync(AddClientsDto addClient);
        Task<string> AddCommentsToClientAsync(int clientId, string content);
        Task<string> UpdateClientAsync(int clientId, UpdateClientsDto request);
        Task<string> IsArchivedClientAsync(bool isArchived, int clientId);

        //Task<string> UnarchivedClientAsync(int clientId);
        //Task<IEnumerable<ClientsDto>> GetRecentlyAddedClientAsync();
        //Task<IEnumerable<ClientsDto>> GetClientSortedByNameAsync(bool ascending);
        //Task<IEnumerable<ClientsDto>> GetClientByIndustryAsync(string industryType);
    }
}
