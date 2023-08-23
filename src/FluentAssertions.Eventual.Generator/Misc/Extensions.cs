namespace mazharenko.FluentAssertions.Eventual.Misc;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

internal static class Extensions
{
	public static TypeSyntax GetFullTypeSyntax(this ClassDeclarationSyntax assertionClass)
	{
		return SyntaxFactory.ParseTypeName($"{assertionClass.Identifier.ValueText}{assertionClass.TypeParameterList}");
	}
	
	public static SyntaxToken Eventual(this SyntaxToken syntaxToken)
	{
		return SyntaxFactory.Identifier(syntaxToken + "_Eventual");
	}
	
	public static bool IsCustomAssertion(this MethodDeclarationSyntax method, SemanticModel semantic)
	{
		foreach (var attributeList in method.AttributeLists)
		{
			foreach (var attribute in attributeList.Attributes)
			{
				var attributeSymbol = semantic.GetSymbolInfo(attribute).Symbol;
				if ("FluentAssertions.CustomAssertionAttribute" == attributeSymbol?.ContainingType.ToString())
					return true;
			}
		}

		return false;
	}
}