using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Extensions;
using Telegram.Bot.Requests;
using Telegram.Bot.Types.Payments;

namespace Telegram.Bot.Types;

/// <summary>
/// A set of extension methods for <see cref="ShippingQuery"/>.
/// </summary>
public static class ShippingQueryExtensions
{
    /// <summary>
    /// If you sent an invoice requesting a shipping address and the parameter <c>isFlexible"</c> was specified,
    /// the Bot API will send an <see cref="Update"/> with a <see cref="ShippingQuery"/> field
    /// to the bot. Use this method to reply to shipping queries
    /// </summary>
    /// <param name="shippingQuery"></param>
    /// <param name="shippingOptions">
    /// Required if ok is <c>true</c>. An array of available shipping options
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    public static async Task AnswerAsync(
        this ShippingQuery shippingQuery,
        IEnumerable<ShippingOption> shippingOptions,
        CancellationToken cancellationToken = default
    ) =>
        await shippingQuery.ThrowIfNull(nameof(shippingQuery))
            .FromCarrier()
            .MakeRequestAsync(
                request: new AnswerShippingQueryRequest(shippingQuery.Id, shippingOptions),
                cancellationToken
            )
            .ConfigureAwait(false);

    /// <summary>
    /// If you sent an invoice requesting a shipping address and the parameter <c>isFlexible"</c> was specified,
    /// the Bot API will send an <see cref="Update"/> with a <see cref="ShippingQuery"/> field
    /// to the bot. Use this method to indicate failed shipping query
    /// </summary>
    /// <param name="shippingQuery"></param>
    /// <param name="errorMessage">
    /// Required if <see cref="AnswerShippingQueryRequest.Ok"/> is <c>false</c>. Error message in
    /// human readable form that explains why it is impossible to complete the order (e.g. "Sorry, delivery to
    /// your desired address is unavailable'). Telegram will display this message to the user
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    public static async Task AnswerAsync(
        this ShippingQuery shippingQuery,
        string errorMessage,
        CancellationToken cancellationToken = default
    ) =>
        await shippingQuery.ThrowIfNull(nameof(shippingQuery))
            .FromCarrier()
            .MakeRequestAsync(
                request: new AnswerShippingQueryRequest(shippingQuery.Id, errorMessage),
                cancellationToken
            )
            .ConfigureAwait(false);

}
