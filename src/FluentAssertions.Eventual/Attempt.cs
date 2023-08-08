using System;
using JetBrains.Annotations;

namespace mazharenko.FluentAssertions.Eventual;

[PublicAPI]
public readonly record struct Attempt(int Number, TimeSpan Elapsed);