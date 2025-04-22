using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crm.Domain;
using Crm.Domain.Entities;
using Crm.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Crm.Persistence.Repositories
{
    public class TaskDetailsRepository : ITaskDetailsRepository
    {
        private readonly CrmDbContext _context;

        public TaskDetailsRepository(CrmDbContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<TaskDetails>, int)> GetAllTaskAsync(TaskFilters filters, int pageNumber, int pageSize)
        {
            var query = _context.TaskDetails
                .Where(t => t.IsArchived == false || t.IsArchived == null)
                .Include(t => t.Clients)
                .AsNoTracking();
                

            if (filters != null)
            {
                if (!string.IsNullOrEmpty(filters.TaskType))
                {
                    query = query.Where(t => t.TaskType != null && t.TaskType.Contains(filters.TaskType));
                }
                if (!string.IsNullOrEmpty(filters.Priority))
                {
                    query = query.Where(t => t.Priority != null && t.Priority.Contains(filters.Priority));
                }
                if (!string.IsNullOrEmpty(filters.Status))
                {
                    query = query.Where(t => t.Status != null && t.Status.Contains(filters.Status));
                }

                if (filters.StartDate.HasValue)
                {
                    query = query.Where(t => t.DueDate.HasValue && t.DueDate >= filters.StartDate.Value);
                }

                if (filters.EndDate.HasValue)
                {
                    query = query.Where(t => t.DueDate.HasValue && t.DueDate <= filters.EndDate.Value);
                }


            }

            int totalRecords = await query.CountAsync();  // Get total count before pagination

            // Apply pagination
            var paginatedTask = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (paginatedTask, totalRecords);
        }

        //retrieve client full name by clientId to get the client name in task
        public async Task<string?> GetClientFullNameAsync(int clientId)
        {
            var client = await _context.Clients.FindAsync(clientId);
            return client == null ? null : $"{client.FirstName} {client.MiddleName} {client.LastName}";
        }


        public async Task<List<TaskDetails>> SearchTaskAsync(string? name)
        {
            var tasks = await _context.TaskDetails
                .Include(t => t.Clients)
                .Where(t => (t.IsArchived == false || t.IsArchived == null) &&
                    EF.Functions.Like((t.TaskTitle ?? "") + " " + (t.TaskType ?? "") + " " + (t.AssignedTo ?? ""), $"%{name}%"))
                .ToListAsync();

            return tasks;
        }
        public async Task<TaskDetails> GetTaskByIdAsync(int id)
        {
            var task = await _context.TaskDetails
                .Include(t => t.Clients)
                .FirstOrDefaultAsync(t => t.Id == id);
            if (task == null) { throw new Exception("Not Found"); }
            return task;
        }

        public async Task <IEnumerable<TaskDetails>> GetAllArchieveAsync()
        {
            return await _context.TaskDetails
                .Where(t => t.IsArchived == true)
                .Include(t => t.Clients)
                .AsNoTracking()
                .ToListAsync();
        }

       
        public async Task AddTaskAsync(TaskDetails task)
        {
            await _context.TaskDetails.AddAsync(task);
            await _context.SaveChangesAsync();
        }

        /*public async Task UpdateTaskAsync(TaskDetails task)
        {
            _context.TaskDetails.Update(task);
            await _context.SaveChangesAsync();
        }*/

        public async Task IsArchivedTaskAsync(bool isArchived, int taskId)
        {
            var task = await _context.TaskDetails.FindAsync(taskId);
            if (task == null) { throw new Exception("Not Found"); }
            
            task.IsArchived = isArchived;
            _context.TaskDetails.Update(task);
            await _context.SaveChangesAsync();
        }
    }
}
