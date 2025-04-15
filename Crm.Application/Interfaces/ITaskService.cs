using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crm.Application.DTO;
using Crm.Application.DTO.Task;
using Crm.Domain;
namespace Crm.Application.Interfaces
{
    public interface ITaskService
    {
        Task<PaginatedResponse<TaskDetailsDto>> GetAllTask(TaskFilters filters, int pageNumber, int pageSize);
        //Task<TaskDetailsDto> GetTaskById(int id);
        Task <IEnumerable<TaskDetailsDto>> SearchTask(string? name);   
        Task <string>AddTask(CreateTaskDto task);
        //Task <string>UpdateTask(TaskDetailsDto task);
        Task <string>IsArchivedTask(bool isArchived, int taskId);
    }
}
