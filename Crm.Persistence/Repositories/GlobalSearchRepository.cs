using Crm.Domain.Entities;
using Crm.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Crm.Persistence.Repositories
{
    public class GlobalSearchRepository : IGlobalSearchRepository
    {
        private readonly CrmDbContext _context;

        public GlobalSearchRepository(CrmDbContext context)
        {
            _context = context;
        }
        public async Task<(List<Clients> clients, List<Users> users/*, List<TaskDetails> task, List<LeadTbl> leads*/)> SearchAsync(string search)
        {
            var clientsQuery = _context.Clients
                .Include(client => client.CompanyDetails)
                .AsQueryable();
            var usersQuery = _context.Users.AsQueryable();
            //var taskQuery = _context.TaskDetails.AsQueryable();
            //var dealQuery = _context.LeadTbl.AsQueryable();


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

                // Search Tasks
                //taskQuery = taskQuery.Where(task =>
                //    task.TaskTitle.ToLower().Contains(search) ||
                //    task.TaskType.ToLower().Contains(search) 
                //);

                //// Search deals
                //dealQuery = dealQuery.Where(lead =>
                //    lead.DealTbl.DealName.ToLower().Contains(search) 
                //);
            }

            var clients = await clientsQuery.ToListAsync();
            var users = await usersQuery.ToListAsync();
            //var task = await taskQuery.ToListAsync();
            //var leads = await dealQuery.ToListAsync();

            return (clients, users/*, task, leads*/);


        }
    }
}
