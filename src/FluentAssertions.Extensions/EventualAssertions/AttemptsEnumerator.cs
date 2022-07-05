using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions.Execution;
using JetBrains.Annotations;

namespace mazharenko.FluentAssertions.Extensions.Eventual;

[PublicAPI]
public class AttemptsEnumerator : IEnumerator<Attempt>, IAsyncEnumerator<Attempt>
{
	private readonly TimeSpan timeout;
	private readonly TimeSpan delay;
	private readonly AssertionScope assertionScope;
	private readonly Stopwatch stopwatch = new();

	private static readonly Attempt InitialAttempt = new(0, TimeSpan.Zero);
	private Attempt attempt = InitialAttempt;
	
	internal AttemptsEnumerator(string? context, TimeSpan timeout, TimeSpan delay)
	{
		this.timeout = timeout;
		this.delay = delay;
		assertionScope = context is null ? new AssertionScope() : new AssertionScope(context);
	}

	public bool MoveNext()
	{
		if (!stopwatch.IsRunning)
			stopwatch.Start();
		
		if (stopwatch.Elapsed > timeout)
			return false;

		if (attempt == InitialAttempt)
		{
			attempt = new Attempt(attempt.Number + 1, stopwatch.Elapsed);
			return true;
		}

		if (!assertionScope.HasFailures())
			return false;
		
		attempt = new Attempt(attempt.Number + 1, stopwatch.Elapsed);
		assertionScope.Discard();
		
		Thread.Sleep(delay);
		
		return true;
	}

	public async ValueTask<bool> MoveNextAsync()
	{
		if (!stopwatch.IsRunning)
			stopwatch.Start();
		
		if (stopwatch.Elapsed > timeout)
			return false;

		if (attempt == InitialAttempt)
		{
			attempt = new Attempt(attempt.Number + 1, stopwatch.Elapsed);
			return true;
		}

		if (!assertionScope.HasFailures())
			return false;
		
		attempt = new Attempt(attempt.Number + 1, stopwatch.Elapsed);
		assertionScope.Discard();
		
		await Task.Delay(delay).ConfigureAwait(false);
		
		return true;
	}

	public void Reset()
	{
		assertionScope.Discard();
	}

	public Attempt Current => attempt;

	object IEnumerator.Current => Current;

	public void Dispose()
	{
		assertionScope.Dispose();
	}

	public ValueTask DisposeAsync()
	{
		Dispose();
		return default;
	}
}