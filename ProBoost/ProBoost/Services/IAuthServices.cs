using ProBoost.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProBoost.Services
{
    public interface IAuthServices
    {
        event EventHandler GetUser;
        User CurrentUser { get; }
        Task Login(string email, string password);
        Task Register(string email,string fullname ,string password);
        Task UpdateUser(User user);
        void Logout();
        void UserPhone();
        void GetOneUser(string uid);
    }
    public enum EventType
    {
        User,Users,UserGet
    }
    public class UserEventArgs: EventArgs
    {
        public User User { get; set; }
        public EventType EventType { get; set; }
        public IEnumerable<User> Users { get; set; }
    }
}
