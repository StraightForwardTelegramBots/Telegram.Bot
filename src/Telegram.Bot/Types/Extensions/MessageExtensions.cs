using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Extensions;
using Telegram.Bot.Requests;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.Payments;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Bot.Types;

/// <summary>
/// A set of extension methods for <see cref="Message"/>.
/// </summary>
public static class MessageExtensions
{
    #region Api Methods

    #region Replying to message ( Alias for Sending Messages )

    /// <summary>
    /// Use this method to send a reply message.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="asReply">To send the as reply to the current message.</param>
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
    public static async Task<Message> ReplyTextAsync(
        this Message message,
        string text,
        bool asReply = false,
        ParseMode? parseMode = default,
        IEnumerable<MessageEntity>? entities = default,
        bool? disableWebPagePreview = default,
        bool? disableNotification = default,
        bool? protectContent = default,
        int? replyToMessageId = default,
        bool? allowSendingWithoutReply = default,
        IReplyMarkup? replyMarkup = default,
        CancellationToken cancellationToken = default) =>
        await message.ThrowIfNull(nameof(message))
            .FromCarrier()
            .MakeRequestAsync(
                request: new SendMessageRequest(message.Chat.Id, text)
                {
                    ParseMode = parseMode ?? message.FromCarrier().ClientDefaults.ParseMode,
                    Entities = entities,
                    DisableWebPagePreview = disableWebPagePreview ?? message.FromCarrier().ClientDefaults.DisableWebPagePreview,
                    DisableNotification = disableNotification ?? message.FromCarrier().ClientDefaults.DisableNotification,
                    ProtectContent = protectContent ?? message.FromCarrier().ClientDefaults.ProtectContent,
                    ReplyToMessageId = replyToMessageId,
                    AllowSendingWithoutReply = allowSendingWithoutReply ?? message.FromCarrier().ClientDefaults.AllowSendingWithoutReply,
                    ReplyMarkup = replyMarkup
                }, cancellationToken
            )
            .ConfigureAwait(false);

    /// <summary>
    /// Use this method to send photos.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="asReply">To send the as reply to the current message.</param>
    /// <param name="photo">
    /// Photo to send. Pass a <see cref="InputTelegramFile.FileId"/> as String to send a photo that exists on
    /// the Telegram servers (recommended), pass an HTTP URL as a String for Telegram to get a photo from
    /// the Internet, or upload a new photo using multipart/form-data. The photo must be at most 10 MB in size.
    /// The photo's width and height must not exceed 10000 in total. Width and height ratio must be at most 20
    /// </param>
    /// <param name="caption">
    /// Photo caption (may also be used when resending photos by <see cref="InputTelegramFile.FileId"/>),
    /// 0-1024 characters after entities parsing
    /// </param>
    /// <param name="parseMode">
    /// Mode for parsing entities in the new caption. See
    /// <a href="https://core.telegram.org/bots/api#formatting-options">formatting</a> options for
    /// more details
    /// </param>
    /// <param name="captionEntities">
    /// List of special entities that appear in the caption, which can be specified instead
    /// of <see cref="ParseMode"/>
    /// </param>
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
    /// <see cref="ReplyKeyboardRemove">remove reply keyboard</see> or to
    /// <see cref="ForceReplyMarkup">force a reply</see> from the user
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    /// <returns>On success, the sent <see cref="Message"/> is returned.</returns>
    public static async Task<Message> ReplyPhotoAsync(
        this Message message,
        InputOnlineFile photo,
        string? caption = default,
        ParseMode? parseMode = default,
        bool asReply = false,
        IEnumerable<MessageEntity>? captionEntities = default,
        bool? disableNotification = default,
        bool? protectContent = default,
        int? replyToMessageId = default,
        bool? allowSendingWithoutReply = default,
        IReplyMarkup? replyMarkup = default,
        CancellationToken cancellationToken = default
    ) => await message.ThrowIfNull(nameof(message))
        .FromCarrier()
        .MakeRequestAsync(
            request: new SendPhotoRequest(message.Chat.Id, photo)
            {
                Caption = caption,
                ParseMode = parseMode ?? message.FromCarrier().ClientDefaults.ParseMode,
                CaptionEntities = captionEntities,
                ReplyToMessageId = asReply ? message.MessageId : replyToMessageId,
                DisableNotification = disableNotification ?? message.FromCarrier().ClientDefaults.DisableNotification,
                ProtectContent = protectContent ?? message.FromCarrier().ClientDefaults.ProtectContent,
                AllowSendingWithoutReply = allowSendingWithoutReply ?? message.FromCarrier().ClientDefaults.AllowSendingWithoutReply,
                ReplyMarkup = replyMarkup
            },
            cancellationToken
        )
        .ConfigureAwait(false);

    /// <summary>
    /// Use this method to send audio files, if you want Telegram clients to display them in the music player.
    /// Your audio must be in the .MP3 or .M4A format. Bots can currently send audio files of up to 50 MB in size,
    /// this limit may be changed in the future.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="asReply">To send the as reply to the current message.</param>
    /// <param name="audio">
    /// Audio file to send. Pass a <see cref="InputTelegramFile.FileId"/> as String to send an audio file that
    /// exists on the Telegram servers (recommended), pass an HTTP URL as a String for Telegram to get an audio
    /// file from the Internet, or upload a new one using multipart/form-data
    /// </param>
    /// <param name="caption">Audio caption, 0-1024 characters after entities parsing</param>
    /// <param name="parseMode">
    /// Mode for parsing entities in the new caption. See
    /// <a href="https://core.telegram.org/bots/api#formatting-options">formatting</a> options for
    /// more details
    /// </param>
    /// <param name="captionEntities">
    /// List of special entities that appear in the caption, which can be specified instead
    /// of <see cref="ParseMode"/>
    /// </param>
    /// <param name="duration">Duration of the audio in seconds</param>
    /// <param name="performer">Performer</param>
    /// <param name="title">Track name</param>
    /// <param name="thumb">
    /// Thumbnail of the file sent; can be ignored if thumbnail generation for the file is supported server-side.
    /// The thumbnail should be in JPEG format and less than 200 kB in size. A thumbnail's width and height
    /// should not exceed 320. Ignored if the file is not uploaded using multipart/form-data. Thumbnails can't be
    /// reused and can be only uploaded as a new file, so you can pass "attach://&lt;file_attach_name&gt;" if the
    /// thumbnail was uploaded using multipart/form-data under &lt;file_attach_name&gt;
    /// </param>
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
    /// <see cref="ReplyKeyboardRemove">remove reply keyboard</see> or to
    /// <see cref="ForceReplyMarkup">force a reply</see> from the user
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    /// <returns>On success, the sent <see cref="Message"/> is returned.</returns>
    public static async Task<Message> ReplyAudioAsync(
        this Message message,
        InputOnlineFile audio,
        string? caption = default,
        ParseMode? parseMode = default,
        bool asReply = false,
        IEnumerable<MessageEntity>? captionEntities = default,
        int? duration = default,
        string? performer = default,
        string? title = default,
        InputMedia? thumb = default,
        bool? disableNotification = default,
        bool? protectContent = default,
        int? replyToMessageId = default,
        bool? allowSendingWithoutReply = default,
        IReplyMarkup? replyMarkup = default,
        CancellationToken cancellationToken = default
    ) => await message.ThrowIfNull(nameof(message))
        .FromCarrier()
        .MakeRequestAsync(
            request: new SendAudioRequest(message.Chat.Id, audio)
            {
                Caption = caption,
                ParseMode = parseMode ?? message.FromCarrier().ClientDefaults.ParseMode,
                CaptionEntities = captionEntities,
                Duration = duration,
                Performer = performer,
                Title = title,
                Thumb = thumb,
                ReplyToMessageId = asReply ? message.MessageId : replyToMessageId,
                DisableNotification = disableNotification ?? message.FromCarrier().ClientDefaults.DisableNotification,
                ProtectContent = protectContent ?? message.FromCarrier().ClientDefaults.ProtectContent,
                AllowSendingWithoutReply = allowSendingWithoutReply ?? message.FromCarrier().ClientDefaults.AllowSendingWithoutReply,
                ReplyMarkup = replyMarkup
            },
            cancellationToken
        )
        .ConfigureAwait(false);

    /// <summary>
    /// Use this method to send general files. Bots can currently send files of any type of up to 50 MB in size,
    /// this limit may be changed in the future.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="asReply">To send the as reply to the current message.</param>
    /// <param name="document">
    /// File to send. Pass a <see cref="InputTelegramFile.FileId"/> as String to send a file that exists on the
    /// Telegram servers (recommended), pass an HTTP URL as a String for Telegram to get a file from the Internet,
    /// or upload a new one using multipart/form-data
    /// </param>
    /// <param name="thumb">
    /// Thumbnail of the file sent; can be ignored if thumbnail generation for the file is supported server-side.
    /// The thumbnail should be in JPEG format and less than 200 kB in size. A thumbnail's width and height should
    /// not exceed 320. Ignored if the file is not uploaded using multipart/form-data. Thumbnails can't be reused
    /// and can be only uploaded as a new file, so you can pass "attach://&lt;file_attach_name&gt;" if the
    /// thumbnail was uploaded using multipart/form-data under &lt;file_attach_name&gt;
    /// </param>
    /// <param name="caption">
    /// Document caption (may also be used when resending documents by file_id), 0-1024 characters after
    /// entities parsing
    /// </param>
    /// <param name="parseMode">
    /// Mode for parsing entities in the new caption. See
    /// <a href="https://core.telegram.org/bots/api#formatting-options">formatting</a> options for
    /// more details
    /// </param>
    /// <param name="captionEntities">
    /// List of special entities that appear in the caption, which can be specified instead
    /// of <see cref="ParseMode"/>
    /// </param>
    /// <param name="disableContentTypeDetection">
    /// Disables automatic server-side content type detection for files uploaded using multipart/form-data
    /// </param>
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
    /// <see cref="ReplyKeyboardRemove">remove reply keyboard</see> or to
    /// <see cref="ForceReplyMarkup">force a reply</see> from the user
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    /// <returns>On success, the sent <see cref="Message"/> is returned.</returns>
    public static async Task<Message> ReplyDocumentAsync(
        this Message message,
        InputOnlineFile document,
        InputMedia? thumb = default,
        string? caption = default,
        ParseMode? parseMode = default,
        bool asReply = false,
        IEnumerable<MessageEntity>? captionEntities = default,
        bool? disableContentTypeDetection = default,
        bool? disableNotification = default,
        bool? protectContent = default,
        int? replyToMessageId = default,
        bool? allowSendingWithoutReply = default,
        IReplyMarkup? replyMarkup = default,
        CancellationToken cancellationToken = default
    ) => await message.ThrowIfNull(nameof(message))
        .FromCarrier()
        .MakeRequestAsync(
            request: new SendDocumentRequest(message.Chat.Id, document)
            {
                Thumb = thumb,
                Caption = caption,
                ParseMode = parseMode ?? message.FromCarrier().ClientDefaults.ParseMode,
                CaptionEntities = captionEntities,
                DisableContentTypeDetection = disableContentTypeDetection,
                ReplyToMessageId = asReply ? message.MessageId : replyToMessageId,
                DisableNotification = disableNotification ?? message.FromCarrier().ClientDefaults.DisableNotification,
                ProtectContent = protectContent ?? message.FromCarrier().ClientDefaults.ProtectContent,
                AllowSendingWithoutReply = allowSendingWithoutReply ?? message.FromCarrier().ClientDefaults.AllowSendingWithoutReply,
                ReplyMarkup = replyMarkup
            },
            cancellationToken
        )
        .ConfigureAwait(false);

    /// <summary>
    /// Use this method to send video files, Telegram clients support mp4 videos (other formats may be sent as
    /// <see cref="Document"/>). Bots can currently send video files of up to 50 MB in size, this limit may be
    /// changed in the future.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="asReply">To send the as reply to the current message.</param>
    /// <param name="video">
    /// Video to send. Pass a <see cref="InputTelegramFile.FileId"/> as String to send a video that exists on
    /// the Telegram servers (recommended), pass an HTTP URL as a String for Telegram to get a video from the
    /// Internet, or upload a new video using multipart/form-data
    /// </param>
    /// <param name="duration">Duration of sent video in seconds</param>
    /// <param name="width">Video width</param>
    /// <param name="height">Video height</param>
    /// <param name="thumb">
    /// Thumbnail of the file sent; can be ignored if thumbnail generation for the file is supported server-side.
    /// The thumbnail should be in JPEG format and less than 200 kB in size. A thumbnail's width and height should
    /// not exceed 320. Ignored if the file is not uploaded using multipart/form-data. Thumbnails can't be reused
    /// and can be only uploaded as a new file, so you can pass "attach://&lt;file_attach_name&gt;" if the
    /// thumbnail was uploaded using multipart/form-data under &lt;file_attach_name&gt;
    /// </param>
    /// <param name="caption">
    /// Video caption (may also be used when resending videos by file_id), 0-1024 characters after entities parsing
    /// </param>
    /// <param name="parseMode">
    /// Mode for parsing entities in the new caption. See
    /// <a href="https://core.telegram.org/bots/api#formatting-options">formatting</a> options for
    /// more details
    /// </param>
    /// <param name="captionEntities">
    /// List of special entities that appear in the caption, which can be specified instead
    /// of <see cref="ParseMode"/>
    /// </param>
    /// <param name="supportsStreaming">Pass <c>true</c>, if the uploaded video is suitable for streaming</param>
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
    /// <see cref="ReplyKeyboardRemove">remove reply keyboard</see> or to
    /// <see cref="ForceReplyMarkup">force a reply</see> from the user
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    /// <returns>On success, the sent <see cref="Message"/> is returned.</returns>
    public static async Task<Message> ReplyVideoAsync(
        this Message message,
        InputOnlineFile video,
        int? duration = default,
        int? width = default,
        int? height = default,
        InputMedia? thumb = default,
        string? caption = default,
        ParseMode? parseMode = default,
        bool asReply = false,
        IEnumerable<MessageEntity>? captionEntities = default,
        bool? supportsStreaming = default,
        bool? disableNotification = default,
        bool? protectContent = default,
        int? replyToMessageId = default,
        bool? allowSendingWithoutReply = default,
        IReplyMarkup? replyMarkup = default,
        CancellationToken cancellationToken = default
    ) => await message.ThrowIfNull(nameof(message))
        .FromCarrier()
        .MakeRequestAsync(
            request: new SendVideoRequest(message.Chat.Id, video)
            {
                Duration = duration,
                Width = width,
                Height = height,
                Thumb = thumb,
                Caption = caption,
                ParseMode = parseMode ?? message.FromCarrier().ClientDefaults.ParseMode,
                CaptionEntities = captionEntities,
                SupportsStreaming = supportsStreaming,
                ReplyToMessageId = asReply ? message.MessageId : replyToMessageId,
                DisableNotification = disableNotification ?? message.FromCarrier().ClientDefaults.DisableNotification,
                ProtectContent = protectContent ?? message.FromCarrier().ClientDefaults.ProtectContent,
                AllowSendingWithoutReply = allowSendingWithoutReply ?? message.FromCarrier().ClientDefaults.AllowSendingWithoutReply,
                ReplyMarkup = replyMarkup
            },
            cancellationToken
        )
        .ConfigureAwait(false);

    /// <summary>
    /// Use this method to send animation files (GIF or H.264/MPEG-4 AVC video without sound). Bots can currently
    /// send animation files of up to 50 MB in size, this limit may be changed in the future.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="asReply">To send the as reply to the current message.</param>
    /// <param name="animation">
    /// Animation to send. Pass a <see cref="InputTelegramFile.FileId"/> as String to send an animation that
    /// exists on the Telegram servers (recommended), pass an HTTP URL as a String for Telegram to get an
    /// animation from the Internet, or upload a new animation using multipart/form-data
    /// </param>
    /// <param name="duration">Duration of sent animation in seconds</param>
    /// <param name="width">Animation width</param>
    /// <param name="height">Animation height</param>
    /// <param name="thumb">
    /// Thumbnail of the file sent; can be ignored if thumbnail generation for the file is supported server-side.
    /// The thumbnail should be in JPEG format and less than 200 kB in size. A thumbnail's width and height should
    /// not exceed 320. Ignored if the file is not uploaded using multipart/form-data. Thumbnails can't be reused
    /// and can be only uploaded as a new file, so you can pass "attach://&lt;file_attach_name&gt;" if the
    /// thumbnail was uploaded using multipart/form-data under &lt;file_attach_name&gt;
    /// </param>
    /// <param name="caption">
    /// Animation caption (may also be used when resending animation by <see cref="InputTelegramFile.FileId"/>),
    /// 0-1024 characters after entities parsing
    /// </param>
    /// <param name="parseMode">
    /// Mode for parsing entities in the new caption. See
    /// <a href="https://core.telegram.org/bots/api#formatting-options">formatting</a> options for
    /// more details
    /// </param>
    /// <param name="captionEntities">
    /// List of special entities that appear in the caption, which can be specified instead
    /// of <see cref="ParseMode"/>
    /// </param>
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
    /// <see cref="ReplyKeyboardRemove">remove reply keyboard</see> or to
    /// <see cref="ForceReplyMarkup">force a reply</see> from the user
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    /// <returns>On success, the sent <see cref="Message"/> is returned.</returns>
    public static async Task<Message> ReplyAnimationAsync(
        this Message message,
        InputOnlineFile animation,
        int? duration = default,
        int? width = default,
        int? height = default,
        InputMedia? thumb = default,
        string? caption = default,
        ParseMode? parseMode = default,
        bool asReply = false,
        IEnumerable<MessageEntity>? captionEntities = default,
        bool? disableNotification = default,
        bool? protectContent = default,
        int? replyToMessageId = default,
        bool? allowSendingWithoutReply = default,
        IReplyMarkup? replyMarkup = default,
        CancellationToken cancellationToken = default
    ) => await message.ThrowIfNull(nameof(message))
        .FromCarrier()
        .MakeRequestAsync(
            request: new SendAnimationRequest(message.Chat.Id, animation)
            {
                Duration = duration,
                Width = width,
                Height = height,
                Thumb = thumb,
                Caption = caption,
                ParseMode = parseMode ?? message.FromCarrier().ClientDefaults.ParseMode,
                CaptionEntities = captionEntities,
                ReplyToMessageId = asReply ? message.MessageId : replyToMessageId,
                DisableNotification = disableNotification ?? message.FromCarrier().ClientDefaults.DisableNotification,
                ProtectContent = protectContent ?? message.FromCarrier().ClientDefaults.ProtectContent,
                AllowSendingWithoutReply = allowSendingWithoutReply ?? message.FromCarrier().ClientDefaults.AllowSendingWithoutReply,
                ReplyMarkup = replyMarkup
            },
            cancellationToken
        )
        .ConfigureAwait(false);

    /// <summary>
    /// Use this method to send audio files, if you want Telegram clients to display the file as a playable voice
    /// message. For this to work, your audio must be in an .OGG file encoded with OPUS (other formats may be sent
    /// as <see cref="Audio"/> or <see cref="Document"/>). Bots can currently send voice messages of up to 50 MB
    /// in size, this limit may be changed in the future.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="asReply">To send the as reply to the current message.</param>
    /// <param name="voice">
    /// Audio file to send. Pass a <see cref="InputTelegramFile.FileId"/> as String to send a file that exists
    /// on the Telegram servers (recommended), pass an HTTP URL as a String for Telegram to get a file from
    /// the Internet, or upload a new one using multipart/form-data
    /// </param>
    /// <param name="caption">Voice message caption, 0-1024 characters after entities parsing</param>
    /// <param name="parseMode">
    /// Mode for parsing entities in the new caption. See
    /// <a href="https://core.telegram.org/bots/api#formatting-options">formatting</a> options for
    /// more details
    /// </param>
    /// <param name="captionEntities">
    /// List of special entities that appear in the caption, which can be specified instead
    /// of <see cref="ParseMode"/>
    /// </param>
    /// <param name="duration">Duration of the voice message in seconds</param>
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
    /// <see cref="ReplyKeyboardRemove">remove reply keyboard</see> or to
    /// <see cref="ForceReplyMarkup">force a reply</see> from the user
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    /// <returns>On success, the sent <see cref="Message"/> is returned.</returns>
    public static async Task<Message> ReplyVoiceAsync(
        this Message message,
        InputOnlineFile voice,
        bool asReply = false,
        string? caption = default,
        ParseMode? parseMode = default,
        IEnumerable<MessageEntity>? captionEntities = default,
        int? duration = default,
        bool? disableNotification = default,
        bool? protectContent = default,
        int? replyToMessageId = default,
        bool? allowSendingWithoutReply = default,
        IReplyMarkup? replyMarkup = default,
        CancellationToken cancellationToken = default
    ) => await message.ThrowIfNull(nameof(message))
        .FromCarrier()
        .MakeRequestAsync(
            request: new SendVoiceRequest(message.Chat.Id, voice)
            {
                Caption = caption,
                ParseMode = parseMode ?? message.FromCarrier().ClientDefaults.ParseMode,
                CaptionEntities = captionEntities,
                Duration = duration,
                ReplyToMessageId = asReply ? message.MessageId : replyToMessageId,
                DisableNotification = disableNotification ?? message.FromCarrier().ClientDefaults.DisableNotification,
                ProtectContent = protectContent ?? message.FromCarrier().ClientDefaults.ProtectContent,
                AllowSendingWithoutReply = allowSendingWithoutReply ?? message.FromCarrier().ClientDefaults.AllowSendingWithoutReply,
                ReplyMarkup = replyMarkup
            },
            cancellationToken
        )
        .ConfigureAwait(false);

    /// <summary>
    /// As of <a href="https://telegram.org/blog/video-messages-and-telescope">v.4.0</a>, Telegram clients
    /// support rounded square mp4 videos of up to 1 minute long. Use this method to send video messages.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="asReply">To send the as reply to the current message.</param>
    /// <param name="videoNote">
    /// Video note to send. Pass a <see cref="InputTelegramFile.FileId"/> as String to send a video note that
    /// exists on the Telegram servers (recommended) or upload a new video using multipart/form-data. Sending
    /// video notes by a URL is currently unsupported
    /// </param>
    /// <param name="duration">Duration of sent video in seconds</param>
    /// <param name="length">Video width and height, i.e. diameter of the video message</param>
    /// <param name="thumb">
    /// Thumbnail of the file sent; can be ignored if thumbnail generation for the file is supported server-side.
    /// The thumbnail should be in JPEG format and less than 200 kB in size. A thumbnail's width and height should
    /// not exceed 320. Ignored if the file is not uploaded using multipart/form-data. Thumbnails can't be reused
    /// and can be only uploaded as a new file, so you can pass "attach://&lt;file_attach_name&gt;" if the
    /// thumbnail was uploaded using multipart/form-data under &lt;file_attach_name&gt;
    /// </param>
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
    /// <see cref="ReplyKeyboardRemove">remove reply keyboard</see> or to
    /// <see cref="ForceReplyMarkup">force a reply</see> from the user
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    /// <returns>On success, the sent <see cref="Message"/> is returned.</returns>
    public static async Task<Message> ReplyVideoNoteAsync(
        this Message message,
        InputTelegramFile videoNote,
        bool asReply = false,
        int? duration = default,
        int? length = default,
        InputMedia? thumb = default,
        bool? disableNotification = default,
        bool? protectContent = default,
        int? replyToMessageId = default,
        bool? allowSendingWithoutReply = default,
        IReplyMarkup? replyMarkup = default,
        CancellationToken cancellationToken = default
    ) => await message.ThrowIfNull(nameof(message))
        .FromCarrier()
        .MakeRequestAsync(
            request: new SendVideoNoteRequest(message.Chat.Id, videoNote)
            {
                Duration = duration,
                Length = length,
                Thumb = thumb,
                ReplyToMessageId = asReply ? message.MessageId : replyToMessageId,
                DisableNotification = disableNotification ?? message.FromCarrier().ClientDefaults.DisableNotification,
                ProtectContent = protectContent ?? message.FromCarrier().ClientDefaults.ProtectContent,
                AllowSendingWithoutReply = allowSendingWithoutReply ?? message.FromCarrier().ClientDefaults.AllowSendingWithoutReply,
                ReplyMarkup = replyMarkup
            },
            cancellationToken
        )
        .ConfigureAwait(false);

    /// <summary>
    /// Use this method to send a group of photos, videos, documents or audios as an album. Documents and audio
    /// files can be only grouped in an album with messages of the same type.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="asReply">To send the as reply to the current message.</param>
    /// <param name="media">An array describing messages to be sent, must include 2-10 items</param>
    /// <param name="disableNotification">
    /// Sends the message silently. Users will receive a notification with no sound
    /// </param>
    /// <param name="protectContent">Protects the contents of sent messages from forwarding and saving</param>
    /// <param name="replyToMessageId">If the message is a reply, ID of the original message</param>
    /// <param name="allowSendingWithoutReply">
    /// Pass <c>true</c>, if the message should be sent even if the specified replied-to message is not found
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    /// <returns>On success, an array of <see cref="Message"/>s that were sent is returned.</returns>
    public static async Task<Message[]> ReplyMediaGroupAsync(
        this Message message,
        IEnumerable<IAlbumInputMedia> media,
        bool asReply = false,
        bool? disableNotification = default,
        bool? protectContent = default,
        int? replyToMessageId = default,
        bool? allowSendingWithoutReply = default,
        CancellationToken cancellationToken = default
    ) => await message.ThrowIfNull(nameof(message))
        .FromCarrier()
        .MakeRequestAsync(
            request: new SendMediaGroupRequest(message.Chat.Id, media)
            {
                ReplyToMessageId = asReply ? message.MessageId : replyToMessageId,
                DisableNotification = disableNotification ?? message.FromCarrier().ClientDefaults.DisableNotification,
                ProtectContent = protectContent ?? message.FromCarrier().ClientDefaults.ProtectContent,
                AllowSendingWithoutReply = allowSendingWithoutReply ?? message.FromCarrier().ClientDefaults.AllowSendingWithoutReply
            },
            cancellationToken
        )
        .ConfigureAwait(false);

    /// <summary>
    /// Use this method to send point on the map.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="asReply">To send the as reply to the current message.</param>
    /// <param name="latitude">Latitude of location</param>
    /// <param name="longitude">Longitude of location</param>
    /// <param name="livePeriod">
    /// Period in seconds for which the location will be updated, should be between 60 and 86400
    /// </param>
    /// <param name="heading">
    /// For live locations, a direction in which the user is moving, in degrees. Must be between 1 and 360
    /// if specified
    /// </param>
    /// <param name="proximityAlertRadius">
    /// For live locations, a maximum distance for proximity alerts about approaching another chat member,
    /// in meters. Must be between 1 and 100000 if specified
    /// </param>
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
    /// <see cref="ReplyKeyboardRemove">remove reply keyboard</see> or to
    /// <see cref="ForceReplyMarkup">force a reply</see> from the user
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    /// <returns>On success, the sent <see cref="Message"/> is returned.</returns>
    public static async Task<Message> ReplyLocationAsync(
        this Message message,
        double latitude,
        double longitude,
        bool asReply = false,
        int? livePeriod = default,
        int? heading = default,
        int? proximityAlertRadius = default,
        bool? disableNotification = default,
        bool? protectContent = default,
        int? replyToMessageId = default,
        bool? allowSendingWithoutReply = default,
        IReplyMarkup? replyMarkup = default,
        CancellationToken cancellationToken = default
    ) => await message.ThrowIfNull(nameof(message))
        .FromCarrier()
        .MakeRequestAsync(
            request: new SendLocationRequest(message.Chat.Id, latitude, longitude)
            {
                LivePeriod = livePeriod,
                Heading = heading,
                ProximityAlertRadius = proximityAlertRadius,
                ReplyToMessageId = asReply ? message.MessageId : replyToMessageId,
                DisableNotification = disableNotification ?? message.FromCarrier().ClientDefaults.DisableNotification,
                ProtectContent = protectContent ?? message.FromCarrier().ClientDefaults.ProtectContent,
                AllowSendingWithoutReply = allowSendingWithoutReply ?? message.FromCarrier().ClientDefaults.AllowSendingWithoutReply,
                ReplyMarkup = replyMarkup
            },
            cancellationToken
        )
        .ConfigureAwait(false);

    /// <summary>
    /// Use this method to send information about a venue.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="asReply">To send the as reply to the current message.</param>
    /// <param name="latitude">Latitude of the venue</param>
    /// <param name="longitude">Longitude of the venue</param>
    /// <param name="title">Name of the venue</param>
    /// <param name="address">Address of the venue</param>
    /// <param name="foursquareId">Foursquare identifier of the venue</param>
    /// <param name="foursquareType">
    /// Foursquare type of the venue, if known. (For example, “arts_entertainment/default”,
    /// “arts_entertainment/aquarium” or “food/icecream”.)
    /// </param>
    /// <param name="googlePlaceId">Google Places identifier of the venue</param>
    /// <param name="googlePlaceType">
    /// Google Places type of the venue. (See
    /// <a href="https://developers.google.com/places/web-service/supported_types">supported types</a>)
    /// </param>
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
    /// <see cref="ReplyKeyboardRemove">remove reply keyboard</see> or to
    /// <see cref="ForceReplyMarkup">force a reply</see> from the user
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    /// <returns>On success, the sent <see cref="Message"/> is returned.</returns>
    /// <a href="https://core.telegram.org/bots/api#sendvenue"/>
    public static async Task<Message> ReplyVenueAsync(
        this Message message,
        double latitude,
        double longitude,
        string title,
        string address,
        bool asReply = false,
        string? foursquareId = default,
        string? foursquareType = default,
        string? googlePlaceId = default,
        string? googlePlaceType = default,
        bool? disableNotification = default,
        bool? protectContent = default,
        int? replyToMessageId = default,
        bool? allowSendingWithoutReply = default,
        IReplyMarkup? replyMarkup = default,
        CancellationToken cancellationToken = default
    ) => await message.ThrowIfNull(nameof(message))
        .FromCarrier()
        .MakeRequestAsync(
            request: new SendVenueRequest(message.Chat.Id, latitude, longitude, title, address)
            {
                FoursquareId = foursquareId,
                FoursquareType = foursquareType,
                GooglePlaceId = googlePlaceId,
                GooglePlaceType = googlePlaceType,
                ReplyToMessageId = asReply ? message.MessageId : replyToMessageId,
                DisableNotification = disableNotification ?? message.FromCarrier().ClientDefaults.DisableNotification,
                ProtectContent = protectContent ?? message.FromCarrier().ClientDefaults.ProtectContent,
                AllowSendingWithoutReply = allowSendingWithoutReply ?? message.FromCarrier().ClientDefaults.AllowSendingWithoutReply,
                ReplyMarkup = replyMarkup
            },
            cancellationToken
        )
        .ConfigureAwait(false);

    /// <summary>
    /// Use this method to send phone contacts.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="asReply">To send the as reply to the current message.</param>
    /// <param name="phoneNumber">Contact's phone number</param>
    /// <param name="firstName">Contact's first name</param>
    /// <param name="lastName">Contact's last name</param>
    /// <param name="vCard">Additional data about the contact in the form of a vCard, 0-2048 bytes</param>
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
    /// <see cref="ReplyKeyboardRemove">remove reply keyboard</see> or to
    /// <see cref="ForceReplyMarkup">force a reply</see> from the user
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    /// <returns>On success, the sent <see cref="Message"/> is returned.</returns>
    public static async Task<Message> ReplyContactAsync(
        this Message message,
        string phoneNumber,
        string firstName,
        bool asReply = false,
        string? lastName = default,
        string? vCard = default,
        bool? disableNotification = default,
        bool? protectContent = default,
        int? replyToMessageId = default,
        bool? allowSendingWithoutReply = default,
        IReplyMarkup? replyMarkup = default,
        CancellationToken cancellationToken = default
    ) => await message.ThrowIfNull(nameof(message))
        .FromCarrier()
        .MakeRequestAsync(
            request: new SendContactRequest(message.Chat.Id, phoneNumber, firstName)
            {
                LastName = lastName,
                Vcard = vCard,
                ReplyToMessageId = asReply ? message.MessageId : replyToMessageId,
                DisableNotification = disableNotification ?? message.FromCarrier().ClientDefaults.DisableNotification,
                ProtectContent = protectContent ?? message.FromCarrier().ClientDefaults.ProtectContent,
                AllowSendingWithoutReply = allowSendingWithoutReply ?? message.FromCarrier().ClientDefaults.AllowSendingWithoutReply,
                ReplyMarkup = replyMarkup
            },
            cancellationToken
        )
        .ConfigureAwait(false);

    /// <summary>
    /// Use this method to send a native poll.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="asReply">To send the as reply to the current message.</param>
    /// <param name="question">Poll question, 1-300 characters</param>
    /// <param name="options">A list of answer options, 2-10 strings 1-100 characters each</param>
    /// <param name="isAnonymous"><c>true</c>, if the poll needs to be anonymous, defaults to <c>true</c></param>
    /// <param name="type">
    /// Poll type, <see cref="PollType.Quiz"/> or <see cref="PollType.Regular"/>,
    /// defaults to <see cref="PollType.Regular"/>
    /// </param>
    /// <param name="allowsMultipleAnswers">
    /// <c>true</c>, if the poll allows multiple answers, ignored for polls in quiz mode,
    /// defaults to <c>false</c>
    /// </param>
    /// <param name="correctOptionId">
    /// 0-based identifier of the correct answer option, required for polls in quiz mode
    /// </param>
    /// <param name="explanation">
    /// Text that is shown when a user chooses an incorrect answer or taps on the lamp icon in a quiz-style poll,
    /// 0-200 characters with at most 2 line feeds after entities parsing
    /// </param>
    /// <param name="explanationParseMode">
    /// Mode for parsing entities in the explanation. See
    /// <a href="https://core.telegram.org/bots/api#formatting-options">formatting options</a>
    /// for more details
    /// </param>
    /// <param name="explanationEntities">
    /// List of special entities that appear in the poll explanation, which can be specified instead
    /// of <see cref="ParseMode"/>
    /// </param>
    /// <param name="openPeriod">
    /// Amount of time in seconds the poll will be active after creation, 5-600. Can't be used together
    /// with <paramref name="closeDate"/>
    /// </param>
    /// <param name="closeDate">
    /// Point in time when the poll will be automatically closed. Must be at least 5 and no more than 600 seconds
    /// in the future. Can't be used together with <paramref name="openPeriod"/>
    /// </param>
    /// <param name="isClosed">
    /// Pass <c>true</c>, if the poll needs to be immediately closed. This can be useful for poll preview
    /// </param>
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
    /// <see cref="ReplyKeyboardRemove">remove reply keyboard</see> or to
    /// <see cref="ForceReplyMarkup">force a reply</see> from the user
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    /// <returns>On success, the sent <see cref="Message"/> is returned.</returns>
    public static async Task<Message> ReplyPollAsync(
        this Message message,
        string question,
        IEnumerable<string> options,
        bool asReply = false,
        bool? isAnonymous = default,
        PollType? type = default,
        bool? allowsMultipleAnswers = default,
        int? correctOptionId = default,
        string? explanation = default,
        ParseMode? explanationParseMode = default,
        IEnumerable<MessageEntity>? explanationEntities = default,
        int? openPeriod = default,
        DateTime? closeDate = default,
        bool? isClosed = default,
        bool? disableNotification = default,
        bool? protectContent = default,
        int? replyToMessageId = default,
        bool? allowSendingWithoutReply = default,
        IReplyMarkup? replyMarkup = default,
        CancellationToken cancellationToken = default
    ) => await message.ThrowIfNull(nameof(message))
        .FromCarrier()
        .MakeRequestAsync(
            request: new SendPollRequest(message.Chat.Id, question, options)
            {
                ReplyToMessageId = asReply ? message.MessageId : replyToMessageId,
                DisableNotification = disableNotification ?? message.FromCarrier().ClientDefaults.DisableNotification,
                ProtectContent = protectContent ?? message.FromCarrier().ClientDefaults.ProtectContent,
                AllowSendingWithoutReply = allowSendingWithoutReply ?? message.FromCarrier().ClientDefaults.AllowSendingWithoutReply,
                ReplyMarkup = replyMarkup,
                IsAnonymous = isAnonymous,
                Type = type,
                AllowsMultipleAnswers = allowsMultipleAnswers,
                CorrectOptionId = correctOptionId,
                Explanation = explanation,
                ExplanationParseMode = explanationParseMode,
                ExplanationEntities = explanationEntities,
                OpenPeriod = openPeriod,
                CloseDate = closeDate,
                IsClosed = isClosed
            },
            cancellationToken
        )
        .ConfigureAwait(false);

    /// <summary>
    /// Use this method to send an animated emoji that will display a random value.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="asReply">To send the as reply to the current message.</param>
    /// <param name="emoji">
    /// Emoji on which the dice throw animation is based. Currently, must be one of <see cref="Emoji.Dice"/>,
    /// <see cref="Emoji.Darts"/>, <see cref="Emoji.Basketball"/>, <see cref="Emoji.Football"/>,
    /// <see cref="Emoji.Bowling"/> or <see cref="Emoji.SlotMachine"/>. Dice can have values 1-6 for
    /// <see cref="Emoji.Dice"/>, <see cref="Emoji.Darts"/> and <see cref="Emoji.Bowling"/>, values 1-5 for
    /// <see cref="Emoji.Basketball"/> and <see cref="Emoji.Football"/>, and values 1-64 for
    /// <see cref="Emoji.SlotMachine"/>. Defauts to <see cref="Emoji.Dice"/>
    /// </param>
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
    /// <see cref="ReplyKeyboardRemove">remove reply keyboard</see> or to
    /// <see cref="ForceReplyMarkup">force a reply</see> from the user
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    /// <returns>On success, the sent <see cref="Message"/> is returned.</returns>
    public static async Task<Message> ReplyDiceAsync(
        this Message message,
        bool asReply = false,
        Emoji? emoji = default,
        bool? disableNotification = default,
        bool? protectContent = default,
        int? replyToMessageId = default,
        bool? allowSendingWithoutReply = default,
        IReplyMarkup? replyMarkup = default,
        CancellationToken cancellationToken = default
    ) => await message.ThrowIfNull(nameof(message))
        .FromCarrier()
        .MakeRequestAsync(
            request: new SendDiceRequest(message.Chat.Id)
            {
                ReplyToMessageId = asReply ? message.MessageId : replyToMessageId,
                DisableNotification = disableNotification ?? message.FromCarrier().ClientDefaults.DisableNotification,
                ProtectContent = protectContent ?? message.FromCarrier().ClientDefaults.ProtectContent,
                AllowSendingWithoutReply = allowSendingWithoutReply ?? message.FromCarrier().ClientDefaults.AllowSendingWithoutReply,
                ReplyMarkup = replyMarkup,
                Emoji = emoji
            },
            cancellationToken
        )
        .ConfigureAwait(false);

    /// <summary>
    /// Use this method to send a game.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="asReply">To send the as reply to the current message.</param>
    /// <param name="gameShortName">
    /// Short name of the game, serves as the unique identifier for the game. Set up your games via
    /// <a href="https://t.me/botfather">@Botfather</a>
    /// </param>
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
    /// <see cref="ReplyKeyboardRemove">remove reply keyboard</see> or to
    /// <see cref="ForceReplyMarkup">force a reply</see> from the user
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    /// <returns>On success, the sent <see cref="Message"/> is returned.</returns>
    public static async Task<Message> ReplyGameAsync(
        this Message message,
        string gameShortName,
        bool asReply = false,
        bool? disableNotification = default,
        bool? protectContent = default,
        int? replyToMessageId = default,
        bool? allowSendingWithoutReply = default,
        InlineKeyboardMarkup? replyMarkup = default,
        CancellationToken cancellationToken = default
    ) => await message.ThrowIfNull(nameof(message))
        .FromCarrier()
        .MakeRequestAsync(
            request: new SendGameRequest(message.Chat.Id, gameShortName)
            {
                ReplyToMessageId = asReply ? message.MessageId : replyToMessageId,
                DisableNotification = disableNotification ?? message.FromCarrier().ClientDefaults.DisableNotification,
                ProtectContent = protectContent ?? message.FromCarrier().ClientDefaults.ProtectContent,
                AllowSendingWithoutReply = allowSendingWithoutReply ?? message.FromCarrier().ClientDefaults.AllowSendingWithoutReply,
                ReplyMarkup = replyMarkup
            },
            cancellationToken
        )
        .ConfigureAwait(false);

    /// <summary>
    /// Use this method to send invoices.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="asReply">To send the as reply to the current message.</param>
    /// <param name="title">Product name, 1-32 characters</param>
    /// <param name="description">Product description, 1-255 characters</param>
    /// <param name="payload">
    /// Bot-defined invoice payload, 1-128 bytes. This will not be displayed to the user,
    /// use for your internal processes
    /// </param>
    /// <param name="providerToken">
    /// Payments provider token, obtained via <a href="https://t.me/botfather">@Botfather</a>
    /// </param>
    /// <param name="currency">
    /// Three-letter ISO 4217 currency code, see
    /// <a href="https://core.telegram.org/bots/payments#supported-currencies">more on currencies</a>
    /// </param>
    /// <param name="prices">
    /// Price breakdown, a list of components (e.g. product price, tax, discount, delivery cost, delivery tax,
    /// bonus, etc.)
    /// </param>
    /// <param name="maxTipAmount">
    /// The maximum accepted amount for tips in the smallest units of the currency (integer, not float/double).
    /// For example, for a maximum tip of <c>US$ 1.45</c> pass <c><paramref name="maxTipAmount"/> = 145</c>.
    /// See the <i>exp</i> parameter in
    /// <a href="https://core.telegram.org/bots/payments/currencies.json">currencies.json</a>, it shows the
    /// number of digits past the decimal point for each currency (2 for the majority of currencies).
    /// Defaults to 0
    /// </param>
    /// <param name="suggestedTipAmounts">
    /// An array of suggested amounts of tips in the <i>smallest units</i> of the currency (integer,
    /// <b>not</b> float/double). At most 4 suggested tip amounts can be specified. The suggested tip amounts must
    /// be positive, passed in a strictly increased order and must not exceed <paramref name="maxTipAmount"/>
    /// </param>
    /// <param name="startParameter">
    /// Unique deep-linking parameter. If left empty, <b>forwarded copies</b> of the sent message will have
    /// a <i>Pay</i> button, allowing multiple users to pay directly from the forwarded message, using the same
    /// invoice. If non-empty, forwarded copies of the sent message will have a <i>URL</i> button with a deep
    /// link to the bot (instead of a <i>Pay</i> button), with the value used as the start parameter
    /// </param>
    /// <param name="providerData">
    /// A JSON-serialized data about the invoice, which will be shared with the payment provider. A detailed
    /// description of required fields should be provided by the payment provide
    /// </param>
    /// <param name="photoUrl">
    /// URL of the product photo for the invoice. Can be a photo of the goods or a marketing image for a service.
    /// People like it better when they see what they are paying for
    /// </param>
    /// <param name="photoSize">Photo size</param>
    /// <param name="photoWidth">Photo width</param>
    /// <param name="photoHeight">Photo height</param>
    /// <param name="needName">Pass <c>true</c>, if you require the user's full name to complete the order</param>
    /// <param name="needPhoneNumber">
    /// Pass <c>true</c>, if you require the user's phone number to complete the order
    /// </param>
    /// <param name="needEmail">Pass <c>true</c>, if you require the user's email to complete the order</param>
    /// <param name="needShippingAddress">
    /// Pass <c>true</c>, if you require the user's shipping address to complete the order
    /// </param>
    /// <param name="sendPhoneNumberToProvider">
    /// Pass <c>true</c>, if user's phone number should be sent to provider
    /// </param>
    /// <param name="sendEmailToProvider">
    /// Pass <c>true</c>, if user's email address should be sent to provider
    /// </param>
    /// <param name="isFlexible">Pass <c>true</c>, if the final price depends on the shipping method</param>
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
    /// <see cref="ReplyKeyboardRemove">remove reply keyboard</see> or to
    /// <see cref="ForceReplyMarkup">force a reply</see> from the user
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    /// <returns>On success, the sent <see cref="Message"/> is returned.</returns>
    public static async Task<Message> ReplyInvoiceAsync(
        this Message message,
        string title,
        string description,
        string payload,
        string providerToken,
        string currency,
        IEnumerable<LabeledPrice> prices,
        bool asReply = false,
        int? maxTipAmount = default,
        IEnumerable<int>? suggestedTipAmounts = default,
        string? startParameter = default,
        string? providerData = default,
        string? photoUrl = default,
        int? photoSize = default,
        int? photoWidth = default,
        int? photoHeight = default,
        bool? needName = default,
        bool? needPhoneNumber = default,
        bool? needEmail = default,
        bool? needShippingAddress = default,
        bool? sendPhoneNumberToProvider = default,
        bool? sendEmailToProvider = default,
        bool? isFlexible = default,
        bool? disableNotification = default,
        bool? protectContent = default,
        int? replyToMessageId = default,
        bool? allowSendingWithoutReply = default,
        InlineKeyboardMarkup? replyMarkup = default,
        CancellationToken cancellationToken = default
    ) => await message.ThrowIfNull(nameof(message))
            .FromCarrier()
            .MakeRequestAsync(
                request: new SendInvoiceRequest(
                    message.Chat.Id,
                    title,
                    description,
                    payload,
                    providerToken,
                    currency,
                    // ReSharper disable once PossibleMultipleEnumeration
                    prices)
                {
                    MaxTipAmount = maxTipAmount,
                    SuggestedTipAmounts = suggestedTipAmounts,
                    StartParameter = startParameter,
                    ProviderData = providerData,
                    PhotoUrl = photoUrl,
                    PhotoSize = photoSize,
                    PhotoWidth = photoWidth,
                    PhotoHeight = photoHeight,
                    NeedName = needName,
                    NeedPhoneNumber = needPhoneNumber,
                    NeedEmail = needEmail,
                    NeedShippingAddress = needShippingAddress,
                    SendPhoneNumberToProvider = sendPhoneNumberToProvider,
                    SendEmailToProvider = sendEmailToProvider,
                    IsFlexible = isFlexible,
                    DisableNotification = disableNotification ?? message.FromCarrier().ClientDefaults.DisableNotification,
                    ProtectContent = protectContent ?? message.FromCarrier().ClientDefaults.ProtectContent,
                    ReplyToMessageId = asReply ? message.MessageId : replyToMessageId,
                    AllowSendingWithoutReply = allowSendingWithoutReply ?? message.FromCarrier().ClientDefaults.AllowSendingWithoutReply,
                    ReplyMarkup = replyMarkup
                },
                cancellationToken
            )
            .ConfigureAwait(false);

    /// <summary>
    /// Use this method to send static .WEBP or animated .TGS stickers.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="asReply">To send the as reply to the current message.</param>
    /// <param name="sticker">
    /// Sticker to send. Pass a <see cref="InputTelegramFile.FileId"/> as String to send a file that exists on
    /// the Telegram servers (recommended), pass an HTTP URL as a String for Telegram to get a .WEBP file from
    /// the Internet, or upload a new one using multipart/form-data
    /// </param>
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
    /// <see cref="ReplyKeyboardRemove">remove reply keyboard</see> or to
    /// <see cref="ForceReplyMarkup">force a reply</see> from the user
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    /// <returns>On success, the sent <see cref="Message"/> is returned.</returns>
    public static async Task<Message> ReplyStickerAsync(
        this Message message,
        InputOnlineFile sticker,
        bool asReply = false,
        bool? disableNotification = default,
        bool? protectContent = default,
        int? replyToMessageId = default,
        bool? allowSendingWithoutReply = default,
        IReplyMarkup? replyMarkup = default,
        CancellationToken cancellationToken = default
    ) =>
        await message.ThrowIfNull(nameof(message))
            .FromCarrier()
            .MakeRequestAsync(
                request: new SendStickerRequest(message.Chat.Id, sticker)
                {
                    DisableNotification = disableNotification ?? message.FromCarrier().ClientDefaults.DisableNotification,
                    ProtectContent = protectContent ?? message.FromCarrier().ClientDefaults.ProtectContent,
                    ReplyToMessageId = replyToMessageId,
                    AllowSendingWithoutReply = allowSendingWithoutReply ?? message.FromCarrier().ClientDefaults.AllowSendingWithoutReply,
                    ReplyMarkup = replyMarkup
                },
                cancellationToken
            )
            .ConfigureAwait(false);

    #endregion

    #region Updating this message


    /// <summary>
    /// Use this method to edit text and game of this message.
    /// </summary>
    /// <param name="message"></param>
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
    public static async Task<Message> EditTextAsync(
        this Message message,
        string text,
        ParseMode? parseMode = default,
        IEnumerable<MessageEntity>? entities = default,
        bool? disableWebPagePreview = default,
        InlineKeyboardMarkup? replyMarkup = default,
        CancellationToken cancellationToken = default
    ) =>
        await message.ThrowIfNull(nameof(message))
            .FromCarrier()
            .MakeRequestAsync(
                request: new EditMessageTextRequest(message.Chat.Id, message.MessageId, text)
                {
                    ParseMode = parseMode ?? message.FromCarrier().ClientDefaults.ParseMode,
                    Entities = entities,
                    DisableWebPagePreview = disableWebPagePreview ?? message.FromCarrier().ClientDefaults.DisableWebPagePreview,
                    ReplyMarkup = replyMarkup
                },
                cancellationToken
            )
            .ConfigureAwait(false);

    /// <summary>
    /// Use this method to edit captions of this message.
    /// </summary>
    /// <param name="message"></param>
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
    public static async Task<Message> EditCaptionAsync(
        this Message message,
        string? caption,
        ParseMode? parseMode = default,
        IEnumerable<MessageEntity>? captionEntities = default,
        InlineKeyboardMarkup? replyMarkup = default,
        CancellationToken cancellationToken = default
    ) => await message.ThrowIfNull(nameof(message))
        .FromCarrier()
        .MakeRequestAsync(
            request: new EditMessageCaptionRequest(message.Chat.Id, message.MessageId)
            {
                Caption = caption,
                ParseMode = parseMode ?? message.FromCarrier().ClientDefaults.ParseMode,
                CaptionEntities = captionEntities,
                ReplyMarkup = replyMarkup
            },
            cancellationToken
        )
        .ConfigureAwait(false);

    /// <summary>
    /// Use this method to edit animation, audio, document, photo, or video of this message. If a message is part of
    /// a message album, then it can be edited only to an audio for audio albums, only to a document for document
    /// albums and to a photo or a video otherwise. Use a previously uploaded file via its
    /// <see cref="InputTelegramFile.FileId"/> or specify a URL
    /// </summary>
    /// <param name="message"></param>
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
    public static async Task<Message> EditMediaAsync(
        this Message message,
        InputMediaBase media,
        InlineKeyboardMarkup? replyMarkup = default,
        CancellationToken cancellationToken = default
    ) => await message.ThrowIfNull(nameof(message))
        .FromCarrier()
        .MakeRequestAsync(
            request: new EditMessageMediaRequest(message.Chat.Id, message.MessageId, media)
            {
                ReplyMarkup = replyMarkup
            },
            cancellationToken
        )
        .ConfigureAwait(false);

    /// <summary>
    /// Use this method to edit only the reply markup of this message.
    /// </summary>
    /// <param name="message"></param>
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
    public static async Task<Message> EditReplyMarkupAsync(
        this Message message,
        InlineKeyboardMarkup? replyMarkup = default,
        CancellationToken cancellationToken = default
    ) => await message.ThrowIfNull(nameof(message))
        .FromCarrier()
        .MakeRequestAsync(
            request: new EditMessageReplyMarkupRequest(message.Chat.Id, message.MessageId)
            {
                ReplyMarkup = replyMarkup
            },
            cancellationToken
        )
        .ConfigureAwait(false);

    /// <summary>
    /// Use this method to edit live location messages. A location can be edited until its
    /// <see cref="Location.LivePeriod"/> expires or editing is explicitly disabled by a call to
    /// <see cref="TelegramBotClientExtensions.StopMessageLiveLocationAsync(ITelegramBotClient, ChatId, int, InlineKeyboardMarkup?, CancellationToken)"/>.
    /// </summary>
    /// <param name="message"></param>
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
    public static async Task<Message> EditLiveLocationAsync(
        this Message message,
        double latitude,
        double longitude,
        float? horizontalAccuracy = default,
        int? heading = default,
        int? proximityAlertRadius = default,
        InlineKeyboardMarkup? replyMarkup = default,
        CancellationToken cancellationToken = default
    ) => await message.ThrowIfNull(nameof(message))
        .FromCarrier()
        .MakeRequestAsync(
            request: new EditMessageLiveLocationRequest(
                message.Chat.Id, message.MessageId, latitude, longitude)
            {
                HorizontalAccuracy = horizontalAccuracy,
                Heading = heading,
                ProximityAlertRadius = proximityAlertRadius,
                ReplyMarkup = replyMarkup
            },
            cancellationToken
        )
        .ConfigureAwait(false);

    /// <summary>
    /// Use this method to delete this message, including service messages, with the following limitations:
    /// <list type="bullet">
    /// <item>A message can only be deleted if it was sent less than 48 hours ago</item>
    /// <item>A dice message in a private chat can only be deleted if it was sent more than 24 hours ago</item>
    /// <item>Bots can delete outgoing messages in private chats, groups, and supergroups</item>
    /// <item>Bots can delete incoming messages in private chats</item>
    /// <item>Bots granted can_post_messages permissions can delete outgoing messages in channels</item>
    /// <item>If the bot is an administrator of a group, it can delete any message there</item>
    /// <item>
    /// If the bot has can_delete_messages permission in a supergroup or a channel, it can delete any message there
    /// </item>
    /// </list>
    /// </summary>
    /// <param name="message"></param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    public static async Task DeleteAsync(
        this Message message,
        CancellationToken cancellationToken = default
    ) =>
        await message.ThrowIfNull(nameof(message))
            .FromCarrier()
            .MakeRequestAsync(request: new DeleteMessageRequest(message.Chat.Id, message.MessageId), cancellationToken)
            .ConfigureAwait(false);

    /// <summary>
    /// Use this method to stop a poll which was sent by the bot.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="replyMarkup">
    /// Additional interface options. An <see cref="InlineKeyboardMarkup">inline keyboard</see>,
    /// <see cref="ReplyKeyboardMarkup">custom reply keyboard</see>, instructions to
    /// <see cref="ReplyKeyboardRemove">remove reply keyboard</see> or to
    /// <see cref="ForceReplyMarkup">force a reply</see> from the user
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    /// <returns>On success, the stopped <see cref="Poll"/> with the final results is returned.</returns>
    public static async Task<Poll> StopPollAsync(
        this Message message,
        InlineKeyboardMarkup? replyMarkup = default,
        CancellationToken cancellationToken = default
    ) =>
        await message.ThrowIfNull(nameof(message))
            .FromCarrier()
            .MakeRequestAsync(
                request: new StopPollRequest(message.Chat.Id, message.MessageId)
                {
                    ReplyMarkup = replyMarkup
                },
                cancellationToken
            )
            .ConfigureAwait(false);

    /// <summary>
    /// Use this method to stop updating a live location message before
    /// <see cref="Location.LivePeriod"/> expires.
    /// </summary>
    /// <param name="message"></param>
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
    public static async Task<Message> StopLiveLocationAsync(
        this Message message,
        InlineKeyboardMarkup? replyMarkup = default,
        CancellationToken cancellationToken = default
    ) =>
        await message.ThrowIfNull(nameof(message))
            .FromCarrier()
            .MakeRequestAsync(
                request: new StopMessageLiveLocationRequest(message.ChatId, message.MessageId)
                {
                    ReplyMarkup = replyMarkup
                },
                cancellationToken
            )
            .ConfigureAwait(false);

    #endregion

    #region Others

    /// <summary>
    /// Use this method to forward this message. Service messages can't be forwarded.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="chatId">
    /// Unique identifier for the target chat or username of the target channel
    /// (in the format <c>@channelusername</c>)
    /// </param>
    /// <param name="disableNotification">
    /// Sends the message silently. Users will receive a notification with no sound
    /// </param>
    /// <param name="protectContent">Protects the contents of sent messages from forwarding and saving</param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    /// <returns>On success, the sent <see cref="Message"/> is returned.</returns>
    public static async Task<Message> ForwardAsync(
        this Message message,
        ChatId chatId,
        bool? disableNotification = default,
        bool? protectContent = default,
        CancellationToken cancellationToken = default
    ) =>
        await message.ThrowIfNull(nameof(message))
            .FromCarrier()
            .MakeRequestAsync(
                request: new ForwardMessageRequest(chatId, message.Chat.Id, message.MessageId)
                {
                    DisableNotification = disableNotification ?? message.FromCarrier().ClientDefaults.DisableNotification,
                    ProtectContent = protectContent ?? message.FromCarrier().ClientDefaults.ProtectContent,
                },
                cancellationToken
            )
            .ConfigureAwait(false);

    /// <summary>
    /// Use this method to copy this message. Service messages and invoice messages can't be copied.
    /// The method is analogous to the method <see cref="TelegramBotClientExtensions.ForwardMessageAsync"/>, but the copied message doesn't
    /// have a link to the original message.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="chatId">
    /// Unique identifier for the target chat or username of the target channel
    /// (in the format <c>@channelusername</c>)
    /// </param>
    /// <param name="caption">
    /// New caption for media, 0-1024 characters after entities parsing. If not specified, the original caption
    /// is kept
    /// </param>
    /// <param name="parseMode">
    /// Mode for parsing entities in the new caption. See
    /// <a href="https://core.telegram.org/bots/api#formatting-options">formatting</a> options for
    /// more details
    /// </param>
    /// <param name="captionEntities">
    /// List of special entities that appear in the caption, which can be specified instead
    /// of <see cref="ParseMode"/>
    /// </param>
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
    /// <see cref="ReplyKeyboardRemove">remove reply keyboard</see> or to
    /// <see cref="ForceReplyMarkup">force a reply</see> from the user
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    /// <returns>Returns the <see cref="MessageId"/> of the sent message on success.</returns>
    public static async Task<MessageId> CopyAsync(
        this Message message,
        ChatId chatId,
        string? caption = default,
        ParseMode? parseMode = default,
        IEnumerable<MessageEntity>? captionEntities = default,
        bool? disableNotification = default,
        bool? protectContent = default,
        int? replyToMessageId = default,
        bool? allowSendingWithoutReply = default,
        IReplyMarkup? replyMarkup = default,
        CancellationToken cancellationToken = default) =>
        await message.ThrowIfNull(nameof(message))
            .FromCarrier()
            .MakeRequestAsync(
                request: new CopyMessageRequest(chatId, message.Chat.Id, message.MessageId)
                {
                    Caption = caption,
                    ParseMode = parseMode ?? message.FromCarrier().ClientDefaults.ParseMode,
                    CaptionEntities = captionEntities,
                    ReplyToMessageId = replyToMessageId,
                    DisableNotification = disableNotification ?? message.FromCarrier().ClientDefaults.DisableNotification,
                    ProtectContent = protectContent ?? message.FromCarrier().ClientDefaults.ProtectContent,
                    AllowSendingWithoutReply = allowSendingWithoutReply ?? message.FromCarrier().ClientDefaults.AllowSendingWithoutReply,
                    ReplyMarkup = replyMarkup
                },
                cancellationToken
            )
            .ConfigureAwait(false);

    /// <summary>
    /// Use this method to add a message to the list of pinned messages in a chat. If the chat is not a private
    /// chat, the bot must be an administrator in the chat for this to work and must have the
    /// '<see cref="ChatPermissions.CanPinMessages"/>' admin right in a supergroup or
    /// '<see cref="ChatMemberAdministrator.CanEditMessages"/>' admin right in a channel
    /// </summary>
    /// <param name="message"></param>
    /// <param name="disableNotification">
    /// Pass <c><c>true</c></c>, if it is not necessary to send a notification to all chat members about
    /// the new pinned message. Notifications are always disabled in channels and private chats
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    public static async Task PinAsync(
        this Message message,
        bool? disableNotification = default,
        CancellationToken cancellationToken = default
    ) =>
        await message.ThrowIfNull(nameof(message))
            .FromCarrier()
            .MakeRequestAsync(
                request: new PinChatMessageRequest(message.Chat.Id, message.MessageId)
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
    /// <param name="message"></param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation
    /// </param>
    public static async Task UnpinAsync(
        this Message message,
        CancellationToken cancellationToken = default
    ) =>
        await message.ThrowIfNull(nameof(message))
            .FromCarrier()
            .MakeRequestAsync(
                request: new UnpinChatMessageRequest(message.Chat.Id) { MessageId = message.MessageId },
                cancellationToken
            )
            .ConfigureAwait(false);

    #endregion

    #endregion

    #region Helpers



    #endregion
}
