using System;
using System.IO;
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
			}, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

		GeneratorDriver driver = CSharpGeneratorDriver.Create(new EventualAssertionsGenerator());

		driver = driver.RunGeneratorsAndUpdateCompilation(sourceTextCompilation, out var outputCompilation, out var _);

		if (!sourceTextCompilation.GetDiagnostics().IsEmpty)
			sourceTextCompilation.GetDiagnostics().Should().OnlyContain(d => d.Severity < DiagnosticSeverity.Error);

		var afterGenerationDiagnostics = outputCompilation.GetDiagnostics();
		
		var settings = new VerifySettings();
		settings.UseUniqueDirectory();
		await Verifier.Verify(driver, settings);

		if (!afterGenerationDiagnostics.IsEmpty)
			afterGenerationDiagnostics.Should().OnlyContain(d => d.Severity < DiagnosticSeverity.Error);

	}
}