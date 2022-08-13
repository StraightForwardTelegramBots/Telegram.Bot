using System;
using System.Runtime.CompilerServices;
using Telegram.Bot.Types.Abstractions;

namespace Telegram.Bot.Extensions;

/// <summary>
/// Extension Methods
/// </summary>
internal static class ObjectExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static T ThrowIfNull<T>(this T? value, string parameterName) =>
        value ?? throw new ArgumentNullException(parameterName);

    internal static ITelegramBotClient FromCarrier<T>(this T value) where T : IClientCarrier =>
        value.Client ?? throw new InvalidOperationException("Something wrong! IClientCarrier should carry a Client.");

    internal static T? FakeIfBool<T>(this bool b) where T : class => null;
}
