using System;
using JetBrains.Annotations;

namespace mazharenko.FluentAssertions.Extensions;

[PublicAPI]
public readonly record struct Attempt(int Number, TimeSpan Elapsed);