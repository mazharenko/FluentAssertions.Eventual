﻿//HintName: Assertions`1.g.cs
// <auto-generated />
#pragma warning disable CS8019, CS8933
#nullable restore
using mazharenko.FluentAssertions.Eventual;

namespace Namespace
{
	[System.CodeDom.Compiler.GeneratedCodeAttribute("mazharenko.FluentAssertions.Eventual", "<version>"]
	public static class Assertions_Eventual_1_Extensions
	{
		/// <summary>
		/// Returns a <see cref="T:Namespace.Assertions_Eventual`1"/> wrapper that adds waiting to the current <see cref="T:Namespace.Assertions`1"/>
		/// </summary>
		public static Assertions_Eventual<T> Eventually<T>(this Assertions<T> underlying)
		{
			return Eventually(underlying, System.TimeSpan.FromSeconds(5), System.TimeSpan.FromMilliseconds(100));
		}

		/// <summary>
		/// Returns a <see cref="T:Namespace.Assertions_Eventual`1"/> wrapper that adds waiting to the current <see cref="T:Namespace.Assertions`1"/>
		/// </summary>
		public static Assertions_Eventual<T> EventuallyLong<T>(this Assertions<T> underlying)
		{
			return Eventually(underlying, System.TimeSpan.FromSeconds(20), System.TimeSpan.FromMilliseconds(500));
		}

		/// <summary>
		/// Returns a <see cref="T:Namespace.Assertions_Eventual`1"/> wrapper that adds waiting to the current <see cref="T:Namespace.Assertions`1"/>
		/// </summary>
		public static Assertions_Eventual<T> Eventually<T>(this Assertions<T> underlying, System.TimeSpan timeout, System.TimeSpan delay)
		{
			return new Assertions_Eventual<T>(underlying, timeout, delay);
		}
	}

	[System.CodeDom.Compiler.GeneratedCodeAttribute("mazharenko.FluentAssertions.Eventual", "<version>"]
	public class Assertions_Eventual<T> : Assertions_Eventual<T, string>
	{
		private readonly Assertions<T> underlying;
		private readonly System.TimeSpan timeout;
		private readonly System.TimeSpan delay;
		public Assertions_Eventual(Assertions<T> underlying, System.TimeSpan timeout, System.TimeSpan delay) : base(underlying, timeout, delay)
		{
			this.underlying = underlying;
			this.timeout = timeout;
			this.delay = delay;
		}
	}
}
#pragma warning restore CS8019, CS8933
