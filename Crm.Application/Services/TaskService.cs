using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crm.Domain.Interfaces;
using Crm.Application.Interfaces;
using Crm.Application.DTO.Task;
using Microsoft.EntityFrameworkCore;
using Crm.Domain.Entities;
using Crm.Domain;
using Crm.Application.DTO;
using Crm.Application.DTO.Clients;

namespace Crm.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskDetailsRepository _taskRepository;

        public TaskService(ITaskDetailsRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<PaginatedResponse<TaskDetailsDto>> GetAllTask(bool ascending, TaskFilters filters, int pageNumber, int pageSize)
        {
            var (task, totalRecords) = await _taskRepository.GetAllTaskAsync(ascending, filters, pageNumber, pageSize);

            if (task == null || !task.Any())
                return new PaginatedResponse<TaskDetailsDto>(new List<TaskDetailsDto>(), totalRecords, pageNumber, pageSize);

            var taskDto = task.Select(MapToDto);
            
            // Apply pagination
            return new PaginatedResponse<TaskDetailsDto>(taskDto, totalRecords, pageNumber, pageSize);
        }

        public async Task<IEnumerable<TaskDetailsDto>> SearchTask(string? name)
        {
            var task = await _taskRepository.SearchTaskAsync(name); ;

            return task.Select(MapToDto).ToList();
        }

        public async Task<string> AddTask(CreateTaskDto task)
        {
            var assignedTo = await _taskRepository.GetClientFullNameAsync(task.ClientId);
            if (assignedTo == null) return "Client not found";

            var addTask = new TaskDetails
            {
                TaskTitle = task.TaskTitle,
                TaskType = task.TaskType,
                AssignedTo = assignedTo,
                Priority = task.Priority,
                DueDate = task.DueDate,
                Status = task.Status,
                ClientID = task.ClientId,
            };
            await _taskRepository.AddTaskAsync(addTask);
            return "Task Added Successfully";
        }
        public async Task <IEnumerable<TaskDetailsDto>> AllArchiveTask()
        {
            var task = await _taskRepository.GetAllArchieveAsync();
            if (task == null || !task.Any())
                return new List<TaskDetailsDto>();

            //calling the MapToClientsDto method to map the clients to ClientsDto
            var clientsDto = task.Select(client => MapToDto(client)).ToList();
            return clientsDto;
        }

        

        public async Task<string> IsArchivedTask(bool isArchived, List<int> taskIds)
        {
            var tasksToArchive = await _taskRepository.GetTaskByIdAsync(taskIds); // This should return a List<TaskDetails>

            if (tasksToArchive == null || !tasksToArchive.Any())
                throw new Exception("No tasks found");

            foreach (var task in tasksToArchive)
            {
                task.IsArchived = isArchived;
            }

            await _taskRepository.UpdateTaskAsync(tasksToArchive);

            return isArchived ? "Tasks Archived Successfully" : "Tasks Unarchived Successfully";
        }
        private TaskDetailsDto MapToDto(TaskDetails lead)
        {
            return new TaskDetailsDto
            {
                Id = lead.Id,
                TaskTitle = lead.TaskTitle,
                TaskType = lead.TaskType,
                AssignedTo = lead.Clients != null ? $"{lead.Clients.FirstName} {lead.Clients.MiddleName} {lead.Clients.LastName}" : "Unknown",
                Priority = lead.Priority,
                DueDate = lead.DueDate,
                Status = lead.Status
            };

        }
    }
}
