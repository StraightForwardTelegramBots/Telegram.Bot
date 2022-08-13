using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Extensions;
using Telegram.Bot.Requests;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Bot.Types;

/// <summary>
/// A set of extension methods for <see cref="User"/>.
/// </summary>
public static class UserExtensions
{
    #region Helpers

    /// <summary>
    /// Gets a text mention based on specified <paramref name="parseMode"/>.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <param name="parseMode">The parse mode.</param>
    /// <param name="displayName">Optional, display name. defaults to <see cref="User.FullName"/>.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NotSupportedException"></exception>
    public static string GetTextMention(
        this User user, ParseMode parseMode = ParseMode.Html, string? displayName = default)
    {
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        displayName ??= user.FullName;

        return parseMode switch
        {
            ParseMode.Html => $"<a href='tg://user?id={user.Id}'>{displayName}</a>",
            ParseMode.Markdown => $"[{displayName}](tg://user?id={user.Id})",
            ParseMode.MarkdownV2 => $"[{displayName}](tg://user?id={user.Id})",
            _ => throw new NotSupportedException("Parse mode not supported.")
        };
    }

    #endregion

    #region Api Methods

    /// <summary>
    /// Use this method to send a private text message to the user.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="text">Text of the message to be sent, 1-4096 characters after entities parsing</param>
    /// <param name="parseMode">
    /// Mode for parsing entities in the new caption. See
    /// <a href="https://core.telegram.org/bots/api#formatting-options">formatting</a> options for more
    /// details
    /// </param>
    /// <param name="entities">
    /// List of special entities that appear in message text, which can be specified instead
    /// of <see cref="ParseMode"/>
    /// </param>
    /// <param name="disableWebPagePreview">Disables link previews for links in this message</param>
    /// <param name="disableNotification">
    /// Sends the message silently. Users will receive a notification with no sound
    /// </param>
    /// <param name="protectContent">Protects the contents of sent messages from forwarding and saving</param>
    /// <param name="replyToMessageId">If the message is a reply, ID of the original message</param>
    /// <param name="allowSendingWithoutReply">
    /// Pass <c>true</c>, if the message should be sent even if the specified replied-to message is not found
    /// </param>
    /// <param name="replyMarkup">
    /// Additional interface options. An <see cref="InlineKeyboardMarkup">inline keyboard</see>,
    /// <see cref="ReplyKeyboardMarkup">custom reply keyboard</see>, instructions to
    /// <see cref="ReplyKeyboardRemove">remove reply keyboard</see> or to <see cref="ForceReplyMarkup">force a
    /// reply</see> from the user
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    /// <returns>On success, the sent <see cref="Message"/> is returned.</returns>
    public static async Task<Message> PmAsync(
        this User user,
        string text,
        ParseMode? parseMode = default,
        IEnumerable<MessageEntity>? entities = default,
        bool? disableWebPagePreview = default,
        bool? disableNotification = default,
        bool? protectContent = default,
        int? replyToMessageId = default,
        bool? allowSendingWithoutReply = default,
        IReplyMarkup? replyMarkup = default,
        CancellationToken cancellationToken = default
    ) =>
        await user.ThrowIfNull(nameof(user))
            .FromCarrier()
            .MakeRequestAsync(
                request: new SendMessageRequest(user.Id, text)
                {
                    ParseMode = parseMode ?? user.FromCarrier().ClientDefaults.ParseMode,
                    Entities = entities,
                    DisableWebPagePreview = disableWebPagePreview ?? user.FromCarrier().ClientDefaults.DisableWebPagePreview,
                    DisableNotification = disableNotification ?? user.FromCarrier().ClientDefaults.DisableNotification,
                    ProtectContent = protectContent ?? user.FromCarrier().ClientDefaults.ProtectContent,
                    ReplyToMessageId = replyToMessageId,
                    AllowSendingWithoutReply = allowSendingWithoutReply ?? user.FromCarrier().ClientDefaults.AllowSendingWithoutReply,
                    ReplyMarkup = replyMarkup
                },
                cancellationToken
            )
            .ConfigureAwait(false);

    /// <summary>
    /// Use this method to get a list of profile pictures for a user.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="offset">
    /// Sequential number of the first photo to be returned. By default, all photos are returned
    /// </param>
    /// <param name="limit">
    /// Limits the number of photos to be retrieved. Values between 1-100 are accepted. Defaults to 100
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    /// <returns>Returns a <see cref="UserProfilePhotos"/> object</returns>
    public static async Task<UserProfilePhotos> GetProfilePhotosAsync(
        this User user,
        int? offset = default,
        int? limit = default,
        CancellationToken cancellationToken = default
    ) =>
        await user.ThrowIfNull(nameof(user))
            .FromCarrier()
            .MakeRequestAsync(
                request: new GetUserProfilePhotosRequest(user.Id)
                {
                    Offset = offset,
                    Limit = limit
                },
                cancellationToken
            )
            .ConfigureAwait(false);

    /// <summary>
    /// Use this method to approve a chat join request. The bot must be an administrator in the chat for this to
    /// work and must have the <see cref="ChatPermissions.CanInviteUsers"/> administrator right.
    /// Returns <c>true</c> on success.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="chatId">
    /// Unique identifier for the target chat or username of the target channel
    /// (in the format <c>@channelusername</c>)
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    public static async Task<bool> ApproveJoinRequest(
        this User user,
        ChatId chatId,
        CancellationToken cancellationToken = default
    ) =>
        await user.ThrowIfNull(nameof(user))
            .FromCarrier()
            .MakeRequestAsync(
                request: new ApproveChatJoinRequest(chatId, user.Id),
                cancellationToken
            )
            .ConfigureAwait(false);

    /// <summary>
    /// Use this method to decline a chat join request. The bot must be an administrator in the chat for this to
    /// work and must have the <see cref="ChatPermissions.CanInviteUsers"/> administrator right.
    /// Returns <c>true</c> on success.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="chatId">
    /// Unique identifier for the target chat or username of the target channel
    /// (in the format <c>@channelusername</c>)
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    public static async Task<bool> DeclineJoinRequest(
        this User user,
        ChatId chatId,
        CancellationToken cancellationToken = default
    ) =>
        await user.ThrowIfNull(nameof(user))
            .FromCarrier()
            .MakeRequestAsync(
                request: new DeclineChatJoinRequest(chatId, user.Id),
                cancellationToken
            )
            .ConfigureAwait(false);

    #endregion
}
