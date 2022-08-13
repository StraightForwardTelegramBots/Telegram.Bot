using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Types.Abstractions;

namespace Telegram.Bot.Types;

/// <summary>
/// Represents a join request sent to a chat.
/// </summary>
[JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
public class ChatJoinRequest : IClientCarrier
{
    ITelegramBotClient? IClientCarrier.Client { get; set; }

    void IClientCarrier.CustomSetter(ITelegramBotClient client)
    {
        (this as IClientCarrier).Client = client;
        From.CallCustomSetter(client);
        Chat.CallCustomSetter(client);
        InviteLink?.CallCustomSetter(client);
    }

    /// <summary>
    /// Chat to which the request was sent
    /// </summary>
    [JsonProperty(Required = Required.Always)]
    public Chat Chat { get; set; } = default!;

    /// <summary>
    /// User that sent the join request
    /// </summary>
    [JsonProperty(Required = Required.Always)]
    public User From { get; set; } = default!;

    /// <summary>
    /// Date the request was sent
    /// </summary>
    [JsonProperty(Required = Required.Always)]
    [JsonConverter(typeof(UnixDateTimeConverter))]
    public DateTime Date { get; set; }

    /// <summary>
    /// Optional. Bio of the user
    /// </summary>
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string? Bio { get; set; }

    /// <summary>
    /// Optional. Chat invite link that was used by the user to send the join request
    /// </summary>
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public ChatInviteLink? InviteLink { get; set; }
}
