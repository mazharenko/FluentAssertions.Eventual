﻿//HintName: Assertions.g.cs
// <auto-generated />
using mazharenko.FluentAssertions.Eventual;
using FluentAssertions;

namespace Namespace
{
	[System.CodeDom.Compiler.GeneratedCodeAttribute("mazharenko.FluentAssertions.Eventual", "4.0.1")]
	public static class Assertions_Eventual_Extensions
	{
		public static Assertions_Eventual Eventually(this Assertions underlying)
		{
			return Eventually(underlying, System.TimeSpan.FromSeconds(5), System.TimeSpan.FromMilliseconds(100));
		}

		public static Assertions_Eventual EventuallyLong(this Assertions underlying)
		{
			return Eventually(underlying, System.TimeSpan.FromSeconds(20), System.TimeSpan.FromMilliseconds(500));
		}

		public static Assertions_Eventual Eventually(this Assertions underlying, System.TimeSpan timeout, System.TimeSpan delay)
		{
			return new Assertions_Eventual(underlying, timeout, delay);
		}
	}

	[System.CodeDom.Compiler.GeneratedCodeAttribute("mazharenko.FluentAssertions.Eventual", "4.0.1")]
	public class Assertions_Eventual
	{
		private readonly Assertions underlying;
		private readonly System.TimeSpan timeout;
		private readonly System.TimeSpan delay;
		public Assertions_Eventual(Assertions underlying, System.TimeSpan timeout, System.TimeSpan delay)
		{
			this.underlying = underlying;
			this.timeout = timeout;
			this.delay = delay;
		}
	}
}