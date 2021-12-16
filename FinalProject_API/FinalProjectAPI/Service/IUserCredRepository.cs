using FinalProjectLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProjectAPI.Service
{
    public interface IUserCredRepository
    {
        Task<bool> UserCredExists(int userId);
        Task<IEnumerable<UserCred>> GetUserCreds();
        Task<UserCred> GetUserCredById(int userId);
        Task<bool> SaveUserCred(UserCred user);
        Task<bool> EditUserCred(UserCred user);
        Task<bool> DeleteUserCred(int userId);
        Task<UserCred> GetUserByEmailPassword(string email, string password);
    }
}
