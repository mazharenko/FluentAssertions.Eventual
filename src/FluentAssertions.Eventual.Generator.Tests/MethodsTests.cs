using System.Threading.Tasks;
using NUnit.Framework;

namespace mazharenko.FluentAssertions.Eventual.Generator.Tests;


public class MethodsTests : BaseTests
{
	[Test]
	public async Task Should_Generate_VoidMethod()
	{
		const string source = @"
using mazharenko.FluentAssertions.Eventual;
using FluentAssertions;
using FluentAssertions.Execution;

namespace Namespace
{
	[GenerateEventual]
	public class IntAssertions
	{
		protected int Subject {get;}
		public IntAssertions(int subject)
		{
			Subject = subject;
		}

		[CustomAssertion]
		public void Be(int expected, string because = null)
		{
			Execute.Assertion
				.ForCondition(Subject == expected)
				.BecauseOf(because);
		}
	}
}
";

		await RunAndVerify(source);
	}

	[Test]
	public async Task Should_Generate_NotVoidMethod()
	{
		const string source = @"
using mazharenko.FluentAssertions.Eventual;
using FluentAssertions;
using FluentAssertions.Execution;

namespace Namespace
{
	[GenerateEventual]
	public class IntAssertions
	{
		protected int Subject {get;}
		public IntAssertions(int subject)
		{
			Subject = subject;
		}

		[CustomAssertion]
		public AndConstraint<IntAssertions> Be(int expected, string because = null)
		{
			Execute.Assertion
				.ForCondition(Subject == expected)
				.BecauseOf(because);
			return new AndConstraint<IntAssertions>(this);
		}
	}
}
";
		await RunAndVerify(source);
	}
}