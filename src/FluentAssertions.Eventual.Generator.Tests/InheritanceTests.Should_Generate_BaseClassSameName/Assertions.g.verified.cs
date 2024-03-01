﻿//HintName: Assertions.g.cs
// <auto-generated />
#pragma warning disable CS8019, CS8933
#nullable restore
using mazharenko.FluentAssertions.Eventual;

namespace Namespace
{
	[System.CodeDom.Compiler.GeneratedCodeAttribute("mazharenko.FluentAssertions.Eventual", "<version>"]
	public static class Assertions_Eventual_Extensions
	{
		/// <summary>
		/// Returns a <see cref="T:Namespace.Assertions_Eventual"/> wrapper that adds waiting to the current <see cref="T:Namespace.Assertions"/>
		/// </summary>
		public static Assertions_Eventual Eventually(this Assertions underlying)
		{
			return Eventually(underlying, System.TimeSpan.FromSeconds(5), System.TimeSpan.FromMilliseconds(100));
		}

		/// <summary>
		/// Returns a <see cref="T:Namespace.Assertions_Eventual"/> wrapper that adds waiting to the current <see cref="T:Namespace.Assertions"/>
		/// </summary>
		public static Assertions_Eventual EventuallyLong(this Assertions underlying)
		{
			return Eventually(underlying, System.TimeSpan.FromSeconds(20), System.TimeSpan.FromMilliseconds(500));
		}

		/// <summary>
		/// Returns a <see cref="T:Namespace.Assertions_Eventual"/> wrapper that adds waiting to the current <see cref="T:Namespace.Assertions"/>
		/// </summary>
		public static Assertions_Eventual Eventually(this Assertions underlying, System.TimeSpan timeout, System.TimeSpan delay)
		{
			return new Assertions_Eventual(underlying, timeout, delay);
		}
	}

	[System.CodeDom.Compiler.GeneratedCodeAttribute("mazharenko.FluentAssertions.Eventual", "<version>"]
	public class Assertions_Eventual : Assertions_Eventual<string>
	{
		private readonly Assertions underlying;
		private readonly System.TimeSpan timeout;
		private readonly System.TimeSpan delay;
		public Assertions_Eventual(Assertions underlying, System.TimeSpan timeout, System.TimeSpan delay) : base(underlying, timeout, delay)
		{
			this.underlying = underlying;
			this.timeout = timeout;
			this.delay = delay;
		}
	}
}
#pragma warning restore CS8019, CS8933
