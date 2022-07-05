using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using JetBrains.Annotations;
using mazharenko.FluentAssertions.Extensions.Eventual;

namespace mazharenko.FluentAssertions.Extensions.Eventual;

[PublicAPI]
public class Attempts : IEnumerable<Attempt>, IAsyncEnumerable<Attempt>
{
	private readonly string? context;
	private readonly TimeSpan timeout;
	private readonly TimeSpan delay;

	internal Attempts(string? context, TimeSpan timeout, TimeSpan delay)
	{
		this.context = context;
		this.timeout = timeout;
		this.delay = delay;
	}

	public IEnumerator<Attempt> GetEnumerator()
	{
		return new AttemptsEnumerator(context, timeout, delay);
	}

	public IAsyncEnumerator<Attempt> GetAsyncEnumerator(CancellationToken cancellationToken = new())
	{
		return new AttemptsEnumerator(context, timeout, delay);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}