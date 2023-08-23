using System.Threading.Tasks;
using NUnit.Framework;

namespace mazharenko.FluentAssertions.Eventual.Generator.Tests;


public class NestedTests : BaseTests
{
	[Test]
	public async Task Should_Generate_FileScopedNamespaces()
	{
		const string source = @"
namespace Namespace;

using mazharenko.FluentAssertions.Eventual;
using FluentAssertions;

[GenerateEventual]
public class Assertions
{
}
";
		await RunAndVerify(source);
	}

	[Test]
	public async Task Should_Generate_NestedNamespaces()
	{
		const string source = @"
using mazharenko.FluentAssertions.Eventual;
using FluentAssertions;

namespace Namespace1
{
	namespace Namespace2
	{
		namespace Namespace3
		{
			[GenerateEventual]
			public class Assertions
			{
			}
		}
	}
}
";
		await RunAndVerify(source);
	}

	[Test]
	public async Task Should_Generate_NoNamespace()
	{
		const string source = @"
using mazharenko.FluentAssertions.Eventual;
using FluentAssertions;

[GenerateEventual]
public class Assertions
{
}
";
		await RunAndVerify(source);
	}
}