using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Common.Interfaces
{
    public interface IWorldRepository : ICouchbaseRepository<World>
    {
    }
}