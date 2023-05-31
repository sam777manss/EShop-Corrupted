using ShoesApi.Models;

namespace ShoesApi.Interfaces
{
    public interface IAdmin
    {
        public Task<List<AdminIndex>> AdminTables(string Uid);

        public Task<bool> Delete(string Id); 

        public Task<UserIndex> Edit(string Id);
    }
}
