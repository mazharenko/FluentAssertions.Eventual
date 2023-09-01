namespace mazharenko.FluentAssertions.Eventual.Misc;

using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

internal static class CodeGeneratedAttribute
{
	public static readonly string AsString = @"[System.CodeDom.Compiler.GeneratedCodeAttribute(""mazharenko.FluentAssertions.Eventual"", ""4.0.2"")]";
	
	public static readonly AttributeListSyntax AsSyntax = 
		SyntaxFactory.ParseSyntaxTree(AsString).GetRoot().DescendantNodesAndSelf().OfType<AttributeListSyntax>().First();
}