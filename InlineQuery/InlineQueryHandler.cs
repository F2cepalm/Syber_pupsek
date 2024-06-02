using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineQueryResults;

namespace CyberPupsekBot.InlineQuery
{
    public class InlineQueryHandler : IDisposable
    {
        public ITelegramBotClient Client { get; set; }
        public Update Update { get; set; }
        public InlineQueryHandler(ITelegramBotClient client, Update update)
        {
            Client = client;
            Update = update;
        }

        public async void Handle()
        {
            var inlineQuery = Update.InlineQuery;

            var message = new InputTextMessageContent("<b>P U P S E K \n" +
                "Бот для вашей тусовки." + 
                "\n@syber_pupsek_bot</b>");
            message.ParseMode = Telegram.Bot.Types.Enums.ParseMode.Html;

            var result = new InlineQueryResultArticle(
                id: "1",
                title: "P U P S E K",
                inputMessageContent: message
            );

            var results = new List<InlineQueryResult>() 
            {
                result
            };


            await Client.AnswerInlineQueryAsync(inlineQuery.Id, results);

        }

        void IDisposable.Dispose()
        {
            
        }
    }
}
