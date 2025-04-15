using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crm.Application.DTO.Login;
using Crm.Application.DTO.Users;
using Crm.Domain.Entities;

namespace Crm.Application.Interfaces
{
    public interface IUserService
    {
        Task<ApiResponseDto> RegisterAsync(RegisterUserDto request);
        Task<LoginResponseDto> LoginAsync(LoginUserDto request);
        //Task<IEnumerable<UserDto>> SearchUsersByNameAsync(string name);
        Task<IEnumerable<UserDto>> GetAllUsersAsync(string? searchName);
        Task<string> UpdateUserAsync(int userId, UpdateUserDto request);
       
        Task<string> IsDeactivateUserAsync(bool isDeactivate,int userId);
        //Task<string> DeleteUserAsync(int userId);
        Task<ApiResponseDto> ForgotPasswordAsync(ForgotPasswordDto request);
        Task<ApiResponseDto> ResetPasswordAsync(ResetPasswordDto request);
        
    }
}
