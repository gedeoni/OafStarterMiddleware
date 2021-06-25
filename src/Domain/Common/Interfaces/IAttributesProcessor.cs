using System;

namespace Domain.Common.Interfaces
{
    public interface IAttributesProcessor
    {
        TAttribute GetAttributeInstance<TAttribute>(Type type) where TAttribute : Attribute;
    }
}