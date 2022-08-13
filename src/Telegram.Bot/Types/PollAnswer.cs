using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Types.Abstractions;

namespace Telegram.Bot.Types;

/// <summary>
/// This object represents an answer of a user in a non-anonymous poll.
/// </summary>
[JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
public class PollAnswer : IClientCarrier
{
    ITelegramBotClient? IClientCarrier.Client { get; set; }

    void IClientCarrier.CustomSetter(ITelegramBotClient client)
    {
        (this as IClientCarrier).Client = client;
        User.CallCustomSetter(client);
    }

    /// <summary>
    /// Unique poll identifier
    /// </summary>
    [JsonProperty(Required = Required.Always)]
    public string PollId { get; set; } = default!;

    /// <summary>
    /// The user, who changed the answer to the poll
    /// </summary>
    [JsonProperty(Required = Required.Always)]
    public User User { get; set; } = default!;

    /// <summary>
    /// 0-based identifiers of answer options, chosen by the user. May be empty if the user retracted their vote.
    /// </summary>
    [JsonProperty(Required = Required.Always)]
    public int[] OptionIds { get; set; } = default!;
}
