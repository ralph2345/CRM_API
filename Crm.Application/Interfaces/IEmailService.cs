using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crm.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendPasswordResetEmail(string email);
    }
}
