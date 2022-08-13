namespace Telegram.Bot.Types.Abstractions;

internal interface IClientCarrier
{
    internal ITelegramBotClient? Client { get; set; }

    internal void CustomSetter(ITelegramBotClient client);
}

