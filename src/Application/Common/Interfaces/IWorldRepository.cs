using Domain.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Application.Common.Interfaces
{
    public interface IWorldRepository : ICouchbaseRepository<World>
    {
        public Task<IAsyncEnumerable<int>> GetTotalWorlds(string entityName);
    }
}