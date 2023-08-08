using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions.Execution;
using Vostok.Commons.Time;

namespace mazharenko.FluentAssertions.Eventual;

internal class AttemptsEnumerator : IEnumerator<Attempt>, IAsyncEnumerator<Attempt>
{
	private enum State
	{
		Initial,
		Waiting,
		LastTry
	}
	
	private readonly TimeSpan timeout;
	private readonly TimeSpan delay;
	
	private State state = State.Initial;
	private AssertionScope? assertionScope;
	private TimeBudget timeBudget;

	internal AttemptsEnumerator(TimeSpan timeout, TimeSpan delay)
	{
		this.timeout = timeout;
		this.delay = delay;
		
		assertionScope = new AssertionScope();
		Current = new Attempt(0, TimeSpan.Zero);
		timeBudget = TimeBudget.CreateNew(timeout);
	}

	public bool MoveNext()
	{
		timeBudget.Start();
		
		switch (state)
		{
			case State.Initial:
				Current = new Attempt(Current.Number + 1, timeBudget.Elapsed);
				state = State.Waiting;
				return true;
			case State.Waiting:
				if (assertionScope is null || !assertionScope.HasFailures())
					return false;
				if (timeBudget.HasExpired)
				{
					assertionScope.Discard();
					assertionScope.Dispose();
					assertionScope = null;
					state = State.LastTry;
					Current = new Attempt(Current.Number + 1, timeBudget.Elapsed);
					return true;
				}
				assertionScope.Discard();
				
				Thread.Sleep(delay);
				
				Current = new Attempt(Current.Number + 1, timeBudget.Elapsed);
				return true;
			case State.LastTry:
				return false;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}
	
	public async ValueTask<bool> MoveNextAsync()
	{
		timeBudget.Start();
		
		switch (state)
		{
			case State.Initial:
				Current = new Attempt(Current.Number + 1, timeBudget.Elapsed);
				state = State.Waiting;
				return true;
			case State.Waiting:
				if (assertionScope is null || !assertionScope.HasFailures())
					return false;
				if (timeBudget.HasExpired)
				{
					assertionScope.Discard();
					assertionScope.Dispose();
					assertionScope = null;
					state = State.LastTry;
					Current = new Attempt(Current.Number + 1, timeBudget.Elapsed);
					return true;
				}
				assertionScope.Discard();

				await Task.Delay(delay).ConfigureAwait(false);
				
				Current = new Attempt(Current.Number + 1, timeBudget.Elapsed);
				return true;
			case State.LastTry:
				return false;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}

	public void Reset()
	{
		assertionScope?.Discard();
		Current = new Attempt(0, TimeSpan.Zero);
		timeBudget = TimeBudget.CreateNew(timeout);
		state = State.Initial;
	}
	public Attempt Current { get; private set; }


	object IEnumerator.Current => Current;

	public void Dispose()
	{
		assertionScope?.Dispose();
		assertionScope = null;
	}

	public ValueTask DisposeAsync()
	{
		Dispose();
		return default;
	}
}