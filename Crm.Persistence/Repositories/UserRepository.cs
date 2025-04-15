using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crm.Domain.Interfaces;
using Crm.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Crm.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly CrmDbContext _context;

        public UserRepository(CrmDbContext context)
        {
            _context = context;
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<Users> GetUserByIdAsync(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        /*public async Task<IEnumerable<Users>> SearchUsersByNameAsync(string name)
        {
            //searching users through their full names
            return await _context.Users
                .Where(u => u.FirstName.Contains(name) ||
                            u.MiddleName.Contains(name) ||
                            u.LastName.Contains(name))
                .ToListAsync();
        }*/

        public async Task<IEnumerable<Users>> GetAllUsersAsync(string? searchName)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchName))
            {
                query = query.Where(u =>
                    u.FirstName.Contains(searchName) ||
                    u.MiddleName.Contains(searchName) ||
                    u.LastName.Contains(searchName));
            }

            return await query.ToListAsync();
        }

        public async Task<Users> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task AddUserAsync(Users user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(Users user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();  
        }

        /*public async Task DeleteUserAsync(Users user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }*/
        public async Task IsDeactivateUserAsync(bool isDeactivate, int userId)
        {
            var user = await GetUserByIdAsync(userId);
            if (user != null)
            {
                user.Status = isDeactivate ? "Inactive" : "Active";
                await UpdateUserAsync(user);
            }
        }

        public async Task<Users?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
