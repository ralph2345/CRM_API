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
        Task<(IEnumerable<TaskDetails>, int)> GetAllTaskAsync(bool ascending, TaskFilters filters,int pageNumber, int pageSize);
        Task <List<TaskDetails>> GetTaskByIdAsync(List<int> id);
        Task <IEnumerable<TaskDetails>> GetAllArchieveAsync();
        Task <List<TaskDetails>> SearchTaskAsync(string? name);
        Task<string?> GetClientFullNameAsync(int clientId);
        Task AddTaskAsync(TaskDetails task);
        Task UpdateTaskAsync(List<TaskDetails> tasks);
        Task IsArchivedTaskAsync(bool isArchived, List<int> taskId);
    }
}
