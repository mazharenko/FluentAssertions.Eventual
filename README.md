[![Nuget package](https://img.shields.io/nuget/v/mazharenko.FluentAssertions.Eventual.svg?label=csharp&logo=nuget)](https://www.nuget.org/packages/mazharenko.FluentAssertions.Eventual/)
[![Nuget package](https://img.shields.io/nuget/v/mazharenko.FluentAssertions.Eventual.FSharp.svg?label=fsharp&logo=nuget)](https://www.nuget.org/packages/mazharenko.FluentAssertions.Eventual.FSharp/)
[![Nuget package](https://img.shields.io/nuget/v/mazharenko.FluentAssertions.Eventual.Generator.svg?label=source%20generator&logo=nuget&labelColor=red)](https://www.nuget.org/packages/mazharenko.FluentAssertions.Eventual.Generator/)

[![License: CC0-1.0](https://img.shields.io/badge/License-CC0_1.0-lightgrey.svg)](LICENSE)


# Eventual assertions for FluentAssertions

`FluentAssertions.Eventual` is an extension that allows to wait for `FluentAssertions` checks to pass which can be useful when writing end-to-end tests.

## Basic usage

Any `FluentAssertions` checks can be placed under a special `foreach` loop, which will implement the waiting and retry logic.

```csharp
foreach (var _ in EventualAssertions.Attempts(4.Seconds(), 400.Milliseconds()))
{
    button.Should().BeVisible();
}
```

## Source generator usage

When having a custom assertion class for a dynamic by nature subject the class can be decorated by the `[GenerateEventual]` attribute to get a special waiting wrapper generated.

Your code (simplified):

```csharp
[GenerateEventual]
public class ButtonAssertions
{
    [CustomAssertion]
    public AndConstraint<ButtonAssertions> BeVisible(string? because = null, params object[] becauseArgs)
    {
        // implementation
    }
}
```

What gets generated (simplified):

```csharp
public static class ButtonAssertions_Eventual_Extensions
{
    public static ButtonAssertions_Eventual Eventually(this ButtonAssertions underlying) { /* ... */ }
    // more generated extensions
}

public class ButtonAssertions_Eventual
{
    // constructor, fields

    [CustomAssertion]
    public AndConstraint<ButtonAssertions> BeVisible(string? because = null, params object[] becauseArgs)
    {
        AndConstraint<CurrentDateTimeAssertions> result = default !;
        foreach (var _  in EventualAssertions.Attempts(timeout, delay))
            result = underlying.BeVisible(because, becauseArgs);
        return result;
    }
}
```

Which allows for the following syntax:

```csharp
button.Should().Eventually().BeVisible();
button.Should().Eventually(4.Seconds(), 400.Milliseconds()).BeVisible();
button.Should().EventuallyLong().BeVisible();
```

## More info

Complete interactive README in mybinder:\
[![Binder](https://mybinder.org/badge_logo.svg)](https://mybinder.org/v2/gh/mazharenko/FluentAssertions.Eventual/HEAD?urlpath=lab/tree/README.ipynb)

Also in nbviewer:\
[![nbviewer](https://raw.githubusercontent.com/jupyter/design/master/logos/Badges/nbviewer_badge.svg)](https://nbviewer.org/github/mazharenko/FluentAssertions.Eventual/tree/HEAD/docs/README.ipynb)
