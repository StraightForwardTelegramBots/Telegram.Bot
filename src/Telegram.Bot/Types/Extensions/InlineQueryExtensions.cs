using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Extensions;
using Telegram.Bot.Requests;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Bot.Types;

/// <summary>
/// A set of extension methods for <see cref="InlineQuery"/>.
/// </summary>
public static class InlineQueryExtensions
{

    /// <summary>
    /// Use this method to send answers to an inline query.
    /// </summary>
    /// <remarks>
    /// No more than <b>50</b> results per query are allowed.
    /// </remarks>
    /// <param name="inlineQuery"></param>
    /// <param name="results">An array of results for the inline query</param>
    /// <param name="cacheTime">
    /// The maximum amount of time in seconds that the result of the inline query may be cached on the server.
    /// Defaults to 300
    /// </param>
    /// <param name="isPersonal">
    /// Pass <c>true</c>, if results may be cached on the server side only for the user that sent the query.
    /// By default, results may be returned to any user who sends the same query
    /// </param>
    /// <param name="nextOffset">
    /// Pass the offset that a client should send in the next query with the same text to receive more results.
    /// Pass an empty string if there are no more results or if you don't support pagination.
    /// Offset length can't exceed 64 bytes
    /// </param>
    /// <param name="switchPmText">
    /// If passed, clients will display a button with specified text that switches the user to a private chat
    /// with the bot and sends the bot a start message with the parameter <paramref name="switchPmParameter"/>
    /// </param>
    /// <param name="switchPmParameter">
    /// <a href="https://core.telegram.org/bots#deep-linking">Deep-linking</a> parameter for the <c>/start</c>
    /// message sent to the bot when user presses the switch button. 1-64 characters, only <c>A-Z</c>, <c>a-z</c>,
    /// <c>0-9</c>, <c>_</c> and <c>-</c> are allowed
    /// <para>
    /// <i>Example</i>: An inline bot that sends YouTube videos can ask the user to connect the bot to their
    /// YouTube account to adapt search results accordingly. To do this, it displays a 'Connect your YouTube
    /// account' button above the results, or even before showing any. The user presses the button, switches
    /// to a private chat with the bot and, in doing so, passes a start parameter that instructs the bot to
    /// return an oauth link. Once done, the bot can offer a
    /// <see cref="InlineKeyboardButton.SwitchInlineQuery"/> button so that the user can
    /// easily return to the chat where they wanted to use the bot’s inline capabilities
    /// </para>
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    public static async Task AnswerAsync(
        this InlineQuery inlineQuery,
        IEnumerable<InlineQueryResult> results,
        int? cacheTime = default,
        bool? isPersonal = default,
        string? nextOffset = default,
        string? switchPmText = default,
        string? switchPmParameter = default,
        CancellationToken cancellationToken = default
    ) =>
        await inlineQuery.ThrowIfNull(nameof(inlineQuery))
            .FromCarrier()
            .MakeRequestAsync(
                request: new AnswerInlineQueryRequest(inlineQuery.Id, results)
                {
                    CacheTime = cacheTime,
                    IsPersonal = isPersonal,
                    NextOffset = nextOffset,
                    SwitchPmText = switchPmText,
                    SwitchPmParameter = switchPmParameter
                },
                cancellationToken
            )
            .ConfigureAwait(false);
}
