﻿namespace mazharenko.FluentAssertions.Eventual;

using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using mazharenko.FluentAssertions.Eventual.Misc;

internal static class ExtensionSyntaxFactory
{
	public static ClassDeclarationSyntax? EventualExtensions(string? namespaceName, ClassDeclarationSyntax originalClass, ClassDeclarationSyntax wrapperClass)
	{
		if (originalClass.Modifiers.Any(m => m.IsKind(SyntaxKind.AbstractKeyword)))
			return null;

		// Assertions
		// Assertions<TSubject, TAssertions>
		var originalFullType = originalClass.GetFullTypeSyntax();
		// Assertions_Eventual
		// Assertions_Eventual<TSubject, TAssertions>
		var wrapperFullType = wrapperClass.GetFullTypeSyntax();

		string GetDocsId(ClassDeclarationSyntax classDeclaration)
		{
			return (namespaceName, classDeclaration.TypeParameterList) switch
			{
				(null or "", null) => $"T:{classDeclaration.Identifier.ValueText}",
				(null or "", var typeParams) =>  $"T:{classDeclaration.Identifier.ValueText}`{typeParams.Parameters.Count}",
				(var ns, null) => $"T:{ns}.{classDeclaration.Identifier.ValueText}",
				(var ns, var typeParams) =>  $"T:{ns}.{classDeclaration.Identifier.ValueText}`{typeParams.Parameters.Count}",				
			};
		}

		// T:Namespace.Assertions
		// T:Namespace.Assertions`2
		var originalDocsId = GetDocsId(originalClass);

		// T:Namespace.Assertions_Eventual
		// T:Namespace.Assertions_Eventual`2
		var wrapperDocsId = GetDocsId(wrapperClass);

		// public static Assertions_Eventual<TSubject, TAssertions> Eventually<TSubject, TAssertions>()
		//	   where ...
		// {}
		var methodSkeleton =
			SyntaxFactory.MethodDeclaration(wrapperFullType, SyntaxFactory.Identifier("Eventually"))
				.AddModifiers(
					SyntaxFactory.Token(SyntaxKind.PublicKeyword),
					SyntaxFactory.Token(SyntaxKind.StaticKeyword)
				).WithLeadingTrivia(SyntaxFactory.ParseLeadingTrivia(@$"
/// <summary>
/// Returns a <see cref=""{wrapperDocsId}""/> wrapper that adds waiting to the current <see cref=""{originalDocsId}"" />
/// </summary>
				"))
				.WithTypeParameterList(originalClass.TypeParameterList)
				.WithConstraintClauses(originalClass.ConstraintClauses);

		var extensionsIdentifier =
			originalClass.TypeParameterList is null
				? $"{originalClass.Identifier.Eventual()}_Extensions"
				: $"{originalClass.Identifier.Eventual()}_{originalClass.TypeParameterList.Parameters.Count}_Extensions";
		
		return SyntaxFactory.ClassDeclaration(extensionsIdentifier)
			.AddAttributeLists(CodeGeneratedAttribute.AsSyntax)
			.AddModifiers(
				SyntaxFactory.Token(SyntaxKind.PublicKeyword),
				SyntaxFactory.Token(SyntaxKind.StaticKeyword)
			)
			.AddMembers(
				// public static Assertions_Eventual<TSubject, TAssertions> Eventually<TSubject, TAssertions>(this Assertions<TSubject, TAssertions> underlying)
				//	   where ...
				// {
				//     return Eventually(underlying, System.TimeSpan.FromSeconds(5), System.TimeSpan.FromMilliseconds(100))
				// }
				methodSkeleton
					.WithParameterList(SyntaxFactory.ParseParameterList($"(this {originalFullType} underlying)"))
					.WithBody(
						SyntaxFactory.Block(
							SyntaxFactory.ReturnStatement(
								SyntaxFactory.ParseExpression(
									"Eventually(underlying, System.TimeSpan.FromSeconds(5), System.TimeSpan.FromMilliseconds(100))")
							)
						)
					),
				// public static Assertions_Eventual<TSubject, TAssertions> EventuallyLong<TSubject, TAssertions>(this Assertions<TSubject, TAssertions> underlying)
				//	   where ...
				// {
				//     return Eventually(underlying, System.TimeSpan.FromSeconds(20), System.TimeSpan.FromMilliseconds(500))
				// }
				methodSkeleton.WithIdentifier(SyntaxFactory.Identifier("EventuallyLong"))
					.WithParameterList(SyntaxFactory.ParseParameterList($"(this {originalFullType} underlying)"))
					.WithBody(
						SyntaxFactory.Block(
							SyntaxFactory.ReturnStatement(
								SyntaxFactory.ParseExpression(
									"Eventually(underlying, System.TimeSpan.FromSeconds(20), System.TimeSpan.FromMilliseconds(500))")
							)
						)
					),
				// public static BaseAssertions_Eventual<TSubject, TAssertions> Eventually<TSubject, TAssertions>(this BaseAssertions<TSubject, TAssertions> underlying, System.TimeSpan timeout, System.TimeSpan delay)
				// 	   where ...
				// {
				// 	   return new BaseAssertions_Eventual<TSubject, TAssertions>(underlying, timeout, delay);
				// }
				methodSkeleton
					.WithParameterList(
						SyntaxFactory.ParseParameterList($"(this {originalFullType} underlying, System.TimeSpan timeout, System.TimeSpan delay)"))
					.WithBody(
						SyntaxFactory.Block(
							SyntaxFactory.ReturnStatement(
								SyntaxFactory.ParseExpression($"new {wrapperFullType}(underlying, timeout, delay)")
							)
						)
					)
			);
	}
}