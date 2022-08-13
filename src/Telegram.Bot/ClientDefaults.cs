using Telegram.Bot.Types.Enums;

namespace Telegram.Bot;

/// <summary>
/// Stores some default values for common stuff related to the client.
/// </summary>
public struct ClientDefaults
{
    public ClientDefaults(
        ParseMode? parseMode = default,
        bool? disableWebPagePreview = default,
        bool? disableNotification = default,
        bool? protectContent = default,
        bool? allowSendingWithoutReply = default)
    {
        ParseMode = parseMode;
        DisableWebPagePreview = disableWebPagePreview;
        DisableNotification = disableNotification;
        ProtectContent = protectContent;
        AllowSendingWithoutReply = allowSendingWithoutReply;
    }

    /// <summary>
    /// Default ParseMode to be used where ever it's not specified.
    /// </summary>
    public ParseMode? ParseMode { get; set; }

    /// <summary>
    /// Default DisableWebPagePreview to be used where ever it's not specified.
    /// </summary>
    public bool? DisableWebPagePreview { get; set; }

    /// <summary>
    /// Default DisableNotification to be used where ever it's not specified.
    /// </summary>
    public bool? DisableNotification { get; set; }

    /// <summary>
    /// Default ProtectContent to be used where ever it's not specified.
    /// </summary>
    public bool? ProtectContent { get; set; }

    /// <summary>
    /// Default AllowSendingWithoutReply to be used where ever it's not specified.
    /// </summary>
    public bool? AllowSendingWithoutReply { get; set; }
}
