using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crm.Domain.Entities;

namespace Crm.Domain.Interfaces
{
    public interface IClientRepository
    {
        Task<(IEnumerable<Clients>, int)> GetAllClientsAsync(bool ascending, bool sortByRecentlyAdded, ClientFilters filters, int pageNumber, int pageSize);
        Task<IEnumerable<Clients>> GetAllArchieveAsync();   
        Task <Clients> GetClientsByIdAsync(int clientId);
        Task<List<Clients>> GetMultipleClientsByIdAsync(List<int> id);
        Task<IEnumerable<Clients>> SearchClientsAsync(string? name);
        Task AddClientsAsync(Clients client, CompanyDetails? company, List<ContactPerson>? contact, ClientDetails? clientDetails);
        Task AddCommentToClientAsync(int clientId, string content);
        Task UpdateClientsAsync(Clients client);
        Task IsArchivedClientsAsync(bool isArchived, List<int> id);
       // Task UnarchivedClientsAsync(Clients client);
        //Task<IEnumerable<Clients>> GetRecentlyAddedClientsAsync();
       // Task<IEnumerable<Clients>> GetClientsSortedByNameAsync(bool ascending);
        //Task<IEnumerable<Clients>> GetClientsByIndustryAsync(string industryType);
    }
}

