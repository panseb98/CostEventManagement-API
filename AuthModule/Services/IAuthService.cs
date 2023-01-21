using CostEventManegement.AuthModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostEventManegement.AuthModule.Services
{
    public interface IAuthService
    {
        Task<LoggedUser> Login(LoginDTO userLoginModel);
        Task<LoggedUser> Register(UserDTO userRegistrationModel);
    }
}
