using Telegram.Bot;
using Telegram.Bot.Types;


namespace Bot
{
    class Randomizer
    {
        private ITelegramBotClient Bot { get; set; }
        private Update Update { get; set; }
        public Randomizer(ITelegramBotClient bot, Update update)
        {
            Bot = bot;
            Update = update;
        }
        internal async Task Process()
        {
            await GetNumber();
            await Roll();
        }
        async public Task GetNumber()
        {
            try
            {
                if (Update.Message.Text == "/number" || Update.Message.Text == "/number@syber_pupsek_bot")
                {
                    Random rand = new Random();
                    int number = rand.Next(0, 101);

                    await Bot.SendTextMessageAsync(Update.Message.Chat.Id, $"Число {number}");
                }
            }
            catch (NullReferenceException) { }
        }
        async public Task Roll()
        {
            if (Update.Message.Text.StartsWith("/roll"))
            {
                Random rn = new Random();
                int randomNumber = rn.Next(0, 121);

                if (randomNumber > 0 && randomNumber < 10)
                    await SendMessageText("Ваш вопрос требует времени.");
                else if (randomNumber > 10 && randomNumber < 20)
                    await SendMessageText("Подтверждено на 100%.");
                else if (randomNumber < 30 && randomNumber > 20)
                    await SendMessageText("Как вы посчитаете нужным, таков и ответ.");
                else if (randomNumber > 30 && randomNumber < 40)
                    await SendMessageText("Я указываю на ваше заблуждение.");
                else if (randomNumber > 40 && randomNumber < 70)
                    await SendMessageText("Я считаю, что ответ на ваш вопрос это \"Да\".");
                else if (randomNumber > 70 && randomNumber < 81)
                    await SendMessageText("Я считаю, что ответ на ваш вопрос это \"Нет\".");
                else if (randomNumber > 81 && randomNumber < 101)
                    await SendMessageText("Я считаю, что это неоспоримая правда.");
                else if (randomNumber > 101 && randomNumber < 121)
                    await SendMessageText("Извините, я бессилен в данных вопросах.");
                else
                    await SendMessageText("А верите ли вы? От вашего честного мнения зависит ответ.");
            }
        }

        async internal Task SendMessageText(string message)
        {
            await Bot.SendTextMessageAsync(Update.Message.Chat.Id, message);
        }
    }
}
