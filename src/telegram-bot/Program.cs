using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramBot;

public class Program
{
    public static async Task Main(string[] args)
    {
        // Endpoint to GET chatId from a channel who bot is inside = https://api.telegram.org/bot{token}/getUpdates

        // TelegramBotClient bot = new TelegramBotClient("");
        // Message message = await bot.SendTextMessageAsync("-1001777960814", "Olá, eu sou o bot do Wilson!");

        TelegramBotClient bot = new TelegramBotClient(Configuration.BotToken);

        User me = await bot.GetMeAsync();
        Console.Title = me.Username;

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        ReceiverOptions receiverOptions = new ReceiverOptions()
        {
            AllowedUpdates = Array.Empty<UpdateType>()
        };

        // StartReceiving does not block the caller thread
        // Receiving is done on the ThreadPool
        bot.StartReceiving(updateHandler: Handlers.HandleUpdateAsync,
                           errorHandler: Handlers.HandleErrorAsync,
                           receiverOptions: receiverOptions,
                           cancellationToken: cancellationTokenSource.Token);

        Console.WriteLine($"Start listening for @{me.Username}");
        Console.ReadLine();

        // Send cancellation request to stop bot
        cancellationTokenSource.Cancel();
    }
}
