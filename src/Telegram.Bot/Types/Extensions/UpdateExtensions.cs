using System;
using System.Diagnostics.CodeAnalysis;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.Payments;

namespace Telegram.Bot.Types;

/// <summary>
/// A set of extension methods for <see cref="Update"/>.
/// </summary>
public static class UpdateExtensions
{
    #region Helpers

    /// <summary>
    /// Ensures original update inside <see cref="Update"/>.
    /// </summary>
    /// <typeparam name="T">Extracted original update from <see cref="Update"/>.</typeparam>
    /// <param name="update">The update itself.</param>
    /// <param name="resolver">A function to extract original update from <see cref="Update"/>.</param>
    /// <param name="extracted">Extracted update <typeparamref name="T"/>.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static bool EnsureOriginal<T>(
        this Update update, Func<Update, T?> resolver, [NotNullWhen(true)] out T? extracted)
    {
        if (update is null)
        {
            throw new ArgumentNullException(nameof(update));
        }

        if (resolver is null)
        {
            throw new ArgumentNullException(nameof(resolver));
        }

        extracted = resolver(update);
        if (extracted is null) return false;
        return true;
    }

    /// <summary>
    /// Ensures original update inside <see cref="Update"/>.
    /// </summary>
    /// <typeparam name="T">Extracted original update from <see cref="Update"/>.</typeparam>
    /// <param name="update">The update itself.</param>
    /// <param name="extracted">Extracted update <typeparamref name="T"/>.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static bool EnsureOriginal<T>(
        this Update update, [NotNullWhen(true)] out T? extracted)
    {
        if (update is null)
        {
            throw new ArgumentNullException(nameof(update));
        }

        extracted = (T?)update.OriginalUpdate;
        if (extracted is null) return false;
        return true;
    }

    /// <summary>
    /// Ensures the original update is a <see cref="Message"/> ( <see cref="UpdateType.Message"/> ).
    /// </summary>
    /// <param name="update">The update itself.</param>
    /// <param name="extracted">The extracted <see cref="Message"/>.</param>
    /// <returns></returns>
    public static bool EnsureMessage(this Update update, [NotNullWhen(true)] out Message? extracted)
        => update.EnsureOriginal(x => x.Message, out extracted);

    /// <summary>
    /// Ensures the original update is a <see cref="Message"/> ( <see cref="UpdateType.EditedMessage"/> ).
    /// </summary>
    /// <param name="update">The update itself.</param>
    /// <param name="extracted">The extracted <see cref="Message"/>.</param>
    /// <returns></returns>
    public static bool EnsureEditedMessage(this Update update, [NotNullWhen(true)] out Message? extracted)
        => update.EnsureOriginal(x => x.EditedMessage, out extracted);


    /// <summary>
    /// Ensures the original update is a <see cref="Message"/> ( <see cref="UpdateType.ChannelPost"/> ).
    /// </summary>
    /// <param name="update">The update itself.</param>
    /// <param name="extracted">The extracted <see cref="Message"/>.</param>
    /// <returns></returns>
    public static bool EnsureChannelPost(this Update update, [NotNullWhen(true)] out Message? extracted)
        => update.EnsureOriginal(x => x.ChannelPost, out extracted);


    /// <summary>
    /// Ensures the original update is a <see cref="Message"/> ( <see cref="UpdateType.EditedChannelPost"/> ).
    /// </summary>
    /// <param name="update">The update itself.</param>
    /// <param name="extracted">The extracted <see cref="Message"/>.</param>
    /// <returns></returns>
    public static bool EnsureEditedChannelPost(this Update update, [NotNullWhen(true)] out Message? extracted)
        => update.EnsureOriginal(x => x.EditedChannelPost, out extracted);


    /// <summary>
    /// Ensures the original update is a <see cref="CallbackQuery"/>.
    /// </summary>
    /// <param name="update">The update itself.</param>
    /// <param name="extracted">The extracted <see cref="CallbackQuery"/>.</param>
    /// <returns></returns>
    public static bool EnsureCallbackQuery(this Update update, [NotNullWhen(true)] out CallbackQuery? extracted)
        => update.EnsureOriginal(x => x.CallbackQuery, out extracted);

    /// <summary>
    /// Ensures the original update is a <see cref="InlineQuery"/>.
    /// </summary>
    /// <param name="update">The update itself.</param>
    /// <param name="extracted">The extracted <see cref="InlineQuery"/>.</param>
    /// <returns></returns>
    public static bool EnsureInlineQuery(this Update update, [NotNullWhen(true)] out InlineQuery? extracted)
        => update.EnsureOriginal(x => x.InlineQuery, out extracted);

    /// <summary>
    /// Ensures the original update is a <see cref="ChosenInlineResult"/>.
    /// </summary>
    /// <param name="update">The update itself.</param>
    /// <param name="extracted">The extracted <see cref="ChosenInlineResult"/>.</param>
    /// <returns></returns>
    public static bool EnsureChosenInlineResult(this Update update, [NotNullWhen(true)] out ChosenInlineResult? extracted)
        => update.EnsureOriginal(x => x.ChosenInlineResult, out extracted);

    /// <summary>
    /// Ensures the original update is a <see cref="Poll"/>.
    /// </summary>
    /// <param name="update">The update itself.</param>
    /// <param name="extracted">The extracted <see cref="Poll"/>.</param>
    /// <returns></returns>
    public static bool EnsurePoll(this Update update, [NotNullWhen(true)] out Poll? extracted)
        => update.EnsureOriginal(x => x.Poll, out extracted);

    /// <summary>
    /// Ensures the original update is a <see cref="PollAnswer"/>.
    /// </summary>
    /// <param name="update">The update itself.</param>
    /// <param name="extracted">The extracted <see cref="PollAnswer"/>.</param>
    /// <returns></returns>
    public static bool EnsurePollAnswer(this Update update, [NotNullWhen(true)] out PollAnswer? extracted)
        => update.EnsureOriginal(x => x.PollAnswer, out extracted);

    /// <summary>
    /// Ensures the original update is a <see cref="ChatJoinRequest"/>.
    /// </summary>
    /// <param name="update">The update itself.</param>
    /// <param name="extracted">The extracted <see cref="ChatJoinRequest"/>.</param>
    /// <returns></returns>
    public static bool EnsureChatJoinRequest(this Update update, [NotNullWhen(true)] out ChatJoinRequest? extracted)
        => update.EnsureOriginal(x => x.ChatJoinRequest, out extracted);

    /// <summary>
    /// Ensures the original update is a <see cref="PreCheckoutQuery"/>.
    /// </summary>
    /// <param name="update">The update itself.</param>
    /// <param name="extracted">The extracted <see cref="PreCheckoutQuery"/>.</param>
    /// <returns></returns>
    public static bool EnsurePreCheckoutQuery(this Update update, [NotNullWhen(true)] out PreCheckoutQuery? extracted)
        => update.EnsureOriginal(x => x.PreCheckoutQuery, out extracted);

    /// <summary>
    /// Ensures the original update is a <see cref="ShippingQuery"/>.
    /// </summary>
    /// <param name="update">The update itself.</param>
    /// <param name="extracted">The extracted <see cref="ShippingQuery"/>.</param>
    /// <returns></returns>
    public static bool EnsureShippingQuery(this Update update, [NotNullWhen(true)] out ShippingQuery? extracted)
        => update.EnsureOriginal(x => x.ShippingQuery, out extracted);

    /// <summary>
    /// Ensures the original update is a <see cref="ChatMemberUpdated"/> ( <see cref="UpdateType.MyChatMember"/> ).
    /// </summary>
    /// <param name="update">The update itself.</param>
    /// <param name="extracted">The extracted <see cref="ChatMemberUpdated"/>.</param>
    /// <returns></returns>
    public static bool EnsureMyChatMember(this Update update, [NotNullWhen(true)] out ChatMemberUpdated? extracted)
        => update.EnsureOriginal(x => x.MyChatMember, out extracted);

    /// <summary>
    /// Ensures the original update is a <see cref="ChatMemberUpdated"/> ( <see cref="UpdateType.ChatMember"/> ).
    /// </summary>
    /// <param name="update">The update itself.</param>
    /// <param name="extracted">The extracted <see cref="ChatMemberUpdated"/>.</param>
    /// <returns></returns>
    public static bool EnsureChatMember(this Update update, [NotNullWhen(true)] out ChatMemberUpdated? extracted)
        => update.EnsureOriginal(x => x.ChatMember, out extracted);

    #endregion
}
