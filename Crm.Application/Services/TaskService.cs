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

        public async Task<PaginatedResponse<TaskDetailsDto>> GetAllTask(TaskFilters filters, int pageNumber, int pageSize)
        {
            var (task, totalRecords) = await _taskRepository.GetAllTaskAsync(filters, pageNumber, pageSize);

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
            var assignedTo = await _taskRepository.GetClientFullNameAsync(task.ClientID);
            if (assignedTo == null) return "Client not found";

            var addTask = new TaskDetails
            {
                TaskTitle = task.TaskTitle,
                TaskType = task.TaskType,
                AssignedTo = assignedTo,
                Priority = task.Priority,
                DueDate = task.DueDate,
                Status = task.Status,
                ClientID = task.ClientID,
            };
            await _taskRepository.AddTaskAsync(addTask);
            return "Task Added Succesfully";
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

        /*public async Task<string> UpdateTask(TaskDetailsDto task)
        {
            var taskToUpdate = await _taskRepository.GetTaskByIdAsync(task.Id);
            if (taskToUpdate == null) throw new Exception("Task not found");

            // Update the task
            //taskToUpdate.TaskID = task.TaskID;
            taskToUpdate.TaskTitle = task.TaskTitle;
            taskToUpdate.TaskType = task.TaskType;
            taskToUpdate.Priority = task.Priority;
            taskToUpdate.DueDate = task.DueDate;
            taskToUpdate.Status = task.Status;

            await _taskRepository.UpdateTaskAsync(taskToUpdate);
            return "Task Updated Succesfully";
        }*/

        public async Task<string> IsArchivedTask(bool isArchived, int taskId)
        {
            var taskToDelete = await _taskRepository.GetTaskByIdAsync(taskId);
            if (taskToDelete == null) throw new Exception("Task not found");

            await _taskRepository.IsArchivedTaskAsync(isArchived, taskId);
            if (isArchived == true)
            {
                taskToDelete.IsArchived = true;
                return "Task Archived Succesfully";
            }
            else
            {
                taskToDelete.IsArchived = false;
                return "Task Unarchived Succesfully";
            }

        }
        private TaskDetailsDto MapToDto(TaskDetails lead)
        {
            return new TaskDetailsDto
            {
                Id = lead.Id,
                TaskID = lead.TaskID,
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
