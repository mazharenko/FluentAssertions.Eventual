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
		return new Attempts(null, timeout, delay);
	}
	
	public static IEnumerable<Attempt> Attempts(string context, TimeSpan timeout, TimeSpan delay)
	{
		return new Attempts(context, timeout, delay);
	}

	public static IEnumerable<Attempt> Attempts(int timeoutMs, int delayMs)
		=> Attempts(TimeSpan.FromMilliseconds(timeoutMs), TimeSpan.FromMilliseconds(delayMs));
	
	public static IEnumerable<Attempt> Attempts(string context, int timeoutMs, int delayMs)
		=> Attempts(context, TimeSpan.FromMilliseconds(timeoutMs), TimeSpan.FromMilliseconds(delayMs));
	
	public static IAsyncEnumerable<Attempt> AttemptsAsync(TimeSpan timeout, TimeSpan delay)
	{
		return new Attempts(null, timeout, delay);
	}
	
	public static IAsyncEnumerable<Attempt> AttemptsAsync(string context, TimeSpan timeout, TimeSpan delay)
	{
		return new Attempts(context, timeout, delay);
	}
	
	public static IAsyncEnumerable<Attempt> AttemptsAsync(int timeoutMs, int delayMs)
		=> AttemptsAsync(TimeSpan.FromMilliseconds(timeoutMs), TimeSpan.FromMilliseconds(delayMs));
	
	public static IAsyncEnumerable<Attempt> AttemptsAsync(string context, int timeoutMs, int delayMs)
		=> AttemptsAsync(context, TimeSpan.FromMilliseconds(timeoutMs), TimeSpan.FromMilliseconds(delayMs));
}