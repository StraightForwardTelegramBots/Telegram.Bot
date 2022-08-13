using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Extensions;
using Telegram.Bot.Requests;
using Telegram.Bot.Types.Payments;

namespace Telegram.Bot.Types;

/// <summary>
/// A set of extension methods for <see cref="PreCheckoutQuery"/>.
/// </summary>
public static class PreCheckoutQueryExtensions
{

    /// <summary>
    /// Once the user has confirmed their payment and shipping details, the Bot API sends the final confirmation
    /// in the form of an <see cref="Update"/> with the field <see cref="PreCheckoutQuery"/>.
    /// Use this method to respond to such pre-checkout queries.
    /// </summary>
    /// <remarks>
    /// <b>Note</b>: The Bot API must receive an answer within 10 seconds after the pre-checkout query was sent.
    /// </remarks>
    /// <param name="preCheckoutQuery"></param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    public static async Task AnswerAsync(
        this PreCheckoutQuery preCheckoutQuery,
        CancellationToken cancellationToken = default
    ) =>
        await preCheckoutQuery.ThrowIfNull(nameof(preCheckoutQuery))
            .FromCarrier()
            .MakeRequestAsync(request: new AnswerPreCheckoutQueryRequest(preCheckoutQuery.Id), cancellationToken)
            .ConfigureAwait(false);

    /// <summary>
    /// Once the user has confirmed their payment and shipping details, the Bot API sends the final confirmation
    /// in the form of an <see cref="Update"/> with the field <see cref="PreCheckoutQuery"/>.
    /// Use this method to respond to indicate failed pre-checkout query
    /// </summary>
    /// <param name="preCheckoutQuery"></param>
    /// <param name="errorMessage">
    /// Required if <see cref="AnswerPreCheckoutQueryRequest.Ok"/> is <c>false</c>. Error message in
    /// human readable form that explains the reason for failure to proceed with the checkout (e.g. "Sorry,
    /// somebody just bought the last of our amazing black T-shirts while you were busy filling out your payment
    /// details. Please choose a different color or garment!"). Telegram will display this message to the user
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    public static async Task AnswerAsync(
        this PreCheckoutQuery preCheckoutQuery,
        string errorMessage,
        CancellationToken cancellationToken = default
    ) =>
        await preCheckoutQuery.ThrowIfNull(nameof(preCheckoutQuery))
            .FromCarrier()
            .MakeRequestAsync(
                request: new AnswerPreCheckoutQueryRequest(preCheckoutQuery.Id, errorMessage),
                cancellationToken
            )
            .ConfigureAwait(false);


}
