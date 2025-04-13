using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using TUnit.Assertions.AssertConditions.Interfaces;
using TUnit.Assertions.AssertionBuilders.Wrappers;

namespace Extensions.Logging.InMemory.Tests;

public static class AssertionExtensions
{
    public static EquivalentToAssertionBuilderWrapper<TActual, TExpected> IsPartiallyEquivalentTo<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields |
                                    DynamicallyAccessedMemberTypes.NonPublicFields |
                                    DynamicallyAccessedMemberTypes.PublicProperties |
                                    DynamicallyAccessedMemberTypes.NonPublicProperties)]
        TActual,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields |
                                    DynamicallyAccessedMemberTypes.NonPublicFields |
                                    DynamicallyAccessedMemberTypes.PublicProperties |
                                    DynamicallyAccessedMemberTypes.NonPublicProperties)]
        TExpected>(this IValueSource<TActual> valueSource, TExpected expected,
        [CallerArgumentExpression(nameof(expected))] string? callerMemberExpression = null)
    {
        return valueSource
            .IsEquivalentTo(expected, callerMemberExpression!)
            .WithPartialEquivalency();
    }
}