using ProBoost.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProBoost.Services
{
    public interface IAuthServices
    {
        User CurrentUser { get; }
        Task Login(string email, string password);
        Task Register(string email,string fullname ,string password);
        void Logout();
    }
}
