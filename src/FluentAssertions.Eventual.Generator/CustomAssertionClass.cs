namespace mazharenko.FluentAssertions.Eventual;

using System.Collections;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis.CSharp.Syntax;

internal readonly record struct CustomAssertionClass(ClassDeclarationSyntax Class, ImmutableArray<MethodDeclarationSyntax> Methods, SimpleNameSyntax? Base)
{
	public bool Equals(CustomAssertionClass other)
	{
		return Equals(Class, other.Class) 
			&& StructuralComparisons.StructuralEqualityComparer.Equals(Methods, other.Methods);
	}

	public override int GetHashCode() 
	{ 
		unchecked 
		{ 
			var hashCode = Class != null ? Class.GetHashCode() : 0;
			hashCode = (hashCode * 397) ^ Methods.GetHashCode(); 
			return hashCode; 
		} 
	}
}