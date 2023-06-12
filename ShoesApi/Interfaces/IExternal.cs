using Microsoft.AspNetCore.Mvc;

namespace ShoesApi.Interfaces
{
    public interface IExternal
    {
        public IActionResult ExternalLogin(string provider);
        public Task<IActionResult> ExternalLoginCallback();
    }
}
