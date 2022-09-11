using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using mazharenko.FluentAssertions.Extensions.Eventual;

namespace mazharenko.FluentAssertions.Extensions;

[PublicAPI]
public static class EventualAssertions
{
	public static IEnumerable<Attempt> Attempts(TimeSpan timeout, TimeSpan delay)
	{
		return new Attempts(timeout, delay);
	}
	
	public static IEnumerable<Attempt> Attempts(int timeoutMs, int delayMs)
		=> Attempts(TimeSpan.FromMilliseconds(timeoutMs), TimeSpan.FromMilliseconds(delayMs));
	
	public static IAsyncEnumerable<Attempt> AttemptsAsync(TimeSpan timeout, TimeSpan delay)
	{
		return new Attempts(timeout, delay);
	}
	
	public static IAsyncEnumerable<Attempt> AttemptsAsync(int timeoutMs, int delayMs)
		=> AttemptsAsync(TimeSpan.FromMilliseconds(timeoutMs), TimeSpan.FromMilliseconds(delayMs));
}