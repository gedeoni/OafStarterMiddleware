using System;
using System.Reflection;

using Domain.Common.Interfaces;

namespace Domain.Common.Attributes
{
    public sealed class AttributesProcessor : IAttributesProcessor
    {
        public TAttribute GetAttributeInstance<TAttribute>(Type type) where TAttribute : Attribute
        {
            return (TAttribute)type.GetTypeInfo().GetCustomAttribute<TAttribute>();
        }
    }
}