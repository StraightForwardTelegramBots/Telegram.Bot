using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Extensions;
using Telegram.Bot.Requests;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

namespace Telegram.Bot.Types;

/// <summary>
/// A sen of extension methods for <see cref="Chat"/>.
/// </summary>
public static class ChatExtensions
{
    #region Api Methods

    /// <summary>
    /// Use this method when you need to tell the user that something is happening on the bot’s side. The status is
    /// set for 5 seconds or less (when a message arrives from your bot, Telegram clients clear its typing status).
    /// </summary>
    /// <example>
    /// <para>
    /// The <a href="https://t.me/imagebot">ImageBot</a> needs some time to process a request and upload the
    /// image. Instead of sending a text message along the lines of “Retrieving image, please wait…”, the bot may
    /// use <see cref="SendChatActionAsync"/> with <see cref="Action"/> = <see cref="ChatAction.UploadPhoto"/>.
    /// The user will see a “sending photo” status for the bot.
    /// </para>
    /// <para>
    /// We only recommend using this method when a response from the bot will take a <b>noticeable</b> amount of
    /// time to arrive.
    /// </para>
    /// </example>
    /// <param name="chat"></param>
    /// <param name="chatAction">
    /// Type of action to broadcast. Choose one, depending on what the user is about to receive:
    /// <see cref="ChatAction.Typing"/> for <see cref="TelegramBotClientExtensions.SendTextMessageAsync">text messages</see>,
    /// <see cref="ChatAction.UploadPhoto"/> for <see cref="TelegramBotClientExtensions.SendPhotoAsync">photos</see>,
    /// <see cref="ChatAction.RecordVideo"/> or <see cref="ChatAction.UploadVideo"/> for
    /// <see cref="TelegramBotClientExtensions.SendVideoAsync">videos</see>, <see cref="ChatAction.RecordVoice"/> or
    /// <see cref="ChatAction.UploadVoice"/> for <see cref="TelegramBotClientExtensions.SendVoiceAsync">voice notes</see>,
    /// <see cref="ChatAction.UploadDocument"/> for <see cref="TelegramBotClientExtensions.SendDocumentAsync">general files</see>,
    /// <see cref="ChatAction.FindLocation"/> for <see cref="TelegramBotClientExtensions.SendLocationAsync">location data</see>,
    /// <see cref="ChatAction.RecordVideoNote"/> or <see cref="ChatAction.UploadVideoNote"/> for
    /// <see cref="TelegramBotClientExtensions.SendVideoNoteAsync">video notes</see>
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    public static async Task SendChatActionAsync(
        this Chat chat,
        ChatAction chatAction,
        CancellationToken cancellationToken = default
    ) =>
        await chat.ThrowIfNull(nameof(chat))
            .FromCarrier()
            .MakeRequestAsync(request: new SendChatActionRequest(chat.Id, chatAction), cancellationToken)
            .ConfigureAwait(false);

    /// <summary>
    /// Use this method to ban a user in a group, a supergroup or a channel. In the case of supergroups and
    /// channels, the user will not be able to return to the chat on their own using invite links, etc., unless
    /// <see cref="TelegramBotClientExtensions.UnbanChatMemberAsync(ITelegramBotClient, ChatId, long, bool?, CancellationToken)">unbanned</see>
    /// first. The bot must be an administrator in the chat for this to work and must have the appropriate
    /// admin rights.
    /// </summary>
    /// <param name="chat"></param>
    /// <param name="userId">Unique identifier of the target user</param>
    /// <param name="untilDate">
    /// Date when the user will be unbanned. If user is banned for more than 366 days or less than 30 seconds
    /// from the current time they are considered to be banned forever. Applied for supergroups and channels only
    /// </param>
    /// <param name="revokeMessages">
    /// Pass <c>true</c> to delete all messages from the chat for the user that is being removed.
    /// If <c>false</c>, the user will be able to see messages in the group that were sent before the user was
    /// removed. Always <c>true</c> for supergroups and channels
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    public static async Task BanMemberAsync(
        this Chat chat,
        long userId,
        DateTime? untilDate = default,
        bool? revokeMessages = default,
        CancellationToken cancellationToken = default
    ) =>
        await chat.ThrowIfNull(nameof(chat))
            .FromCarrier()
            .MakeRequestAsync(
                request: new BanChatMemberRequest(chat.Id, userId)
                {
                    UntilDate = untilDate,
                    RevokeMessages = revokeMessages
                },
                cancellationToken
            )
            .ConfigureAwait(false);

    /// <summary>
    /// Use this method to unban a previously banned user in a supergroup or channel. The user will <b>not</b>
    /// return to the group or channel automatically, but will be able to join via link, etc. The bot must be an
    /// administrator for this to work. By default, this method guarantees that after the call the user is not a
    /// member of the chat, but will be able to join it. So if the user is a member of the chat they will also be
    /// <b>removed</b> from the chat. If you don't want this, use the parameter <paramref name="onlyIfBanned"/>
    /// </summary>
    /// <param name="chat"></param>
    /// <param name="userId">Unique identifier of the target user</param>
    /// <param name="onlyIfBanned">Do nothing if the user is not banned</param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    public static async Task UnbanMemberAsync(
        this Chat chat,
        long userId,
        bool? onlyIfBanned = default,
        CancellationToken cancellationToken = default
    ) =>
        await chat.ThrowIfNull(nameof(chat))
            .FromCarrier()
            .MakeRequestAsync(
                request: new UnbanChatMemberRequest(chat.Id, userId)
                {
                    OnlyIfBanned = onlyIfBanned
                },
                cancellationToken
            )
            .ConfigureAwait(false);

    /// <summary>
    /// Use this method to restrict a user in a supergroup. The bot must be an administrator in the supergroup
    /// for this to work and must have the appropriate admin rights. Pass <c>true</c> for all permissions to
    /// lift restrictions from a user.
    /// </summary>
    /// <param name="chat"></param>
    /// <param name="userId">Unique identifier of the target user</param>
    /// <param name="permissions">New user permissions</param>
    /// <param name="untilDate">Date when restrictions will be lifted for the user, unix time. If user is restricted for more than 366 days or less than 30 seconds from the current time, they are considered to be restricted forever.</param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    public static async Task RestrictMemberAsync(
        this Chat chat,
        long userId,
        ChatPermissions permissions,
        DateTime? untilDate = default,
        CancellationToken cancellationToken = default
    ) =>
        await chat.ThrowIfNull(nameof(chat))
            .FromCarrier()
            .MakeRequestAsync(
                request: new RestrictChatMemberRequest(chat.Id, userId, permissions)
                {
                    UntilDate = untilDate
                },
                cancellationToken
            )
            .ConfigureAwait(false);

    /// <summary>
    /// Use this method to promote or demote a user in a supergroup or a channel. The bot must be an administrator in the chat for this to work and must have the appropriate admin rights. Pass <c><c>false</c></c> for all boolean parameters to demote a user.
    /// </summary>
    /// <param name="chat"></param>
    /// <param name="userId">Unique identifier of the target user</param>
    /// <param name="isAnonymous">Pass <c>true</c>, if the administrator's presence in the chat is hidden</param>
    /// <param name="canManageChat">Pass <c>true</c>, if the administrator can access the chat event log, chat statistics, message statistics in channels, see channel members, see anonymous administrators in supergroups and ignore slow mode. Implied by any other administrator privilege</param>
    /// <param name="canPostMessages">Pass <c>true</c>, if the administrator can create channel posts, channels only</param>
    /// <param name="canEditMessages">Pass <c>true</c>, if the administrator can edit messages of other users, channels only</param>
    /// <param name="canDeleteMessages">Pass <c>true</c>, if the administrator can delete messages of other users</param>
    /// <param name="canManageVideoChats">Pass <c>true</c>, if the administrator can manage voice chats, supergroups only</param>
    /// <param name="canRestrictMembers">Pass <c>true</c>, if the administrator can restrict, ban or unban chat members</param>
    /// <param name="canPromoteMembers">Pass <c>true</c>, if the administrator can add new administrators with a subset of his own privileges or demote administrators that he has promoted, directly or indirectly (promoted by administrators that were appointed by him)</param>
    /// <param name="canChangeInfo">Pass <c>true</c>, if the administrator can change chat title, photo and other settings</param>
    /// <param name="canInviteUsers">Pass <c>true</c>, if the administrator can invite new users to the chat</param>
    /// <param name="canPinMessages">Pass <c>true</c>, if the administrator can pin messages, supergroups only</param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    public static async Task PromoteMemberAsync(
        this Chat chat,
        long userId,
        bool? isAnonymous = default,
        bool? canManageChat = default,
        bool? canPostMessages = default,
        bool? canEditMessages = default,
        bool? canDeleteMessages = default,
        bool? canManageVideoChats = default,
        bool? canRestrictMembers = default,
        bool? canPromoteMembers = default,
        bool? canChangeInfo = default,
        bool? canInviteUsers = default,
        bool? canPinMessages = default,
        CancellationToken cancellationToken = default
    ) =>
        await chat.ThrowIfNull(nameof(chat))
            .FromCarrier()
            .MakeRequestAsync(
                request: new PromoteChatMemberRequest(chat.Id, userId)
                {
                    IsAnonymous = isAnonymous,
                    CanManageChat = canManageChat,
                    CanPostMessages = canPostMessages,
                    CanEditMessages = canEditMessages,
                    CanDeleteMessages = canDeleteMessages,
                    CanManageVideoChat = canManageVideoChats,
                    CanRestrictMembers = canRestrictMembers,
                    CanPromoteMembers = canPromoteMembers,
                    CanChangeInfo = canChangeInfo,
                    CanInviteUsers = canInviteUsers,
                    CanPinMessages = canPinMessages,
                },
                cancellationToken
            )
            .ConfigureAwait(false);

    /// <summary>
    /// Use this method to set a custom title for an administrator in a supergroup promoted by the bot.
    /// </summary>
    /// <param name="chat"></param>
    /// <param name="userId">Unique identifier of the target user</param>
    /// <param name="customTitle">
    /// New custom title for the administrator; 0-16 characters, emoji are not allowed
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    public static async Task SetAdministratorCustomTitleAsync(
        this Chat chat,
        long userId,
        string customTitle,
        CancellationToken cancellationToken = default
    ) =>
        await chat.ThrowIfNull(nameof(chat))
            .FromCarrier()
            .MakeRequestAsync(
                request: new SetChatAdministratorCustomTitleRequest(chat.Id, userId, customTitle),
                cancellationToken
            )
            .ConfigureAwait(false);

    /// <summary>
    /// Use this method to ban a channel chat in a supergroup or a channel. The owner of the chat will not be
    /// able to send messages and join live streams on behalf of the chat, unless it is unbanned first. The bot
    /// must be an administrator in the supergroup or channel for this to work and must have the appropriate
    /// administrator rights. Returns <c>true</c> on success.
    /// </summary>
    /// <param name="chat"></param>
    /// <param name="senderChatId">Unique identifier of the target sender chat</param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    public static async Task BanSenderChatAsync(
        this Chat chat,
        long senderChatId,
        CancellationToken cancellationToken = default
    ) =>
        await chat.ThrowIfNull(nameof(chat))
            .FromCarrier()
            .MakeRequestAsync(
                new BanChatSenderChatRequest(chat.Id, senderChatId),
                cancellationToken
            )
            .ConfigureAwait(false);

    /// <summary>
    /// Use this method to unban a previously banned channel chat in a supergroup or channel. The bot must be
    /// an administrator for this to work and must have the appropriate administrator rights.
    /// Returns <c>true</c> on success.
    /// </summary>
    /// <param name="chat"></param>
    /// <param name="senderChatId">Unique identifier of the target sender chat</param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    public static async Task UnbanSenderChatAsync(
        this Chat chat,
        long senderChatId,
        CancellationToken cancellationToken = default
    ) =>
        await chat.ThrowIfNull(nameof(chat))
            .FromCarrier()
            .MakeRequestAsync(
                new UnbanChatSenderChatRequest(chat.Id, senderChatId),
                cancellationToken
            )
            .ConfigureAwait(false);

    /// <summary>
    /// Use this method to set default chat permissions for all members. The bot must be an administrator
    /// in the group or a supergroup for this to work and must have the can_restrict_members admin rights
    /// </summary>
    /// <param name="chat"></param>
    /// <param name="permissions">New default chat permissions</param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    public static async Task SetPermissionsAsync(
        this Chat chat,
        ChatPermissions permissions,
        CancellationToken cancellationToken = default
    ) =>
        await chat.ThrowIfNull(nameof(chat))
            .FromCarrier()
            .MakeRequestAsync(
                request: new SetChatPermissionsRequest(chat.Id, permissions),
                cancellationToken
            )
            .ConfigureAwait(false);

    /// <summary>
    /// Use this method to generate a new primary invite link for a chat; any previously generated primary
    /// link is revoked. The bot must be an administrator in the chat for this to work and must have the
    /// appropriate admin rights
    /// </summary>
    /// <param name="chat"></param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    public static async Task<string> ExportInviteLinkAsync(
        this Chat chat,
        CancellationToken cancellationToken = default
    ) =>
        await chat.ThrowIfNull(nameof(chat))
            .FromCarrier()
            .MakeRequestAsync(
                request: new ExportChatInviteLinkRequest(chat.Id),
                cancellationToken
            )
            .ConfigureAwait(false);

    /// <summary>
    /// Use this method to create an additional invite link for a chat. The bot must be an administrator
    /// in the chat for this to work and must have the appropriate admin rights. The link can be revoked
    /// using the method <see cref="TelegramBotClientExtensions.RevokeChatInviteLinkAsync"/>
    /// </summary>
    /// <param name="chat"></param>
    /// <param name="name">Invite link name; 0-32 characters</param>
    /// <param name="expireDate">Point in time when the link will expire</param>
    /// <param name="memberLimit">
    /// Maximum number of users that can be members of the chat simultaneously after joining the chat
    /// via this invite link; 1-99999
    /// </param>
    /// <param name="createsJoinRequest">
    /// Set to <c>true</c>, if users joining the chat via the link need to be approved by chat administrators.
    /// If <c>true</c>, <paramref name="memberLimit"/> can't be specified
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    /// <returns>Returns the new invite link as <see cref="ChatInviteLink"/> object.</returns>
    public static async Task<ChatInviteLink> CreateInviteLinkAsync(
        this Chat chat,
        string? name = default,
        DateTime? expireDate = default,
        int? memberLimit = default,
        bool? createsJoinRequest = default,
        CancellationToken cancellationToken = default
    ) =>
        await chat.ThrowIfNull(nameof(chat))
            .FromCarrier()
            .MakeRequestAsync(
                request: new CreateChatInviteLinkRequest(chat.Id)
                {
                    Name = name,
                    ExpireDate = expireDate,
                    MemberLimit = memberLimit,
                    CreatesJoinRequest = createsJoinRequest,
                },
                cancellationToken
            )
            .ConfigureAwait(false);

    /// <summary>
    /// Use this method to edit a non-primary invite link created by the bot. The bot must be an
    /// administrator in the chat for this to work and must have the appropriate admin rights
    /// </summary>
    /// <param name="chat"></param>
    /// <param name="inviteLink">The invite link to edit</param>
    /// <param name="name">Invite link name; 0-32 characters</param>
    /// <param name="expireDate">Point in time when the link will expire</param>
    /// <param name="memberLimit">
    /// Maximum number of users that can be members of the chat simultaneously after joining the chat
    /// via this invite link; 1-99999
    /// </param>
    /// <param name="createsJoinRequest">
    /// Set to <c>true</c>, if users joining the chat via the link need to be approved by chat administrators.
    /// If <c>true</c>, <paramref name="memberLimit"/> can't be specified
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    /// <returns>Returns the edited invite link as a <see cref="ChatInviteLink"/> object.</returns>
    public static async Task<ChatInviteLink> EditInviteLinkAsync(
        this Chat chat,
        string inviteLink,
        string? name = default,
        DateTime? expireDate = default,
        int? memberLimit = default,
        bool? createsJoinRequest = default,
        CancellationToken cancellationToken = default
    ) =>
        await chat.ThrowIfNull(nameof(chat))
            .FromCarrier()
            .MakeRequestAsync(
                request: new EditChatInviteLinkRequest(chat.Id, inviteLink)
                {
                    Name = name,
                    ExpireDate = expireDate,
                    MemberLimit = memberLimit,
                    CreatesJoinRequest = createsJoinRequest,
                },
                cancellationToken
            )
            .ConfigureAwait(false);

    /// <summary>
    /// Use this method to revoke an invite link created by the bot. If the primary link is revoked, a new
    /// link is automatically generated. The bot must be an administrator in the chat for this to work and
    /// must have the appropriate admin rights
    /// </summary>
    /// <param name="chat"></param>
    /// <param name="inviteLink">The invite link to revoke</param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    /// <returns>Returns the revoked invite link as <see cref="ChatInviteLink"/> object.</returns>
    public static async Task<ChatInviteLink> RevokeInviteLinkAsync(
        this Chat chat,
        string inviteLink,
        CancellationToken cancellationToken = default
    ) =>
        await chat.ThrowIfNull(nameof(chat))
            .FromCarrier()
            .MakeRequestAsync(
                request: new RevokeChatInviteLinkRequest(chat.Id, inviteLink),
                cancellationToken
            )
            .ConfigureAwait(false);

    /// <summary>
    /// Use this method to approve a chat join request. The bot must be an administrator in the chat for this to
    /// work and must have the <see cref="ChatPermissions.CanInviteUsers"/> administrator right.
    /// Returns <c>true</c> on success.
    /// </summary>
    /// <param name="chat"></param>
    /// <param name="userId">Unique identifier of the target user</param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    public static async Task<bool> ApproveJoinRequest(
        this Chat chat,
        long userId,
        CancellationToken cancellationToken = default
    ) =>
        await chat.ThrowIfNull(nameof(chat))
            .FromCarrier()
            .MakeRequestAsync(
                request: new ApproveChatJoinRequest(chat.Id, userId),
                cancellationToken
            )
            .ConfigureAwait(false);

    /// <summary>
    /// Use this method to decline a chat join request. The bot must be an administrator in the chat for this to
    /// work and must have the <see cref="ChatPermissions.CanInviteUsers"/> administrator right.
    /// Returns <c>true</c> on success.
    /// </summary>
    /// <param name="chat"></param>
    /// <param name="userId">Unique identifier of the target user</param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    public static async Task<bool> DeclineJoinRequest(
        this Chat chat,
        long userId,
        CancellationToken cancellationToken = default
    ) =>
        await chat.ThrowIfNull(nameof(chat))
            .FromCarrier()
            .MakeRequestAsync(
                request: new DeclineChatJoinRequest(chat.Id, userId),
                cancellationToken
            )
            .ConfigureAwait(false);

    /// <summary>
    /// Use this method to set a new profile photo for the chat. Photos can't be changed for private chats.
    /// The bot must be an administrator in the chat for this to work and must have the appropriate admin rights
    /// </summary>
    /// <param name="chat"></param>
    /// <param name="photo">New chat photo, uploaded using multipart/form-data</param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    public static async Task SetPhotoAsync(
        this Chat chat,
        InputFileStream photo,
        CancellationToken cancellationToken = default
    ) =>
        await chat.ThrowIfNull(nameof(chat))
            .FromCarrier()
            .MakeRequestAsync(request: new SetChatPhotoRequest(chat.Id, photo), cancellationToken)
            .ConfigureAwait(false);

    /// <summary>
    /// Use this method to delete a chat photo. Photos can't be changed for private chats. The bot must be an
    /// administrator in the chat for this to work and must have the appropriate admin rights
    /// </summary>
    /// <param name="chat"></param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    public static async Task DeletePhotoAsync(
        this Chat chat,
        CancellationToken cancellationToken = default
    ) =>
        await chat.ThrowIfNull(nameof(chat))
            .FromCarrier()
            .MakeRequestAsync(
                request: new DeleteChatPhotoRequest(chat.Id),
                cancellationToken
            )
            .ConfigureAwait(false);

    /// <summary>
    /// Use this method to change the title of a chat. Titles can't be changed for private chats. The bot
    /// must be an administrator in the chat for this to work and must have the appropriate admin rights
    /// </summary>
    /// <param name="chat"></param>
    /// <param name="title">New chat title, 1-255 characters</param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    public static async Task SetTitleAsync(
        this Chat chat,
        string title,
        CancellationToken cancellationToken = default
    ) =>
        await chat.ThrowIfNull(nameof(chat))
            .FromCarrier()
            .MakeRequestAsync(request: new SetChatTitleRequest(chat.Id, title), cancellationToken)
            .ConfigureAwait(false);

    /// <summary>
    /// Use this method to change the description of a group, a supergroup or a channel. The bot must
    /// be an administrator in the chat for this to work and must have the appropriate admin rights
    /// </summary>
    /// <param name="chat"></param>
    /// <param name="description">New chat Description, 0-255 characters</param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    public static async Task SetDescriptionAsync(
        this Chat chat,
        string? description = default,
        CancellationToken cancellationToken = default
    ) =>
        await chat.ThrowIfNull(nameof(chat))
            .FromCarrier()
            .MakeRequestAsync(
                request: new SetChatDescriptionRequest(chat.Id) { Description = description },
                cancellationToken
            )
            .ConfigureAwait(false);

    /// <summary>
    /// Use this method to add a message to the list of pinned messages in a chat. If the chat is not a private
    /// chat, the bot must be an administrator in the chat for this to work and must have the
    /// '<see cref="ChatPermissions.CanPinMessages"/>' admin right in a supergroup or
    /// '<see cref="ChatMemberAdministrator.CanEditMessages"/>' admin right in a channel
    /// </summary>
    /// <param name="chat"></param>
    /// <param name="messageId">Identifier of a message to pin</param>
    /// <param name="disableNotification">
    /// Pass <c><c>true</c></c>, if it is not necessary to send a notification to all chat members about
    /// the new pinned message. Notifications are always disabled in channels and private chats
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    public static async Task PinMessageAsync(
        this Chat chat,
        int messageId,
        bool? disableNotification = default,
        CancellationToken cancellationToken = default
    ) =>
        await chat.ThrowIfNull(nameof(chat))
            .FromCarrier()
            .MakeRequestAsync(
                request: new PinChatMessageRequest(chat.Id, messageId)
                {
                    DisableNotification = disableNotification
                },
                cancellationToken
            )
            .ConfigureAwait(false);

    /// <summary>
    /// Use this method to remove a message from the list of pinned messages in a chat. If the chat is not
    /// a private chat, the bot must be an administrator in the chat for this to work and must have the
    /// '<see cref="ChatMemberAdministrator.CanPinMessages"/>' admin right in a supergroup or
    /// '<see cref="ChatMemberAdministrator.CanEditMessages"/>' admin right in a channel
    /// </summary>
    /// <param name="chat"></param>
    /// <param name="messageId">
    /// Identifier of a message to unpin. If not specified, the most recent pinned message (by sending date)
    /// will be unpinned
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    public static async Task UnpinMessageAsync(
        this Chat chat,
        int? messageId = default,
        CancellationToken cancellationToken = default
    ) =>
        await chat.ThrowIfNull(nameof(chat))
            .FromCarrier()
            .MakeRequestAsync(
                request: new UnpinChatMessageRequest(chat.Id) { MessageId = messageId },
                cancellationToken
            )
            .ConfigureAwait(false);

    /// <summary>
    /// Use this method to clear the list of pinned messages in a chat. If the chat is not a private chat,
    /// the bot must be an administrator in the chat for this to work and must have the
    /// '<see cref="ChatMemberAdministrator.CanPinMessages"/>' admin right in a supergroup or
    /// '<see cref="ChatMemberAdministrator.CanEditMessages"/>' admin right in a channel
    /// </summary>
    /// <param name="chat"></param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    public static async Task UnpinAllMessages(
        this Chat chat,
        CancellationToken cancellationToken = default
    ) =>
        await chat.ThrowIfNull(nameof(chat))
            .FromCarrier()
            .MakeRequestAsync(request: new UnpinAllChatMessagesRequest(chat.Id), cancellationToken)
            .ConfigureAwait(false);

    /// <summary>
    /// Use this method for your bot to leave a group, supergroup or channel.
    /// </summary>
    /// <param name="chat"></param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    public static async Task LeaveAsync(
        this Chat chat,
        CancellationToken cancellationToken = default
    ) =>
        await chat.ThrowIfNull(nameof(chat))
            .FromCarrier()
            .MakeRequestAsync(request: new LeaveChatRequest(chat.Id), cancellationToken)
            .ConfigureAwait(false);

    /// <summary>
    /// Use this method to get up to date information about the chat (current name of the user for one-on-one
    /// conversations, current username of a user, group or channel, etc.)
    /// </summary>
    /// <param name="chat"></param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    /// <returns>Returns a <see cref="Chat"/> object on success.</returns>
    public static async Task<Chat> GetAsync(
        this Chat chat,
        CancellationToken cancellationToken = default
    ) =>
        await chat.ThrowIfNull(nameof(chat))
            .FromCarrier()
            .MakeRequestAsync(request: new GetChatRequest(chat.Id), cancellationToken)
            .ConfigureAwait(false);

    /// <summary>
    /// Use this method to get a list of administrators in a chat.
    /// </summary>
    /// <param name="chat"></param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    /// <returns>
    /// On success, returns an Array of <see cref="ChatMember"/> objects that contains information about all chat
    /// administrators except other bots. If the chat is a group or a supergroup and no administrators were
    /// appointed, only the creator will be returned
    /// </returns>
    public static async Task<ChatMember[]> GetAdministratorsAsync(
        this Chat chat,
        CancellationToken cancellationToken = default
    ) =>
        await chat.ThrowIfNull(nameof(chat))
            .FromCarrier()
            .MakeRequestAsync(request: new GetChatAdministratorsRequest(chat.Id), cancellationToken)
            .ConfigureAwait(false);

    /// <summary>
    /// Use this method to get the number of members in a chat.
    /// </summary>
    /// <param name="chat"></param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    /// <returns>Returns <see cref="int"/> on success..</returns>
    public static async Task<int> GetMemberCountAsync(
        this Chat chat,
        CancellationToken cancellationToken = default
    ) =>
        await chat.ThrowIfNull(nameof(chat))
            .FromCarrier()
            .MakeRequestAsync(request: new GetChatMemberCountRequest(chat.Id), cancellationToken)
            .ConfigureAwait(false);

    /// <summary>
    /// Use this method to get information about a member of a chat.
    /// </summary>
    /// <param name="chat"></param>
    /// <param name="userId">Unique identifier of the target user</param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    /// <returns>Returns a <see cref="ChatMember"/> object on success.</returns>
    public static async Task<ChatMember> GetMemberAsync(
        this Chat chat,
        long userId,
        CancellationToken cancellationToken = default
    ) =>
        await chat.ThrowIfNull(nameof(chat))
            .FromCarrier()
            .MakeRequestAsync(request: new GetChatMemberRequest(chat.Id, userId), cancellationToken)
            .ConfigureAwait(false);

    /// <summary>
    /// Use this method to set a new group sticker set for a supergroup. The bot must be an administrator in the
    /// chat for this to work and must have the appropriate admin rights. Use the field
    /// <see cref="Chat.CanSetStickerSet"/> optionally returned in <see cref="TelegramBotClientExtensions.GetChatAsync"/> requests to check
    /// if the bot can use this method.
    /// </summary>
    /// <param name="chat"></param>
    /// <param name="stickerSetName">Name of the sticker set to be set as the group sticker set</param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    public static async Task SetStickerSetAsync(
        this Chat chat,
        string stickerSetName,
        CancellationToken cancellationToken = default
    ) =>
        await chat.ThrowIfNull(nameof(chat))
            .FromCarrier()
            .MakeRequestAsync(request: new SetChatStickerSetRequest(chat.Id, stickerSetName), cancellationToken)
            .ConfigureAwait(false);

    /// <summary>
    /// Use this method to delete a group sticker set from a supergroup. The bot must be an administrator in the
    /// chat for this to work and must have the appropriate admin rights. Use the field
    /// <see cref="Chat.CanSetStickerSet"/> optionally returned in <see cref="TelegramBotClientExtensions.GetChatAsync"/> requests to
    /// check if the bot can use this method
    /// </summary>
    /// <param name="chat"></param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    public static async Task DeleteStickerSetAsync(
        this Chat chat,
        CancellationToken cancellationToken = default
    ) =>
        await chat.ThrowIfNull(nameof(chat))
            .FromCarrier()
            .MakeRequestAsync(request: new DeleteChatStickerSetRequest(chat.Id), cancellationToken)
            .ConfigureAwait(false);

    #endregion
}
