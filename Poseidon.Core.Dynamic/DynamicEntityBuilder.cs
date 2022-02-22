using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Poseidon.Common.Util;
using Poseidon.Core.Entities;
using Poseidon.Core.Entities.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Poseidon.Core.Dynamic
{
    public class DynamicEntityBuilder
    {
        private readonly string namespaceName;
        private NamespaceDeclarationSyntax @namespace;
        private HashSet<string> declaredMembers = new HashSet<string>();

        public DynamicEntityBuilder(string namespaceName)
        {
            this.namespaceName = namespaceName;
            // Create a namespace: (namespace CodeGenerationSample)
            this.@namespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(namespaceName)).NormalizeWhitespace();
        }

        public void AddClass(EntityType entityType)
        {
            //  Create a class:
            var classDeclaration = SyntaxFactory.ClassDeclaration(entityType.Name);

            // Added the class to the set so if other members reference this type we don't try to recreate it causing duplication.
            declaredMembers.Add(entityType.Name);

            // Add the public modifier: (public class Abc)
            classDeclaration = classDeclaration.AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));

            // Inherit Entity<{entityType.Name}>
            classDeclaration = classDeclaration.AddBaseListTypes(
                SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName($"Entity<{entityType.Name}>")));

            // Create property
            var propsDeclarations = entityType.Properties.Select(p => this.BuildProperty(p));

            // Add the property to the class.
            classDeclaration = classDeclaration.AddMembers(propsDeclarations.ToArray());

            // Add the class to the namespace.
            this.@namespace = this.@namespace.AddMembers(classDeclaration);
        }

        public CSharpCompilation BuildCompilation()
        {
            var etType = typeof(EntityType);
            // Add System using statement: (using System)
            this.@namespace = this.@namespace.AddUsings(
                SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System")),
                SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(etType.Namespace!))
            );

            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(this.@namespace.NormalizeWhitespace().ToFullString());

            var references = etType.Assembly.GetReferencedAssemblies()
                .Select(a => MetadataReference.CreateFromFile(Assembly.Load(a).Location)).ToList();
            references.Add(MetadataReference.CreateFromFile(typeof(object).Assembly.Location));
            references.Add(MetadataReference.CreateFromFile(etType.Assembly.Location));
            references.Add(MetadataReference.CreateFromFile(typeof(EquatableObject<>).Assembly.Location));

            Assembly.GetEntryAssembly().GetReferencedAssemblies()
                        .ToList()
                        .ForEach(a => references.Add(MetadataReference.CreateFromFile(Assembly.Load(a).Location)));

            return CSharpCompilation.Create(namespaceName,
                syntaxTrees: new[] { syntaxTree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        }

        private PropertyDeclarationSyntax BuildProperty(EntityProperty property)
        {
            var propType = property.GetEntityPropertyType();
            // Create a Property: (public int {property.Name} { get; set; })
            var propertyDeclaration = SyntaxFactory.PropertyDeclaration(SyntaxFactory.ParseTypeName(propType.FullName!), property.Name)
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .AddAccessorListAccessors(
                    SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                    SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)));

            return propertyDeclaration;
        }
    }
}