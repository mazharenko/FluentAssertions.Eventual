namespace mazharenko.FluentAssertions.Eventual;

using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Misc;


internal static class WrapperSyntaxFactory
{
	public static ClassDeclarationSyntax EventualWrapper(CustomAssertionClass assertionClassToWrap)
	{
		var originalFullType = assertionClassToWrap.Class.GetFullTypeSyntax();
		var wrapperIdentifier = assertionClassToWrap.Class.Identifier.Eventual();

		var wrapperClass =
			SyntaxFactory.ClassDeclaration(wrapperIdentifier)
				.AddAttributeLists(CodeGeneratedAttribute.AsSyntax)
				.WithModifiers(assertionClassToWrap.Class.Modifiers)
				.WithTypeParameterList(assertionClassToWrap.Class.TypeParameterList)
				.WithConstraintClauses(assertionClassToWrap.Class.ConstraintClauses)
				.AddMembers(
					SyntaxFactory.ParseMemberDeclaration($"private readonly {originalFullType} underlying;")!,
					SyntaxFactory.ParseMemberDeclaration($"private readonly System.TimeSpan timeout;")!,
					SyntaxFactory.ParseMemberDeclaration($"private readonly System.TimeSpan delay;")!,
					SyntaxFactory.ParseMemberDeclaration(@$"
public {wrapperIdentifier}({originalFullType} underlying, System.TimeSpan timeout, System.TimeSpan delay)
{(assertionClassToWrap.Base is null ? "" : ": base(underlying, timeout, delay)")}
{{
	this.{"underlying"} = {"underlying"};
	this.timeout = timeout;
	this.delay = delay;
}}
")!
				)
				.AddMembers(assertionClassToWrap.Methods
					.Select(GenerateEventualMethod)
					.ToArray<MemberDeclarationSyntax>()
				);

		if (assertionClassToWrap.Base is { } baseSyntax)
		{
			// Assertions : BaseAssertions<T1>
			// -> Assertions_Eventual : BaseAssertions_Eventual<T1>
			var newBaseType =
				baseSyntax.WithIdentifier(baseSyntax.Identifier.Eventual());

			wrapperClass = wrapperClass.AddBaseListTypes(
				SyntaxFactory.SimpleBaseType(newBaseType)
			);
		}

		return wrapperClass;
	}

	private static MethodDeclarationSyntax GenerateEventualMethod(MethodDeclarationSyntax method)
	{
		var documentationContent = method.DescendantTrivia()
				.Where(t => 
					t.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia)
				).FirstOrDefault();

		// [CustomAssertion]
		// /// <summary></summary>
		var methodWithAttribute = 
			SyntaxFactory.MethodDeclaration(method.ReturnType, method.Identifier)
				.WithParameterList(method.ParameterList)
				.WithModifiers(
					SyntaxFactory.TokenList(
						method.Modifiers.Select(x => x.WithoutTrivia())
					)
				)
				.WithLeadingTrivia(documentationContent)
				.WithAttributeLists(
					new SyntaxList<AttributeListSyntax>(
						SyntaxFactory.AttributeList(
							SyntaxFactory.SeparatedList(
								new[] { SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("CustomAssertion")) }
							)
						)
					)
				);

		// underlying.<Method>(<arg1>, <arg2>, ...)
		var underlyingInvocation = SyntaxFactory.InvocationExpression(
			SyntaxFactory.MemberAccessExpression(
				SyntaxKind.SimpleMemberAccessExpression,
				SyntaxFactory.IdentifierName("underlying"),
				SyntaxFactory.IdentifierName(method.Identifier)
			),
			SyntaxFactory.ArgumentList(
				SyntaxFactory.SeparatedList(
					method.ParameterList.Parameters.Select(parameter =>
						SyntaxFactory.Argument(SyntaxFactory.IdentifierName(parameter.Identifier))
					)
				)
			)
		);

		if (method.ReturnType is PredefinedTypeSyntax { Keyword.ValueText: "void" })
			// using (var waiting = new WaitingAssertion(timeout, delay))
			//     while (waiting.CanMakeAttempt())
			//         <underlyingInvocation>
			return
				methodWithAttribute.WithBody(
					SyntaxFactory.Block(
						SyntaxFactory.ForEachVariableStatement(
							SyntaxFactory.ParseExpression("var _"),
							SyntaxFactory.ParseExpression("EventualAssertions.Attempts(timeout, delay)"),
							SyntaxFactory.ExpressionStatement(underlyingInvocation)
						)
					)
				);

		// <ReturnType> result = default;
		// using (var waiting = new WaitingAssertion(timeout, delay))
		//     while (waiting.CanMakeAttempt())
		//         result = <underlyingInvocation>;
		// return result;
		return
			methodWithAttribute.WithBody(
				SyntaxFactory.Block(
					SyntaxFactory.ParseStatement($"{method.ReturnType} result = default!;"),
					SyntaxFactory.ForEachVariableStatement(
						SyntaxFactory.ParseExpression("var _"),
						SyntaxFactory.ParseExpression("EventualAssertions.Attempts(timeout, delay)"),
						SyntaxFactory.ExpressionStatement(
							SyntaxFactory.AssignmentExpression(
								SyntaxKind.SimpleAssignmentExpression,
								SyntaxFactory.IdentifierName("result"),
								underlyingInvocation
							)
						)
					),
					SyntaxFactory.ReturnStatement(SyntaxFactory.IdentifierName("result"))
				)
			);
	}
}