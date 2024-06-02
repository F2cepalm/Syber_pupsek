using System.Diagnostics.Metrics;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Bot
{
    class TextExtensionCommands
    {
        private ITelegramBotClient Bot { get; set; }
        private Update Update { get; set; }
        public TextExtensionCommands(ITelegramBotClient bot, Update update)
        {
            Bot = bot;
            Update = update;
        }
        internal async Task Process()
        {
            Kick();
        }
        async public Task Kick()
        {
            string text = Update.Message.Text;
             if (text.StartsWith("/kick"))
             {
                var textSplitted = text.Split(' ');
                if(textSplitted.Length > 1 )
                {
                    string message = "";
                    for (global::System.Int32 i = 1; i < textSplitted.Length; i++)
                    {
                        message += textSplitted[i];
                        message += " ";
                    }
                    await Bot.SendTextMessageAsync(Update.Message.Chat.Id, "@" + Update.Message.From.Username + " жестко ударил по жопе " + new string(message));
                }
                else
                {
                    await Bot.SendTextMessageAsync(Update.Message.Chat.Id, "@" + Update.Message.From.Username + " промахнулся по чьей-то жопе");
                }
                await Bot.DeleteMessageAsync(Update.Message.Chat.Id, Update.Message.MessageId);
            }
        }
    }
}
