﻿using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using VerifyNUnit;
using VerifyTests;

namespace mazharenko.FluentAssertions.Eventual.Generator.Tests;

public abstract class BaseTests
{
	protected async Task RunAndVerify(string source)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source, path: "source.cs");
		var dotNetAssemblyPath = Path.GetDirectoryName(typeof(object).Assembly.Location)!;

		var sourceTextCompilation = CSharpCompilation.Create(
			assemblyName: "Tests", 
			syntaxTrees: new [] {syntaxTree}, 
			new[] {
				MetadataReference.CreateFromFile(Path.Combine(dotNetAssemblyPath, "mscorlib.dll")), 
				MetadataReference.CreateFromFile(Path.Combine(dotNetAssemblyPath, "System.dll")), 
				MetadataReference.CreateFromFile(Path.Combine(dotNetAssemblyPath, "System.Core.dll")), 
				MetadataReference.CreateFromFile(Path.Combine(dotNetAssemblyPath, "System.Private.CoreLib.dll")),
				MetadataReference.CreateFromFile(Path.Combine(dotNetAssemblyPath, "System.Runtime.dll")), 
				MetadataReference.CreateFromFile(Path.Combine(dotNetAssemblyPath, "netstandard.dll")), 
				MetadataReference.CreateFromFile(typeof(CustomAssertionAttribute).Assembly.Location),
				MetadataReference.CreateFromFile(typeof(GenerateEventualAttribute).Assembly.Location),
				MetadataReference.CreateFromFile(typeof(EventualAssertions).Assembly.Location),
				MetadataReference.CreateFromFile(typeof(Attribute).Assembly.Location),
			}, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary).WithNullableContextOptions(NullableContextOptions.Enable));

		GeneratorDriver driver = CSharpGeneratorDriver.Create(new EventualAssertionsGenerator());

		driver = driver.RunGeneratorsAndUpdateCompilation(sourceTextCompilation, out var outputCompilation, out var _);

		sourceTextCompilation.GetDiagnostics().Should().BeEmpty();

		var afterGenerationDiagnostics = outputCompilation.GetDiagnostics();
		
		var settings = new VerifySettings();
		settings.UseUniqueDirectory();
		settings.AddScrubber((sb, c) =>
		{
			var s = sb.ToString();
			var generatedCodeAttributes =
				Regex.Matches(s,
					"""
					\[System.CodeDom.Compiler.GeneratedCodeAttribute\("mazharenko.FluentAssertions.Eventual"\, \"[^\"]+\"\)\]
					""");
			foreach (Match match in generatedCodeAttributes)
			{
				sb.Replace(match.Value,
					"""
					[System.CodeDom.Compiler.GeneratedCodeAttribute("mazharenko.FluentAssertions.Eventual", "<version>"]
					""");
			}
		});
		await Verifier.Verify(driver, settings);

		afterGenerationDiagnostics.Should().BeEmpty();
	}
}