
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Poseidon.Common.Entities;
using System.Reflection;

namespace Poseidon.Common.OData
{
    public static class EdmModelBuilder
    {
        public static IEdmModel GetEdmModel(Assembly assembly)
        {

            var builder = new ODataConventionModelBuilder();
            var baseType = typeof(IEntity);
            var types = assembly.GetTypes().Where(p => baseType.IsAssignableFrom(p));

            foreach (var type in types)
            {
                builder.AddEntitySet(type.Name,
                    builder.AddEntityType(type).HasKey(type.GetProperty(nameof(IEntity.Id))));
            }

            return builder.GetEdmModel();
        }

        private static Type typeOfODataConventionModelBuilder = typeof(ODataConventionModelBuilder);
        private static EntitySetConfiguration CallEntitySetDynamically(ODataConventionModelBuilder builder, Type entityTypeType)
        {
            MethodInfo mi = typeOfODataConventionModelBuilder.GetMethod("EntitySet")!;
            MethodInfo miConstructed = mi.MakeGenericMethod(entityTypeType);
            object[] args = { entityTypeType.Name };
            var result = miConstructed.Invoke(builder, args);
            return (result as EntitySetConfiguration)!;
        }

    }
}