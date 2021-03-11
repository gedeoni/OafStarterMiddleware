using System;

namespace Domain.Entities
{
    public class World
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Entity { get; set; } = "World";
        public string Name { get; set; }
        public bool HasLife { get; set; }
    }
}
