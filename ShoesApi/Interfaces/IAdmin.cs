using ShoesApi.Models;

namespace ShoesApi.Interfaces
{
    public interface IAdmin
    {
        public Task<List<AdminIndex>> AdminTables(string Uid);
    }
}
