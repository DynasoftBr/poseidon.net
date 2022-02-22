using System;
using System.Collections.Generic;
using Poseidon.Common.Entities;

namespace Poseidon.Core.Entities
{
    public class EntityType : Entity<EntityType>
    {
        public string Name { get; set; }
        public List<EntityProperty> Properties { get; set; }

        public EntityType()
        {
            
        }
    }
}
