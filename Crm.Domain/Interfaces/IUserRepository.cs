using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crm.Domain.Entities;

namespace Crm.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> UserExistsAsync(string email);
        Task<Users> GetUserByIdAsync(int userId);
        Task <List<Users>> GetMultipleUserByIdAsync(List<int> userId);
        Task<Users> GetUserByUsernameAsync(string? username);
        //Task<IEnumerable<Users>> SearchUsersByNameAsync(string name);   
        Task<IEnumerable<Users>> GetAllUsersAsync(string searchName);
        Task AddUserAsync(Users user);
        Task UpdateUserAsync(Users user);   
        //Task DeleteUserAsync(Users user);
        Task IsDeactivateUserAsync(bool isDeactivate, List<int> userId);
        Task<Users?> GetUserByEmailAsync(string email);
        
    }
}
