using Telegram.Bot;
using Telegram.Bot.Types;
using System.Text.Json;
using System.Timers;


namespace Bot
{
    class CallUsernames
    {
        private ITelegramBotClient Bot {  get; set; }
        private Update Update { get; set; }
        public CallUsernames(ITelegramBotClient bot, Update update)
        {
            Bot = bot;
            Update = update;
        }
        internal async Task Process()
        {
            await Send();
            await Callme();
            await Nocall();
        }
        async private Task Send()
        {
            string[]? tags = new string[0];

            if (Update.Message.Text == "/all@syber_pupsek_bot" || Update.Message.Text == "/all")
            {
                try
                {
                    tags = JsonSerializer.Deserialize<string[]>(System.IO.File.ReadAllText("Saves.json"));
                }
                catch (Exception ex) { await Console.Out.WriteLineAsync(ex.ToString()); }

                foreach (var item in tags)
                {
                    await Bot.SendTextMessageAsync(Update.Message.Chat.Id, "@" + item);
                }
                await Bot.DeleteMessageAsync(Update.Message.Chat.Id, Update.Message.MessageId);
            }
        }

        private async Task Callme()
        {
            if(Update.Message.Text.StartsWith("/callme"))
            {
                List<string> names = new List<string>();
                try
                {
                    names = JsonSerializer.Deserialize<List<string>>(System.IO.File.ReadAllText("Saves.json"));
                }
                catch { }
                if (!names.Contains(Update.Message.From.Username))
                {
                    names.Add(Update.Message.From.Username);
                    System.IO.File.WriteAllText("Saves.json", JsonSerializer.Serialize(names));
                }

            }
        }
        private async Task Nocall()
        {
            if(Update.Message.Text.StartsWith("/nocall"))
            {
                List<string> names = new List<string>();
                try
                {
                    names = JsonSerializer.Deserialize<List<string>>(System.IO.File.ReadAllText("Saves.json"));
                }
                catch { }
                names.Remove(Update.Message.From.Username);
                System.IO.File.WriteAllText("Saves.json", JsonSerializer.Serialize(names));
            }
        }
    }
}