using Microsoft.AspNetCore.Mvc;
using ShoesApi.Models;

namespace ShoesApi.Interfaces
{
    public interface IUser
    {
        public Task<bool> RegisterUser(Register register);
        public Task<Response> RegisterAdmin(Register registerAdmin);
        public Task<CommonIndex> LoginUser(Login login);
        public Task<bool> LogOut();
    }
}
