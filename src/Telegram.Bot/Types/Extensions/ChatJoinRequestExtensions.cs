using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Extensions;
using Telegram.Bot.Requests;

namespace Telegram.Bot.Types;

/// <summary>
/// A set of extension methods for <see cref="ChatJoinRequest"/>.
/// </summary>
public static class ChatJoinRequestExtensions
{
    /// <summary>
    /// Use this method to approve a chat join request. The bot must be an administrator in the chat for this to
    /// work and must have the <see cref="ChatPermissions.CanInviteUsers"/> administrator right.
    /// Returns <c>true</c> on success.
    /// </summary>
    /// <param name="chatJoinRequest"></param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    public static async Task<bool> ApproveAsync(
        this ChatJoinRequest chatJoinRequest,
        CancellationToken cancellationToken = default
    ) =>
        await chatJoinRequest.ThrowIfNull(nameof(chatJoinRequest))
            .FromCarrier()
            .MakeRequestAsync(
                request: new ApproveChatJoinRequest(chatJoinRequest.Chat.Id, chatJoinRequest.From.Id),
                cancellationToken
            )
            .ConfigureAwait(false);

    /// <summary>
    /// Use this method to decline a chat join request. The bot must be an administrator in the chat for this to
    /// work and must have the <see cref="ChatPermissions.CanInviteUsers"/> administrator right.
    /// Returns <c>true</c> on success.
    /// </summary>
    /// <param name="chatJoinRequest"></param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    public static async Task<bool> DeclineAsync(
        this ChatJoinRequest chatJoinRequest,
        CancellationToken cancellationToken = default
    ) =>
        await chatJoinRequest.ThrowIfNull(nameof(chatJoinRequest))
            .FromCarrier()
            .MakeRequestAsync(
                request: new DeclineChatJoinRequest(chatJoinRequest.Chat.Id, chatJoinRequest.From.Id),
                cancellationToken
            )
            .ConfigureAwait(false);
}
