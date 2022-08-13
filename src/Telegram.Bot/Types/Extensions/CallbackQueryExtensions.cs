using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Extensions;
using Telegram.Bot.Requests;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Bot.Types;

/// <summary>
/// A set of extension methods for <see cref="CallbackQuery"/>.
/// </summary>
public static class CallbackQueryExtensions
{
    #region Api Methods

    /// <summary>
    /// Use this method to send answers to callback queries sent from
    /// <see cref="InlineKeyboardMarkup">inline keyboards</see>. The answer will be displayed
    /// to the user as a notification at the top of the chat screen or as an alert
    /// </summary>
    /// <remarks>
    /// Alternatively, the user can be redirected to the specified Game URL.For this option to work, you must
    /// first create a game for your bot via <c>@Botfather</c> and accept the terms. Otherwise, you may use
    /// links like <c>t.me/your_bot?start=XXXX</c> that open your bot with a parameter
    /// </remarks>
    /// <param name="query"></param>
    /// <param name="text">
    /// Text of the notification. If not specified, nothing will be shown to the user, 0-200 characters
    /// </param>
    /// <param name="showAlert">
    /// If <c>true</c>, an alert will be shown by the client instead of a notification at the top of the chat
    /// screen. Defaults to <c>false</c>
    /// </param>
    /// <param name="url">
    /// URL that will be opened by the user's client. If you have created a
    /// <a href="https://core.telegram.org/bots/api#game">Game</a> and accepted the conditions via
    /// <c>@Botfather</c>, specify the URL that opens your game — note that this will only work if the query
    /// comes from a callback_game button
    /// <para>
    /// Otherwise, you may use links like <c>t.me/your_bot?start=XXXX</c> that open your bot with a parameter
    /// </para>
    /// </param>
    /// <param name="cacheTime">
    /// The maximum amount of time in seconds that the result of the callback query may be cached client-side.
    /// Telegram apps will support caching starting in version 3.14
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    public static async Task AnswerAsync(
        this CallbackQuery query,
        string? text = default,
        bool? showAlert = default,
        string? url = default,
        int? cacheTime = default,
        CancellationToken cancellationToken = default
    ) =>
        await query.ThrowIfNull(nameof(query))
            .FromCarrier()
            .MakeRequestAsync(
                request: new AnswerCallbackQueryRequest(query.Id)
                {
                    Text = text,
                    ShowAlert = showAlert,
                    Url = url,
                    CacheTime = cacheTime
                },
                cancellationToken
            )
            .ConfigureAwait(false);

    #region Updating message

    /// <summary>
    /// Use this method to edit text and game messages.
    /// </summary>
    /// <param name="callbackQuery"></param>
    /// <param name="text">New text of the message, 1-4096 characters after entities parsing</param>
    /// <param name="parseMode">
    /// Mode for parsing entities in the new caption. See
    /// <a href="https://core.telegram.org/bots/api#formatting-options">formatting</a> options for
    /// more details
    /// </param>
    /// <param name="entities">
    /// List of special entities that appear in message text, which can be specified instead
    /// of <see cref="ParseMode"/>
    /// </param>
    /// <param name="disableWebPagePreview">Disables link previews for links in this message</param>
    /// <param name="replyMarkup">
    /// Additional interface options. An <see cref="InlineKeyboardMarkup">inline keyboard</see>,
    /// <see cref="ReplyKeyboardMarkup">custom reply keyboard</see>, instructions to
    /// <see cref="ReplyKeyboardRemove">remove reply keyboard</see> or to
    /// <see cref="ForceReplyMarkup">force a reply</see> from the user
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    /// <returns>On success the edited <see cref="Message"/> is returned.</returns>
    public static async Task<Message?> EditTextAsync(
        this CallbackQuery callbackQuery,
        string text,
        ParseMode? parseMode = default,
        IEnumerable<MessageEntity>? entities = default,
        bool? disableWebPagePreview = default,
        InlineKeyboardMarkup? replyMarkup = default,
        CancellationToken cancellationToken = default)
    {
        return callbackQuery switch
        {
            { Message: { } msg } => await callbackQuery.ThrowIfNull(nameof(callbackQuery))
                .FromCarrier()
                .MakeRequestAsync(
                    request: new EditMessageTextRequest(msg.Chat.Id, msg.MessageId, text)
                    {
                        ParseMode = parseMode ?? callbackQuery.FromCarrier().ClientDefaults.ParseMode,
                        Entities = entities,
                        DisableWebPagePreview = disableWebPagePreview ?? callbackQuery.FromCarrier().ClientDefaults.DisableWebPagePreview,
                        ReplyMarkup = replyMarkup
                    },
                    cancellationToken
                )
                .ConfigureAwait(false),
            { InlineMessageId: { } inlineMessageId } => (await callbackQuery.ThrowIfNull(nameof(callbackQuery))
                .FromCarrier()
                .MakeRequestAsync(
                    request: new EditInlineMessageTextRequest(inlineMessageId, text)
                    {
                        ParseMode = parseMode ?? callbackQuery.FromCarrier().ClientDefaults.ParseMode,
                        Entities = entities,
                        DisableWebPagePreview = disableWebPagePreview ?? callbackQuery.FromCarrier().ClientDefaults.DisableWebPagePreview,
                        ReplyMarkup = replyMarkup
                    },
                    cancellationToken
                )
                .ConfigureAwait(false))
                .FakeIfBool<Message>(),
            _ => throw new InvalidOperationException("At least one of Message or InlineMessageId should be available.")
        };
    }

    /// <summary>
    /// Use this method to edit captions of messages.
    /// </summary>
    /// <param name="callbackQuery"></param>
    /// <param name="caption">New caption of the message, 0-1024 characters after entities parsing</param>
    /// <param name="parseMode">
    /// Mode for parsing entities in the new caption. See
    /// <a href="https://core.telegram.org/bots/api#formatting-options">formatting</a> options for
    /// more details
    /// </param>
    /// <param name="captionEntities">
    /// List of special entities that appear in the caption, which can be specified instead
    /// of <see cref="ParseMode"/>
    /// </param>
    /// <param name="replyMarkup">
    /// Additional interface options. An <see cref="InlineKeyboardMarkup">inline keyboard</see>,
    /// <see cref="ReplyKeyboardMarkup">custom reply keyboard</see>, instructions to
    /// <see cref="ReplyKeyboardRemove">remove reply keyboard</see> or to
    /// <see cref="ForceReplyMarkup">force a reply</see> from the user
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    /// <returns>On success the edited <see cref="Message"/> is returned.</returns>
    public static async Task<Message?> EditCaptionAsync(
        this CallbackQuery callbackQuery,
        string? caption,
        ParseMode? parseMode = default,
        IEnumerable<MessageEntity>? captionEntities = default,
        InlineKeyboardMarkup? replyMarkup = default,
        CancellationToken cancellationToken = default)
    {
        return callbackQuery switch
        {
            { Message: { } msg } => await callbackQuery.ThrowIfNull(nameof(callbackQuery))
                .FromCarrier()
                .MakeRequestAsync(
                    request: new EditMessageCaptionRequest(msg.Chat.Id, msg.MessageId)
                    {
                        Caption = caption,
                        ParseMode = parseMode ?? callbackQuery.FromCarrier().ClientDefaults.ParseMode,
                        CaptionEntities = captionEntities,
                        ReplyMarkup = replyMarkup
                    },
                    cancellationToken
                )
                .ConfigureAwait(false),
            { InlineMessageId: { } inlineMessageId } => (await callbackQuery.ThrowIfNull(nameof(callbackQuery))
                .FromCarrier()
                .MakeRequestAsync(
                    request: new EditInlineMessageCaptionRequest(inlineMessageId)
                    {
                        Caption = caption,
                        ParseMode = parseMode ?? callbackQuery.FromCarrier().ClientDefaults.ParseMode,
                        CaptionEntities = captionEntities,
                        ReplyMarkup = replyMarkup
                    },
                    cancellationToken
                )
                .ConfigureAwait(false))
                .FakeIfBool<Message>(),
            _ => throw new InvalidOperationException("At least one of Message or InlineMessageId should be available.")
        };
    }

    /// <summary>
    /// Use this method to edit live location messages. A location can be edited until its
    /// <see cref="Location.LivePeriod"/> expires or editing is explicitly disabled by a call to
    /// <see cref="TelegramBotClientExtensions.StopMessageLiveLocationAsync(ITelegramBotClient, ChatId, int, InlineKeyboardMarkup?, CancellationToken)"/>.
    /// </summary>
    /// <param name="callbackQuery"></param>
    /// <param name="latitude">Latitude of new location</param>
    /// <param name="longitude">Longitude of new location</param>
    /// <param name="horizontalAccuracy">
    /// The radius of uncertainty for the location, measured in meters; 0-1500
    /// </param>
    /// <param name="heading">
    /// Direction in which the user is moving, in degrees. Must be between 1 and 360 if specified
    /// </param>
    /// <param name="proximityAlertRadius">
    /// Maximum distance for proximity alerts about approaching another chat member, in meters.
    /// Must be between 1 and 100000 if specified
    /// </param>
    /// <param name="replyMarkup">
    /// Additional interface options. An <see cref="InlineKeyboardMarkup">inline keyboard</see>,
    /// <see cref="ReplyKeyboardMarkup">custom reply keyboard</see>, instructions to
    /// <see cref="ReplyKeyboardRemove">remove reply keyboard</see> or to
    /// <see cref="ForceReplyMarkup">force a reply</see> from the user
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    /// <returns>On success the edited <see cref="Message"/> is returned.</returns>
    public static async Task<Message?> EditLiveLocationAsync(
        this CallbackQuery callbackQuery,
        double latitude,
        double longitude,
        float? horizontalAccuracy = default,
        int? heading = default,
        int? proximityAlertRadius = default,
        InlineKeyboardMarkup? replyMarkup = default,
        CancellationToken cancellationToken = default
    ) => callbackQuery switch
    {
        { Message: { } msg } => await callbackQuery.ThrowIfNull(nameof(callbackQuery))
            .FromCarrier()
            .MakeRequestAsync(
                request: new EditMessageLiveLocationRequest(msg.Chat.Id, msg.MessageId, latitude, longitude)
                {
                    HorizontalAccuracy = horizontalAccuracy,
                    Heading = heading,
                    ProximityAlertRadius = proximityAlertRadius,
                    ReplyMarkup = replyMarkup
                },
                cancellationToken
            )
            .ConfigureAwait(false),
        { InlineMessageId: { } inlineMessageId } => (await callbackQuery.ThrowIfNull(nameof(callbackQuery))
            .FromCarrier()
            .MakeRequestAsync(
                request: new EditInlineMessageLiveLocationRequest(inlineMessageId, latitude, longitude)
                {
                    HorizontalAccuracy = horizontalAccuracy,
                    Heading = heading,
                    ProximityAlertRadius = proximityAlertRadius,
                    ReplyMarkup = replyMarkup
                },
                cancellationToken
            )
            .ConfigureAwait(false))
            .FakeIfBool<Message>(),
        _ => throw new InvalidOperationException("At least one of Message or InlineMessageId should be available.")
    };

    /// <summary>
    /// Use this method to edit animation, audio, document, photo, or video messages. If a message is part of
    /// a message album, then it can be edited only to an audio for audio albums, only to a document for document
    /// albums and to a photo or a video otherwise. Use a previously uploaded file via its
    /// <see cref="InputTelegramFile.FileId"/> or specify a URL
    /// </summary>
    /// <param name="callbackQuery"></param>
    /// <param name="media">A new media content of the message</param>
    /// <param name="replyMarkup">
    /// Additional interface options. An <see cref="InlineKeyboardMarkup">inline keyboard</see>,
    /// <see cref="ReplyKeyboardMarkup">custom reply keyboard</see>, instructions to
    /// <see cref="ReplyKeyboardRemove">remove reply keyboard</see> or to
    /// <see cref="ForceReplyMarkup">force a reply</see> from the user
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    /// <returns>On success the edited <see cref="Message"/> is returned.</returns>
    public static async Task<Message?> EditMediaAsync(
        this CallbackQuery callbackQuery,
        InputMediaBase media,
        InlineKeyboardMarkup? replyMarkup = default,
        CancellationToken cancellationToken = default
    ) => callbackQuery switch
    {
        { Message: { } msg } => await callbackQuery.ThrowIfNull(nameof(callbackQuery))
            .FromCarrier()
            .MakeRequestAsync(
                request: new EditMessageMediaRequest(msg.Chat.Id, msg.MessageId, media)
                {
                    ReplyMarkup = replyMarkup
                },
                cancellationToken
            )
            .ConfigureAwait(false),
        { InlineMessageId: { } inlineMessageId } => (await callbackQuery.ThrowIfNull(nameof(callbackQuery))
            .FromCarrier()
            .MakeRequestAsync(
                request: new EditInlineMessageMediaRequest(inlineMessageId, media)
                {
                    ReplyMarkup = replyMarkup
                },
                cancellationToken
            )
            .ConfigureAwait(false))
            .FakeIfBool<Message>(),
        _ => throw new InvalidOperationException("At least one of Message or InlineMessageId should be available.")
    };

    /// <summary>
    /// Use this method to edit only the reply markup of messages.
    /// </summary>
    /// <param name="callbackQuery"></param>
    /// <param name="replyMarkup">
    /// Additional interface options. An <see cref="InlineKeyboardMarkup">inline keyboard</see>,
    /// <see cref="ReplyKeyboardMarkup">custom reply keyboard</see>, instructions to
    /// <see cref="ReplyKeyboardRemove">remove reply keyboard</see> or to
    /// <see cref="ForceReplyMarkup">force a reply</see> from the user
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    /// <returns>On success the edited <see cref="Message"/> is returned.</returns>
    public static async Task<Message?> EditReplyMarkupAsync(
        this CallbackQuery callbackQuery,
        InlineKeyboardMarkup? replyMarkup = default,
        CancellationToken cancellationToken = default
    ) => callbackQuery switch
    {
        { Message: { } msg } => await callbackQuery.ThrowIfNull(nameof(callbackQuery))
            .FromCarrier()
            .MakeRequestAsync(
                request: new EditMessageReplyMarkupRequest(msg.Chat.Id, msg.MessageId)
                {
                    ReplyMarkup = replyMarkup
                },
                cancellationToken
            )
            .ConfigureAwait(false),
        { InlineMessageId: { } inlineMessageId } => (await callbackQuery.ThrowIfNull(nameof(callbackQuery))
            .FromCarrier()
            .MakeRequestAsync(
                request: new EditInlineMessageReplyMarkupRequest(inlineMessageId)
                {
                    ReplyMarkup = replyMarkup
                },
                cancellationToken
            )
            .ConfigureAwait(false))
            .FakeIfBool<Message>(),
        _ => throw new InvalidOperationException("At least one of Message or InlineMessageId should be available.")
    };

    /// <summary>
    /// Use this method to stop updating a live location message before
    /// <see cref="Location.LivePeriod"/> expires.
    /// </summary>
    /// <param name="callbackQuery"></param>
    /// <param name="replyMarkup">
    /// Additional interface options. An <see cref="InlineKeyboardMarkup">inline keyboard</see>,
    /// <see cref="ReplyKeyboardMarkup">custom reply keyboard</see>, instructions to
    /// <see cref="ReplyKeyboardRemove">remove reply keyboard</see> or to
    /// <see cref="ForceReplyMarkup">force a reply</see> from the user
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    /// <returns>On success the sent <see cref="Message"/> is returned.</returns>
    public static async Task<Message?> StopLiveLocationAsync(
        this CallbackQuery callbackQuery,
        InlineKeyboardMarkup? replyMarkup = default,
        CancellationToken cancellationToken = default
    ) => callbackQuery switch
    {
        { Message: { ChatId: { } chatId } msg } => await callbackQuery
            .ThrowIfNull(nameof(callbackQuery))
                .FromCarrier()
                .MakeRequestAsync(
                    request: new StopMessageLiveLocationRequest(chatId, msg.MessageId)
                    {
                        ReplyMarkup = replyMarkup
                    },
                    cancellationToken
                )
                .ConfigureAwait(false),
        { InlineMessageId: { } inlineMessageId } => (await callbackQuery
            .ThrowIfNull(nameof(callbackQuery))
                .FromCarrier()
                .MakeRequestAsync(
                    request: new StopInlineMessageLiveLocationRequest(inlineMessageId)
                    {
                        ReplyMarkup = replyMarkup
                    },
                    cancellationToken
                )
                .ConfigureAwait(false))
                .FakeIfBool<Message>(),
        _ => throw new InvalidOperationException("At least one of Message or InlineMessageId should be available.")
    };

    #endregion

    #endregion
}
