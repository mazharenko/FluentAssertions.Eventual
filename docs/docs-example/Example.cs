namespace docs_example;

using System;
using FluentAssertions;
using FluentAssertions.Execution;
using mazharenko.FluentAssertions.Eventual;

public class CurrentDateTime
{
	public DateTime Now => DateTime.Now;
}

public static class CurrentDateTimeExtensions
{
	public static CurrentDateTimeAssertions Should(this CurrentDateTime currentDateTime)
	{
		return new CurrentDateTimeAssertions(currentDateTime);
	}
}

[GenerateEventual]
public class CurrentDateTimeAssertions
{
	public CurrentDateTime Subject { get; }
	public CurrentDateTimeAssertions(CurrentDateTime currentDateTime)
	{
		Subject = currentDateTime;
	}

	[CustomAssertion]
	public AndConstraint<CurrentDateTimeAssertions> BeAfter(DateTime expected, string? because = null, params object[] becauseArgs)
	{
		var now = Subject.Now;
		Execute.Assertion    
			.ForCondition(now > expected)
			.BecauseOf(because, becauseArgs)
			.FailWith("Expected {context:the date and time} to be after {0}{reason}, but found {1}.", expected, now);
				
		return new AndConstraint<CurrentDateTimeAssertions>(this);
	}
}
