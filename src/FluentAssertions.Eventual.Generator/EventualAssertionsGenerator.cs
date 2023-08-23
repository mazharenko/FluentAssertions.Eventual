﻿namespace mazharenko.FluentAssertions.Eventual;

using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Misc;
using static MoreLinq.MoreEnumerable;

[Generator]
public class EventualAssertionsGenerator : IIncrementalGenerator
{
	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		var assertionsDeclarations =
			context.SyntaxProvider.ForAttributeWithMetadataName(typeof(GenerateEventualAttribute).FullName,
					(s, _) => s is ClassDeclarationSyntax,
					(s, _) =>
					{
						var @class = (ClassDeclarationSyntax)s.TargetNode;
						var customAssertionMethods =
							@class.Members.OfType<MethodDeclarationSyntax>()
								.Where(method => method.IsCustomAssertion(s.SemanticModel))
								.ToImmutableArray();

						var symbol = (INamedTypeSymbol)s.TargetSymbol;
						return new CustomAssertionClassStage1(@class, symbol, customAssertionMethods);
					}
				)
				.Collect()
				.SelectMany((array, _) => { return array.Select(x => Convert(x, array)); });

		context.RegisterSourceOutput(assertionsDeclarations, GenerateEventual);
	}

	private static CustomAssertionClass Convert(CustomAssertionClassStage1 classStage1, ImmutableArray<CustomAssertionClassStage1> all)
	{
		if (classStage1.Type.BaseType is null)
			return new CustomAssertionClass(classStage1.Class, classStage1.Methods, null);

		foreach (var other in all)
		{
			// ConstructedFrom works for both generic and non-generic base classes
			if (classStage1.Type.BaseType.ConstructedFrom.Equals(other.Type, SymbolEqualityComparer.Default))
			{
				// e.g.
				// Assertions : BaseAssertions<Subject, Assertions>
				// Assertions<TSubject, TAssertions> : BaseAssertions<TSubject, TAssertions>
				if (classStage1.Type.BaseType.IsGenericType)
				{
					var @base =
						SyntaxFactory.GenericName(other.Class.Identifier,
							SyntaxFactory.TypeArgumentList(
								SyntaxFactory.SeparatedList(
									classStage1.Type.BaseType.TypeArguments.Select(
										typeArg => SyntaxFactory.ParseTypeName(typeArg.ToDisplayString())
									)
								)
							)
						);
					return new CustomAssertionClass(classStage1.Class, classStage1.Methods, @base);
				}

				return new CustomAssertionClass(classStage1.Class, classStage1.Methods, SyntaxFactory.IdentifierName(other.Class.Identifier));
			}
		}

		return new CustomAssertionClass(classStage1.Class, classStage1.Methods, null);
	}

	private static void GenerateEventual(SourceProductionContext context, CustomAssertionClass assertionClass)
	{
		context.CancellationToken.ThrowIfCancellationRequested();

		var wrapper = WrapperSyntaxFactory.EventualWrapper(assertionClass);
		var extensions = ExtensionSyntaxFactory.EventualExtensions(assertionClass.Class, wrapper);

		var rootUsings = assertionClass.Class.SyntaxTree.GetCompilationUnitRoot().Usings;
		var fileScopedUsings = 
			assertionClass.Class.Ancestors().OfType<FileScopedNamespaceDeclarationSyntax>().FirstOrDefault()?.Usings
			?? Enumerable.Empty<UsingDirectiveSyntax>();
		
		var usings = 
			rootUsings.Concat(fileScopedUsings)
			.Concat(Enumerable.Repeat(SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("mazharenko.FluentAssertions.Eventual")), 1))
			.Where(u => u.Name is not null)
			.DistinctBy(u => u.Name!.ToString());

		var newRoot =
			SyntaxFactory.CompilationUnit()
				.WithUsings(SyntaxFactory.List(usings))
				.WithMembers(
					assertionClass.Class.Ancestors().OfType<BaseNamespaceDeclarationSyntax>()
						.Aggregate(
							SyntaxFactory.List(
								new MemberDeclarationSyntax?[]
									{
										extensions, wrapper
									}.Where(node => node is not null)
									.Select(x => x!)
							),
							(list, ns) => SyntaxFactory.SingletonList<MemberDeclarationSyntax>(
								SyntaxFactory.NamespaceDeclaration(ns.Name).WithMembers(list)
							)
						)
				)
				.WithLeadingTrivia(
					SyntaxFactory.Comment($"// <auto-generated />")
				);

		var fileName =
			assertionClass.Class.TypeParameterList is null
				? assertionClass.Class.Identifier.ValueText
				: $"{assertionClass.Class.Identifier.ValueText}`{assertionClass.Class.TypeParameterList.Parameters.Count}";

		context.AddSource(fileName + ".g.cs", SourceText.From(
			newRoot.NormalizeWhitespace(indentation: "\t").ToFullString(),
			Encoding.UTF8
		));
	}

}