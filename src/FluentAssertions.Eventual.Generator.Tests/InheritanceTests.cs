using System.Threading.Tasks;
using NUnit.Framework;

namespace mazharenko.FluentAssertions.Eventual.Generator.Tests;


public class InheritanceTests : BaseTests
{
	[Test]
	public async Task Should_Generate_ForRecursiveGeneric()
	{
		const string source = @"
using mazharenko.FluentAssertions.Eventual;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace Namespace
{
	[GenerateEventual]
	public class BaseAssertions<TSubject, TAssertions> : ReferenceTypeAssertions<TSubject, TAssertions> 
		where TSubject : class
		where TAssertions : BaseAssertions<TSubject, TAssertions>
	{
		protected override string Identifier => ""object"";

		protected BaseAssertions(TSubject subject) : base(subject)
		{
		}
	}

	[GenerateEventual]
	public class Assertions : BaseAssertions<string, Assertions>
	{
		public Assertions(string subject) : base(subject)
		{}
	}
}
";
		await RunAndVerify(source);
	}

	[Test]
	public async Task Should_Generate_BaseClassSameName()
	{
		const string source = @"
using mazharenko.FluentAssertions.Eventual;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace Namespace
{
	[GenerateEventual]
	public class Assertions<T1, T2>
	{
	}

	[GenerateEventual]
	public class Assertions<T> : Assertions<T, string>
	{
	}

	[GenerateEventual]
	public class Assertions : Assertions<string>
	{
	}
}
";
		await RunAndVerify(source);
	}

	[Test]
	public async Task Should_Generate_ForRecursiveGeneric_Abstract()
	{
		const string source = @"
using mazharenko.FluentAssertions.Eventual;
using FluentAssertions;
using FluentAssertions.Primitives;

namespace Namespace
{
	[GenerateEventual]
	public abstract class BaseAssertions<TSubject, TAssertions> : ReferenceTypeAssertions<TSubject, TAssertions> 
		where TSubject : class
		where TAssertions : BaseAssertions<TSubject, TAssertions>
	{
		protected override string Identifier => ""object"";

		protected BaseAssertions(TSubject subject) : base(subject)
		{
		}
	}

	[GenerateEventual]
	public class Assertions : BaseAssertions<string, Assertions>
	{
		public Assertions(string subject) : base(subject)
		{}
	}
}
";
		await RunAndVerify(source);
	}

	[Test]
	public async Task Should_Generate_BaseClassWithoutAttribute()
	{
		const string source = @"
using mazharenko.FluentAssertions.Eventual;

namespace Namespace
{
	public abstract class BaseAssertions
	{
	}

	[GenerateEventual]
	public class Assertions : BaseAssertions
	{
	}
}
";
		await RunAndVerify(source);
	}
}