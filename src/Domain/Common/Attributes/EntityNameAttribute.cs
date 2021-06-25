using System;

namespace Domain.Common.Attributes
{
    [
        AttributeUsage(
            AttributeTargets.Class | AttributeTargets.Struct,
            AllowMultiple = false
        )
    ]
    public sealed class EntityNameAttribute : Attribute
    {
        public string Name { get; }

        public EntityNameAttribute(string name)
        {
            Name = name;
        }
    }
}