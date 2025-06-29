using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace UnitTests.EditMode
{
    public class CodeConventionsTests
    {
        [Test]
        public void AllClassesShouldBeInNamespaces()
        {
            var classesNotInNamespaces = 
                from path in CSharpAssetPaths()
                let source = AssetDatabase.LoadAssetAtPath<TextAsset>(path)
                from @class in ClassesNotInNameSpace(source.text)
                select (@class, path);
            
            classesNotInNamespaces.Should().BeEmpty();
        }

        [Test]
        public void AllInterfacesShouldBeInInterfacesFolder()
        {
            var interfacesNotInInterfacesFolder = 
                from path in CSharpAssetPaths()
                let source = AssetDatabase.LoadAssetAtPath<TextAsset>(path)
                from @interface in InterfacesNotInInterfacesFolder(source.text, path)
                select (@interface, path);
            
            interfacesNotInInterfacesFolder.Should().BeEmpty();
        }

        private static IEnumerable<string> CSharpAssetPaths() =>
            AssetDatabase.FindAssets("t:TextAsset", new[] { "Assets/Scripts" })
                .Select(AssetDatabase.GUIDToAssetPath)
                .Where(IsCsharpFile);

        private static IEnumerable<object> ClassesNotInNameSpace(string sourceText) =>
            CSharpSyntaxTree
                .ParseText(sourceText)
                .GetRoot()
                .DescendantNodesAndSelf()
                .OfType<ClassDeclarationSyntax>()
                .Where(NotInNamespace)
                .Select(ClassName);

        private static IEnumerable<object> InterfacesNotInInterfacesFolder(string sourceText, string path) =>
            CSharpSyntaxTree
                .ParseText(sourceText)
                .GetRoot()
                .DescendantNodesAndSelf()
                .OfType<InterfaceDeclarationSyntax>()
                .Where(_ => !IsInInterfacesFolder(path))
                .Select(InterfaceName);
        private static bool IsCsharpFile(string path) =>
            path.EndsWith(".cs", StringComparison.InvariantCultureIgnoreCase);

        private static bool NotInNamespace(ClassDeclarationSyntax node) => !InNamespace(node);
        private static bool InNamespace(ClassDeclarationSyntax node) => node.Ancestors().OfType<NamespaceDeclarationSyntax>().Any();
        private static string ClassName(ClassDeclarationSyntax node) => node.Identifier.Text;
        private static bool IsInInterfacesFolder(string path) => path.Contains("/Interfaces/", StringComparison.InvariantCultureIgnoreCase);
        private static string InterfaceName(InterfaceDeclarationSyntax node) => node.Identifier.Text;


    }
}