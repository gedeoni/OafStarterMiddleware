using Application.Common.Interfaces;
using Infrastructure.Persistence;

namespace Integration
{

    public class RepositoryFixture
    {
        public readonly IWorldRepository worldRepo;

        public RepositoryFixture(IWorldRepository worldRepo)
        {
            // worldRepo = new WorldRepository();
        }
    }
}