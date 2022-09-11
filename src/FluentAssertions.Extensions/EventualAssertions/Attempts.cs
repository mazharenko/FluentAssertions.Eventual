using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using JetBrains.Annotations;

namespace mazharenko.FluentAssertions.Extensions.Eventual;

[PublicAPI]
public class Attempts : IEnumerable<Attempt>, IAsyncEnumerable<Attempt>
{
	private readonly TimeSpan timeout;
	private readonly TimeSpan delay;

	internal Attempts(TimeSpan timeout, TimeSpan delay)
	{
		this.timeout = timeout;
		this.delay = delay;
	}

	public IEnumerator<Attempt> GetEnumerator()
	{
		return new AttemptsEnumerator(timeout, delay);
	}

	public IAsyncEnumerator<Attempt> GetAsyncEnumerator(CancellationToken cancellationToken = new())
	{
		return new AttemptsEnumerator(timeout, delay);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}