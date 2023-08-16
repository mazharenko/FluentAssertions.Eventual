namespace mazharenko.FluentAssertions.Eventual;

using System.Collections;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

internal readonly record struct CustomAssertionClassStage1(ClassDeclarationSyntax Class, INamedTypeSymbol Type,
	ImmutableArray<MethodDeclarationSyntax> Methods)
{
	public bool Equals(CustomAssertionClassStage1 other)
	{
		return Equals(Class, other.Class)
		       && SymbolEqualityComparer.Default.Equals(Type, other.Type)
		       && StructuralComparisons.StructuralEqualityComparer.Equals(Methods, other.Methods);
	}

	public override int GetHashCode()
	{
		unchecked
		{
			var hashCode = Class != null ? Class.GetHashCode() : 0;
			hashCode = (hashCode * 397) ^ SymbolEqualityComparer.Default.GetHashCode(Type);
			hashCode = (hashCode * 397) ^ Methods.GetHashCode();
			return hashCode;
		}
	}
}