using System.Runtime.CompilerServices;
using VerifyTests;

namespace mazharenko.FluentAssertions.Eventual.Generator.Tests;

public static class ModuleInitializer
{
	[ModuleInitializer]
	public static void Init() =>
		VerifySourceGenerators.Initialize();
}