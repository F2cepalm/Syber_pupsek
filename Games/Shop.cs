using Newtonsoft.Json;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace CyberPupsekBot.Games
{
    internal class Shop
    {
        static InlineKeyboardButton[][] keyboardBasic = new[]
{
                        new[] { new InlineKeyboardButton("btn11") { Text = "Игры", CallbackData = "shop_games" } },
                        new[] { new InlineKeyboardButton("btn12") { Text = "Предметы", CallbackData = "shop_prod" } },
                        new[] { new InlineKeyboardButton("btn20") { Text = "Закрыть", CallbackData = "shop_close" } }
                    };
        static InlineKeyboardButton[][] keyboardGames = new[]
{
                        new[] { new InlineKeyboardButton("btn13") { Text = "<<< Назад", CallbackData = "shop_start" } }
                    };
        static InlineKeyboardButton[][] keyboardProds = new[]
{
                        new[] { new InlineKeyboardButton("btn15") { Text = "Рулетка", CallbackData = "shop_roulette" } },
                        new[] { new InlineKeyboardButton("btn13") { Text = "<<< Назад", CallbackData = "shop_start" } }
                    };
        static InlineKeyboardButton[][] keyboardRouletteType = new[]
{
                        new[] { new InlineKeyboardButton("btn17") { Text = "Фразы смерти", CallbackData = "shop_roulette_deathMessages" } },
                        new[] { new InlineKeyboardButton("btn18") { Text = "Фразы выстрела", CallbackData = "shop_roulette_shootMessages" } },
                        new[] { new InlineKeyboardButton("btn19") { Text = "Фразы победы", CallbackData = "shop_roulette_wictoryMessages" } },
                        new[] { new InlineKeyboardButton("btn13") { Text = "<<< Назад", CallbackData = "shop_prod" } }
                    };

        [JsonProperty]
        internal static Dictionary<long, List<int>> userProducts = Read().Result;

        internal static async Task ProcessMessage(Update upd, ITelegramBotClient bot)
        {
            if(upd.Message.Text.StartsWith("/shop"))
            {
                try
                {
                    await UserProductModel.ChangeStatus(upd);
                    InlineKeyboardMarkup markup = new InlineKeyboardMarkup(keyboardBasic);
                    await bot.SendTextMessageAsync(upd.Message.From.Id, $"Магазин: \n\nУ вас {CoinData.GetAmount(upd).Result.ToString()} монет. \nЧто вы хотели бы приобрести?", replyMarkup:markup);
                }
                catch (ApiRequestException ex)
                {
                    if(upd.Message.From.Username != null || upd.Message.From.Username != "")
                    {
                        await bot.SendTextMessageAsync(upd.Message.Chat.Id, upd.Message.From.Username + ", начните диалог с ботом в личных сообщениях для использования данной команды.");
                    }
                    else
                    {
                        await bot.SendTextMessageAsync(upd.Message.Chat.Id, "Начните диалог с ботом в личных сообщениях для использования данной команды.");
                    }
                }
            }
        }
        internal static async Task ProcessCallback(Update upd, ITelegramBotClient bot)
        {
            InlineKeyboardMarkup markup = new InlineKeyboardMarkup(keyboardBasic);

            switch (upd.CallbackQuery.Data)
            {
                case "shop_start":
                    await bot.SendTextMessageAsync(upd.CallbackQuery.From.Id, $"Магазин: \n\nУ вас {CoinData.GetAmount(upd).Result.ToString()} монет. \nЧто вы хотели бы приобрести?", replyMarkup: markup);
                    break;
                case "shop_games":
                    markup = new InlineKeyboardMarkup(keyboardGames);
                    await bot.SendTextMessageAsync(upd.CallbackQuery.From.Id, $"Магазин: \n\nУ вас {CoinData.GetAmount(upd).Result.ToString()} монет. \nВыберите игру из списка для просмотра описания", replyMarkup: markup);
                    break;
                case "shop_prod":
                    markup = new InlineKeyboardMarkup(keyboardProds);
                    await bot.SendTextMessageAsync(upd.CallbackQuery.From.Id, $"Магазин: \n\nУ вас {CoinData.GetAmount(upd).Result.ToString()} монет. \nВыберите игру из списка для просмотра предметов, относящихся к ней", replyMarkup: markup);
                    break;
                case "shop_roulette":
                    markup = new InlineKeyboardMarkup(keyboardRouletteType);
                    await bot.SendTextMessageAsync(upd.CallbackQuery.From.Id, $"Магазин: \n\nУ вас {CoinData.GetAmount(upd).Result.ToString()} монет. \nВыберите вид предмета из списка для просмотра описания", replyMarkup: markup);
                    break;
                case "shop_close":
                    await UserProductModel.ChangeStatus(upd);
                    break;
                default:
                    break;
            }
        }

        internal static async Task<Dictionary<long, List<int>>> Read()
        {
            if(!System.IO.File.Exists("UserProduct.json"))
            { System.IO.File.Create("UserProduct.json"); return new Dictionary<long, List<int>>(); }

            Dictionary<long, List<int>> res = JsonConvert.DeserializeObject<Dictionary<long, List<int>>>(System.IO.File.ReadAllText("UserProduct.json"));
            if(res != null)
                return res;
            else
                return new Dictionary<long, List<int>>();
        }
        internal static async Task Write()
        {
            if (!System.IO.File.Exists("UserProduct.json"))
            { System.IO.File.Create("UserProduct.json"); }

            System.IO.File.WriteAllText("UserProduct.json", JsonConvert.SerializeObject(userProducts));
        }
        internal static async Task Buy()
        {

        }
    }
}
