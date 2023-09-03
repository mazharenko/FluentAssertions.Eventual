﻿//HintName: Assertions`2.g.cs
// <auto-generated />
#pragma warning disable CS8019, CS8933
#nullable restore
using mazharenko.FluentAssertions.Eventual;

namespace Namespace
{
	[System.CodeDom.Compiler.GeneratedCodeAttribute("mazharenko.FluentAssertions.Eventual", "4.0.2")]
	public static class Assertions_Eventual_2_Extensions
	{
		public static Assertions_Eventual<T1, T2> Eventually<T1, T2>(this Assertions<T1, T2> underlying)
		{
			return Eventually(underlying, System.TimeSpan.FromSeconds(5), System.TimeSpan.FromMilliseconds(100));
		}

		public static Assertions_Eventual<T1, T2> EventuallyLong<T1, T2>(this Assertions<T1, T2> underlying)
		{
			return Eventually(underlying, System.TimeSpan.FromSeconds(20), System.TimeSpan.FromMilliseconds(500));
		}

		public static Assertions_Eventual<T1, T2> Eventually<T1, T2>(this Assertions<T1, T2> underlying, System.TimeSpan timeout, System.TimeSpan delay)
		{
			return new Assertions_Eventual<T1, T2>(underlying, timeout, delay);
		}
	}

	[System.CodeDom.Compiler.GeneratedCodeAttribute("mazharenko.FluentAssertions.Eventual", "4.0.2")]
	public class Assertions_Eventual<T1, T2>
	{
		private readonly Assertions<T1, T2> underlying;
		private readonly System.TimeSpan timeout;
		private readonly System.TimeSpan delay;
		public Assertions_Eventual(Assertions<T1, T2> underlying, System.TimeSpan timeout, System.TimeSpan delay)
		{
			this.underlying = underlying;
			this.timeout = timeout;
			this.delay = delay;
		}
	}
}
#pragma warning restore CS8019, CS8933
