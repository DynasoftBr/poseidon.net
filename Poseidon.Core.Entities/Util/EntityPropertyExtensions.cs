using System;

namespace Poseidon.Core.Entities.Util
{
    public static class EntityPropertyExtensions
    {
        public static Type GetEntityPropertyType(this EntityProperty property)
        {
            return property.Type switch
            {
                PropetyType.Text => typeof(String),
                PropetyType.Number => typeof(Int64),
                PropetyType.Boolean => typeof(Boolean),
                _ => throw new Exception("")
            };
        }
    }
}