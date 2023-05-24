using Microsoft.AspNetCore.Mvc;
using ShoesApi.Models;

namespace ShoesApi.Interfaces
{
    public interface IUser
    {
        public Task<bool> RegisterUser(Register register);
        public Task<AdminIndex> LoginUser(Login login);
    }
}
