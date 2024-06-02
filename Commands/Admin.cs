using Telegram.Bot;
using Telegram.Bot.Types;

namespace CyberPupsekBot.Commands
{
    internal class Admin
    {
        public static bool RIsDeletable = true;
        public static void AdminMessage(ITelegramBotClient bot, Update update)
        {
            if(update.Message.Text.StartsWith("/admes") && update.Message.From.Username == "Gekkooni")
            {
                var text = update.Message.Text.Split(' ');
                bot.DeleteMessageAsync(update.Message.Chat.Id, update.Message.MessageId);
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
                bot.SendTextMessageAsync(update.Message.Chat.Id, message);
            }
        }
        public async static void ChangeRStatus(Update update, ITelegramBotClient bot)
        {
            if(update.Message.Text == "@AllowRahat" && update.Message.From.Id == 820612424)
            {
                RIsDeletable = false;
                await bot.SendTextMessageAsync(update.Message.From.Id, "Рахат разрешен до перезапуска бота");
            }
            else if(update.Message.Text == "@KillRahat" && update.Message.From.Id == 820612424)
            {
                RIsDeletable = true;
            }
        }
    }
}
