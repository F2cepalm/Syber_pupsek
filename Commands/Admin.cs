using Telegram.Bot;
using Telegram.Bot.Types;

namespace CyberPupsekBot.Commands
{
    internal class Admin
    {
        public static async Task AdminMessage(ITelegramBotClient bot, Update update)
        {
            if(update.Message.Text.StartsWith("/admes") && update.Message.From.Username == "Gekkooni")
            {
                var text = update.Message.Text.Split(' ');
                await bot.DeleteMessageAsync(update.Message.Chat.Id, update.Message.MessageId);
                text[0] = "";
                string message = "";
                foreach(var line in text)
                {
                    if (line is "\\n")
                        message += "\n";
                    else
                        message += line;
                    message += " ";
                }
                await bot.SendTextMessageAsync(update.Message.Chat.Id, message);
            }
        }
    }
}
