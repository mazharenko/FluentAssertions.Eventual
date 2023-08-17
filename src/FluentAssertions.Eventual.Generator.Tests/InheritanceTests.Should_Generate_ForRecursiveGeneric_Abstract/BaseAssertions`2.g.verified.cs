﻿//HintName: BaseAssertions`2.g.cs
// <auto-generated />
using mazharenko.FluentAssertions.Eventual;
using FluentAssertions;
using FluentAssertions.Primitives;

namespace Namespace
{
	[System.CodeDom.Compiler.GeneratedCodeAttribute("mazharenko.FluentAssertions.Eventual", "4.0.1")]
	public abstract class BaseAssertions_Eventual<TSubject, TAssertions>
		where TSubject : class where TAssertions : BaseAssertions<TSubject, TAssertions>
	{
		private readonly BaseAssertions<TSubject, TAssertions> underlying;
		private readonly System.TimeSpan timeout;
		private readonly System.TimeSpan delay;
		public BaseAssertions_Eventual(BaseAssertions<TSubject, TAssertions> underlying, System.TimeSpan timeout, System.TimeSpan delay)
		{
			this.underlying = underlying;
			this.timeout = timeout;
			this.delay = delay;
		}
	}
}