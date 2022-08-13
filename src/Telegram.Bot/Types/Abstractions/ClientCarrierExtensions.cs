using System.Linq;

namespace Telegram.Bot.Types.Abstractions
{
    internal static class ClientCarrierExtensions
    {
        internal static void CallCustomSetter(
            this object? obj, ITelegramBotClient client)
        {
            if (obj is IClientCarrier carrier)
            {
                carrier.CustomSetter(client);
            }
        }
    }
}
