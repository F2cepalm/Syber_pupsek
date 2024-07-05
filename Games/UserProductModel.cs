using Newtonsoft.Json;
using System.Linq;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CyberPupsekBot.Games
{
    internal class UserProductModel
    {
        [JsonProperty]
        public static List<UserProductModel> list = Read().Result;
        [JsonProperty]
        public long ChatId { get; set; }
        [JsonProperty]
        public bool HasInChat { get; set; }

        internal static async Task Write()
        {
            System.IO.File.WriteAllText("ShopUser.json", JsonConvert.SerializeObject(list));
        }
        internal static async Task<List<UserProductModel>> Read()
        {
            try
            {
                if (!System.IO.File.Exists("ShopUser.json"))
                { System.IO.File.Create("ShopUser.json"); }
                var str = JsonConvert.DeserializeObject<List<UserProductModel>>(System.IO.File.ReadAllText("ShopUser.json"));
                if(str == null)
                    return new List<UserProductModel> { };
                else 
                    { return str; }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\n\n\n\n\n\n File ShopUser has not found \n\n\n\n\n\n\n\n\n\n\n\n\n");
                return new List<UserProductModel> { };
            }
        }
        internal static async Task ChangeStatus(Update upd)
        {
            if(list.Count != 0)
            {
                if (upd.Type == UpdateType.Message)
                {
                    if (list.Contains(list.FirstOrDefault(e => e.ChatId == upd.Message.Chat.Id)))
                    {
                        if (list.FirstOrDefault(e => e.ChatId == upd.Message.Chat.Id).HasInChat == false)
                        {
                            list.FirstOrDefault(e => e.ChatId == upd.Message.Chat.Id).HasInChat = true;
                        }
                    }
                    else
                    {
                        list.Add(new UserProductModel { HasInChat = true, ChatId = upd.Message.Chat.Id });
                    }
                    await Write();
                }
                else if (upd.Type == UpdateType.CallbackQuery)
                {
                    if (list.Contains(list.FirstOrDefault(e => e.ChatId == upd.CallbackQuery.Message.Chat.Id)))
                    {
                        if (upd.CallbackQuery.Data == "shop_close")
                        {
                            list.FirstOrDefault(e => e.ChatId == upd.CallbackQuery.Message.Chat.Id).HasInChat = false;
                        }
                    }
                    else
                    {
                        list.Add(new UserProductModel { HasInChat = true, ChatId = upd.CallbackQuery.Message.Chat.Id });
                    }
                    await Write();
                }
            }
        }
    }
}
