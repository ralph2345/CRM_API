using Crm.Domain;
using Crm.Domain.Entities;
using Crm.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Crm.Persistence.Repositories
{
    public class ClientsRepository : IClientRepository
    {
        private readonly CrmDbContext _context;

        public ClientsRepository(CrmDbContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<Clients>, int)> GetAllClientsAsync(bool ascending, bool sortByRecentlyAdded, ClientFilters filters, int pageNumber, int pageSize)
        {
            var query = _context.Clients
                .Where(client => !client.IsArchived)//do not fetch archived clients
                .Include(client => client.CompanyDetails)
                .Include(client => client.ContactPerson)
                .Include(client => client.ClientDetails)
                //.ThenInclude(clients => clients.Notes)
                .AsNoTracking();

            // Apply filters
            if (filters != null)
            {
                if (!string.IsNullOrEmpty(filters.IndustryType))
                {
                    query = query.Where(client => client.CompanyDetails != null && client.CompanyDetails.IndustryType.Contains(filters.IndustryType));
                }

                if (!string.IsNullOrEmpty(filters.LeadSource))
                {
                    query = query.Where(client => client.ClientDetails != null && client.ClientDetails.LeadSources.Contains(filters.LeadSource));
                }
            }

            // Sorting
            if (sortByRecentlyAdded)
            {
                query = query.OrderByDescending(client => client.DateCreated);
            }
            else
            {
                query = ascending ? query.OrderBy(client => client.FirstName) : query.OrderByDescending(client => client.FirstName);
            }

            int totalRecords = await query.CountAsync();  // Get total count before pagination

            // Apply pagination
            var paginatedClients = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (paginatedClients, totalRecords);
        }

        public async Task<IEnumerable<Clients>> GetAllArchieveAsync()
        {
            return await _context.Clients
                .Where(c => c.IsArchived)  // Only archived clients will fetch
                .Include(c => c.CompanyDetails)
                .Include(c => c.ContactPerson)
                .Include(c => c.ClientDetails)
                .AsNoTracking()  // Improves performance if no updates are needed
                .ToListAsync();
        }

        public async Task<IEnumerable<Clients>> SearchClientsAsync(string? name)
        {
            if (string.IsNullOrEmpty(name))
                return new List<Clients>();

            //searching by names and company names
            return await _context.Clients
                .Include(client => client.CompanyDetails)
                .Include(client => client.ContactPerson)
                .Include(client => client.ClientDetails)
                .Where(client =>
                    !client.IsArchived &&
                    (
                        EF.Functions.Like(client.FirstName + " " + client.MiddleName + " " + client.LastName, $"%{name}%") || // Full name search
                        (client.CompanyDetails != null && EF.Functions.Like(client.CompanyDetails.CompanyName, $"%{name}%"))
                    )
                ).ToListAsync();
        }


        public async Task<Clients> GetClientsByIdAsync(int clientId)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return await _context.Clients
                .Where(client => !client.IsArchived)//not archived clients
                .Include(c => c.CompanyDetails) // Load company details
                .Include(c => c.ContactPerson)  // Load contact person
                .Include(c => c.ClientDetails) // Load client details
                .ThenInclude(cd => cd.Notes) // Load client notes
                .FirstOrDefaultAsync(c => c.ClientID == clientId);
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task<List<Clients>> GetMultipleClientsByIdAsync(List<int> clientId)
        {
            return await _context.Clients
                .Include(c => c.CompanyDetails) // Load company details
                .Include(c => c.ContactPerson)  // Load contact person
                .Include(c => c.ClientDetails) // Load client details
                .ThenInclude(cd => cd.Notes) // Load client notes
                .Where(c => clientId.Contains(c.ClientID))
                .ToListAsync();
        }

        public async Task AddClientsAsync(Clients clients, CompanyDetails? company, List<ContactPerson>? contact, ClientDetails? clientDetails)
        {
            //add first the client to generate the other details
            await _context.Clients.AddAsync(clients);
            await _context.SaveChangesAsync();

            if (company != null)
            {
                company.ClientID = clients.ClientID;
                await _context.CompanyDetails.AddAsync(company);
            }

            if (contact != null && contact.Any())
            {
                foreach (var contacts in contact)
                {
                    contacts.ClientID = clients.ClientID; // Assign ClientID to each contact
                }
                await _context.ContactPersons.AddRangeAsync(contact);
            }

            if (clientDetails != null)
            {
                clientDetails.ClientID = clients.ClientID;
                if (clientDetails.Notes != null && clientDetails.Notes.Any())
                {
                    foreach (var note in clientDetails.Notes)
                    {
                        note.ClientDetails = clientDetails;
                    }
                }
                await _context.ClientDetails.AddAsync(clientDetails);
            }

            await _context.SaveChangesAsync();

        }

        public async Task AddCommentToClientAsync(int clientId, string content)
        {
            var clientDetails = await _context.ClientDetails
                .FirstOrDefaultAsync(cd => cd.ClientID == clientId);
            if (clientDetails == null) { throw new Exception("Client Details Not Found"); }

            var comment = new Comments
            {
                Content = content,
                CreatedAt = DateTimeOffset.UtcNow,
                ClientDetailsId = clientDetails.ClientDetailsId
            };

            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateClientsAsync(Clients clients)
        {
            _context.Clients.Update(clients);
            await _context.SaveChangesAsync();
        }

        public async Task IsArchivedClientsAsync(bool isArchived, List<int> ids)
        {
            var clients = await _context.Clients
                .Where(c => ids.Contains(c.ClientID))
                .ToListAsync(); // Retrieve multiple clients

            if (!clients.Any()) { throw new Exception("Not Found"); }

            foreach (var client in clients)
            {
                client.IsArchived = isArchived;
            }

            _context.Clients.UpdateRange(clients); // Use UpdateRange for multiple entities
            await _context.SaveChangesAsync();
        }






    }
}
