using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Telegram.Bot.Types;

namespace Telegram.Bot.Polling;

/// <summary>
/// A very simple <see cref="IUpdateHandler"/> implementation
/// </summary>
[PublicAPI]
public class DefaultUpdateHandler : IUpdateHandler
{
    readonly Func<ITelegramBotClient, Update, CancellationToken, Task> _updateHandler;
    readonly Func<ITelegramBotClient, Exception, CancellationToken, Task> _pollingErrorHandler;

    private Task DefaultErrorHandlerAsync(
        ITelegramBotClient _,
        Exception exception,
        CancellationToken __)
    {
        Console.WriteLine(exception);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Constructs a new <see cref="DefaultUpdateHandler"/> with the specified callback functions
    /// </summary>
    /// <param name="updateHandler">The function to invoke when an update is received</param>
    /// <param name="pollingErrorHandler">The function to invoke when an error occurs</param>
    public DefaultUpdateHandler(
        Func<ITelegramBotClient, Update, CancellationToken, Task> updateHandler,
        Func<ITelegramBotClient, Exception, CancellationToken, Task>? pollingErrorHandler = default)
    {
        _updateHandler = updateHandler ?? throw new ArgumentNullException(nameof(updateHandler));
        _pollingErrorHandler = pollingErrorHandler ?? DefaultErrorHandlerAsync;
    }

    /// <inheritdoc />
    public async Task HandleUpdateAsync(
        ITelegramBotClient botClient,
        Update update,
        CancellationToken cancellationToken
    ) =>
        await _updateHandler(botClient, update, cancellationToken).ConfigureAwait(false);

    /// <inheritdoc />
    public async Task HandlePollingErrorAsync(
        ITelegramBotClient botClient,
        Exception exception,
        CancellationToken cancellationToken
    ) =>
        await _pollingErrorHandler(botClient, exception, cancellationToken).ConfigureAwait(false);
}
