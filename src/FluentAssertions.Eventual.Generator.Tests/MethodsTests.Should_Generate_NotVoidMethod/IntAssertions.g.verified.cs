﻿//HintName: IntAssertions.g.cs
// <auto-generated />
#pragma warning disable CS8019, CS8933
using mazharenko.FluentAssertions.Eventual;
using FluentAssertions;
using FluentAssertions.Execution;

namespace Namespace
{
	[System.CodeDom.Compiler.GeneratedCodeAttribute("mazharenko.FluentAssertions.Eventual", "4.0.2")]
	public static class IntAssertions_Eventual_Extensions
	{
		public static IntAssertions_Eventual Eventually(this IntAssertions underlying)
		{
			return Eventually(underlying, System.TimeSpan.FromSeconds(5), System.TimeSpan.FromMilliseconds(100));
		}

		public static IntAssertions_Eventual EventuallyLong(this IntAssertions underlying)
		{
			return Eventually(underlying, System.TimeSpan.FromSeconds(20), System.TimeSpan.FromMilliseconds(500));
		}

		public static IntAssertions_Eventual Eventually(this IntAssertions underlying, System.TimeSpan timeout, System.TimeSpan delay)
		{
			return new IntAssertions_Eventual(underlying, timeout, delay);
		}
	}

	[System.CodeDom.Compiler.GeneratedCodeAttribute("mazharenko.FluentAssertions.Eventual", "4.0.2")]
	public class IntAssertions_Eventual
	{
		private readonly IntAssertions underlying;
		private readonly System.TimeSpan timeout;
		private readonly System.TimeSpan delay;
		public IntAssertions_Eventual(IntAssertions underlying, System.TimeSpan timeout, System.TimeSpan delay)
		{
			this.underlying = underlying;
			this.timeout = timeout;
			this.delay = delay;
		}

		[CustomAssertion]
		public AndConstraint<IntAssertions> Be(int expected, string because = null)
		{
			AndConstraint<IntAssertions> result = default;
			foreach (var _  in EventualAssertions.Attempts(timeout, delay))
				result = underlying.Be(expected, because);
			return result;
		}
	}
}