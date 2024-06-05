using Telegram.Bot;
using Telegram.Bot.Types;
using System.Text.Json;
using System.Timers;
using CyberPupsekBot.Types;


namespace Bot
{
    class CallUsernames
    {
        public static Dictionary<long, UserTagsGroupSpecified> GroupSpecifiedUserList = UserTagsGroupSpecified.Read();
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
            if (Update.Message.Text == "/all@syber_pupsek_bot" || Update.Message.Text == "/all")
            {
                if (GroupSpecifiedUserList.ContainsKey(Update.Message.Chat.Id))
                {
                    if(GroupSpecifiedUserList[Update.Message.Chat.Id].UserTags.Count > 0)
                    {
                        foreach(var item in GroupSpecifiedUserList[Update.Message.Chat.Id].UserTags)
                        {
                            await Bot.SendTextMessageAsync(Update.Message.Chat.Id, "@" + item.ToString());
                        }
                    }
                }
            }
        }

        private async Task Callme()
        {
            if(Update.Message.Text.StartsWith("/callme"))
            {
                if(!GroupSpecifiedUserList.ContainsKey(Update.Message.Chat.Id))
                { 
                    GroupSpecifiedUserList.Add(Update.Message.Chat.Id, new UserTagsGroupSpecified { UserTags = new List<string> { Update.Message.From.Username } });
                    await Bot.SendTextMessageAsync(Update.Message.Chat.Id, "Вы добавлены в список перечисления тегов команды /all");
                }
                else
                {
                    if (!GroupSpecifiedUserList[Update.Message.Chat.Id].UserTags.Contains(Update.Message.From.Username))
                    {
                        GroupSpecifiedUserList[Update.Message.Chat.Id].UserTags.Add(Update.Message.Chat.Id.ToString());
                        await Bot.SendTextMessageAsync(Update.Message.Chat.Id, "Вы добавлены в список перечисления тегов команды /all");
                    }
                    else
                    {
                        await Bot.SendTextMessageAsync(Update.Message.Chat.Id, "Вы уже были добавлены \nв список перечисления тегов команды /all");
                    }
                }
                UserTagsGroupSpecified.Write(GroupSpecifiedUserList);
            }
        }
        private async Task Nocall()
        {
            if(Update.Message.Text.StartsWith("/nocall"))
            {
                if (GroupSpecifiedUserList.ContainsKey(Update.Message.Chat.Id))
                {
                    if (GroupSpecifiedUserList[Update.Message.Chat.Id].UserTags.Contains(Update.Message.From.Username))
                    {
                        GroupSpecifiedUserList[Update.Message.Chat.Id].UserTags.Remove(Update.Message.From.Username);
                        await Bot.SendTextMessageAsync(Update.Message.Chat.Id, "Вас больше не побеспокоят командой /all");
                    }
                    else
                    {
                        await Bot.SendTextMessageAsync(Update.Message.Chat.Id, "Вас итак нет в списке перечисления команды /all");
                    }
                }
                else
                {
                    await Bot.SendTextMessageAsync(Update.Message.Chat.Id, "Вас итак нет в списке перечисления команды /all");
                }
                UserTagsGroupSpecified.Write(GroupSpecifiedUserList);
            }
        }
    }
}