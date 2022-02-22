using Xunit;
using System.Runtime;
using System.Runtime.Loader;
using System.Reflection;
using System.IO;
using Microsoft.CodeAnalysis.Emit;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using System;
using Poseidon.Core.Entities.Util;

namespace Poseidon.Core.Dynamic.Tests;

public class UnitTest1
{
    [Fact]
    public void ShouldBuildNamespaceClassFromSimpleEntityTypeObject()
    {
        var @namespace = "TestNamespace";
        var builder = new DynamicEntityBuilder(@namespace);
        var entityType = new Entities.EntityType
        {
            Name = "User",
            Properties = new System.Collections.Generic.List<Entities.EntityProperty>{
               new Entities.EntityProperty{
                   Name = "TextProp",
                   Type = Entities.PropetyType.Text
               },
               new Entities.EntityProperty{
                   Name = "NumberProp",
                   Type = Entities.PropetyType.Number
               },
               new Entities.EntityProperty{
                   Name = "BoolenProp",
                   Type = Entities.PropetyType.Boolean
               }
           }
        };

        builder.AddClass(entityType);

        var compilation = builder.BuildCompilation();

        using (var ms = new MemoryStream())
        {
            EmitResult result = compilation.Emit(ms);

            Assert.True(result.Success, String.Join("\n", result.Diagnostics.Where(diagnostic =>
                    diagnostic.IsWarningAsError ||
                    diagnostic.Severity == DiagnosticSeverity.Error)));

            ms.Seek(0, SeekOrigin.Begin);
            Assembly assembly = Assembly.Load(ms.ToArray());
            var typeCreated = assembly.GetType($"{@namespace}.{entityType.Name}");

            Assert.NotNull(typeCreated);
            Assert.All(entityType.Properties,
                prop => Assert.NotNull(typeCreated!.GetProperty(prop.Name, returnType: prop.GetEntityPropertyType())));
        }
    }
}