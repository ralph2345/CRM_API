using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crm.Domain.Entities;
using Crm.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Crm.Persistence.Repositories
{
    public class GlobalSearchRepository : IGlobalSearchRepository
    {
        private readonly CrmDbContext _context;

        public GlobalSearchRepository(CrmDbContext context)
        {
            _context = context;
        }
        public async Task<(List<Clients> clients, List<Users> users/*, int totalRecords*/)> SearchAsync(string search/*, int pageNumber, int pageSize*/)
        {
            var clientsQuery = _context.Clients
                .Include(client => client.CompanyDetails)
                .AsQueryable();
            var usersQuery = _context.Users.AsQueryable();


            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();

                // Search Clients
                clientsQuery = clientsQuery.Where(client =>
                    client.FirstName.ToLower().Contains(search) ||
                    client.MiddleName.ToLower().Contains(search) ||
                    client.LastName.ToLower().Contains(search) ||
                    client.Email.ToLower().Contains(search) ||
                    client.PhoneNumber.Contains(search) ||
                    client.CompanyDetails.CompanyName.ToLower().Contains(search)
                );

                // Search Users
                usersQuery = usersQuery.Where(user =>
                    user.FirstName.ToLower().Contains(search) ||
                    user.MiddleName.ToLower().Contains(search) ||
                    user.LastName.ToLower().Contains(search) ||
                    user.Email.ToLower().Contains(search) ||
                    user.PhoneNumber.Contains(search) ||
                    user.UserName.ToLower().Contains(search)
                );
            }

            var clients = await clientsQuery.ToListAsync();
            var users = await usersQuery.ToListAsync();

            return (clients, users);

           
        }
    }
}
