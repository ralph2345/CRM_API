using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crm.Domain.Entities;

namespace Crm.Domain.Interfaces
{
    public interface ITaskDetailsRepository
    {
        Task<(IEnumerable<TaskDetails>, int)> GetAllTaskAsync(TaskFilters filters,int pageNumber, int pageSize);
        Task <TaskDetails> GetTaskByIdAsync(int id);
        Task <List<TaskDetails>> SearchTaskAsync(string? name);
        Task<string?> GetClientFullNameAsync(int clientId);
        Task AddTaskAsync(TaskDetails task);
       //Task UpdateTaskAsync(TaskDetails task);
        Task IsArchivedTaskAsync(bool isArchived, int taskId);
    }
}
