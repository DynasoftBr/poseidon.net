using Poseidon.Common.Entities;

namespace Poseidon.Core.Entities
{
    public class EntityProperty : Entity<EntityProperty>
    {
        public string Name { get; set; }
        public PropetyType Type { get; set; }
    }

    public enum PropetyType
    {
        Text,
        Boolean,
        Number
    }
}