using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace CyberPupsekBot.Functions
{
    internal class AnonymousMessage
    {
        internal async static Task Send(ITelegramBotClient bot, Update update)
        {
            if(update.Message.Text.StartsWith("/an"))
            {
                var text = update.Message.Text.Split(' ');
                try
                {
                    await bot.DeleteMessageAsync(update.Message.Chat.Id, update.Message.MessageId);
                }
                catch (ApiRequestException e)
                {
                    
                }
                text[0] = "";
                string message = string.Join(" ", text);

                await bot.SendTextMessageAsync(update.Message.Chat.Id, "Анонимное сообщение: " + message);
            }
        }
    }
}
