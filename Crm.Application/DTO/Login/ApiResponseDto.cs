using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crm.Application.DTO.Login
{
    public class ApiResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public ApiResponseDto(bool statusCode, string message)
        {
            Success = statusCode;
            Message = message;
        }
    }
}


