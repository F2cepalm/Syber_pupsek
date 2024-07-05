using Newtonsoft.Json;
using System.Drawing;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace CyberPupsekBot.Games
{
    [Serializable]
    public class CoinData
    {
        public static List<CoinData>? globalCoinList = ReadFile();
        [JsonProperty]
        public int Coins;
        [JsonProperty]
        public long UserId;
        [JsonProperty]
        public int CoinsSpent;

        public CoinData(long Id, int coins)
        {
            UserId = Id;
            Coins = coins;
            CoinsSpent = 0;
        }
        internal static async Task IncCoins(long UserId, int count)
        {
            globalCoinList = ReadFile();
            if (globalCoinList.Contains(globalCoinList.First(e => e.UserId == UserId)))
            {
                globalCoinList = UpdateList(count, 0, UserId);
            }
            else
            {
                await WriteToList(new CoinData(UserId, count));
            }
        }
        internal static async Task DecCoins(long UserId, int count)
        {
            globalCoinList = ReadFile();
            if (globalCoinList.Contains(globalCoinList.First(e => e.UserId == UserId)))
            {
                globalCoinList = UpdateList(-count, 0, UserId);
            }
            else
            {
                await WriteToList(new CoinData(UserId, 0));
            }
        }
        internal static async Task GetAmount(Update upd, ITelegramBotClient bot)
        {
            if (upd.Message.Text.ToLower().StartsWith("/mycoins") || upd.Message.Text.ToLower().StartsWith("/coins"))
            {
                int coins;
                var dataCoin = ReadFile().FirstOrDefault(obj => obj.UserId == upd.Message.From.Id);
                if (dataCoin != null)
                {
                    coins = dataCoin.Coins;
                }
                else
                {
                    coins = 0;
                }
                if (upd.Message.From.Username != null)
                {
                    try
                    {
                        await bot.SendTextMessageAsync(upd.Message.Chat.Id, $"@{upd.Message.From.Username}, у вас {coins} монет");
                    }
                    catch(ApiRequestException e)
                    {
                        await bot.SendTextMessageAsync(upd.Message.Chat.Id, $"Начните диалог с ботом в личных сообщениях для корректной работы. \n\t@syber_pupsek_bot");
                        await bot.SendTextMessageAsync(upd.Message.Chat.Id, $"У вас {coins} монет");
                    }
                }
                else
                    await bot.SendTextMessageAsync(upd.Message.Chat.Id, "@" + upd.Message.From.Username + $", у вас {coins} монет");
            }
        }
        internal static async Task<int> GetAmount(Update upd)
        {
            int coins;
            CoinData dataCoin;
            try
            {
                dataCoin = ReadFile().First(obj => obj.UserId == upd.Message.From.Id);
            }
            catch (NullReferenceException e)
            {
                dataCoin = ReadFile().FirstOrDefault(obj => obj.UserId == upd.CallbackQuery.From.Id);
            }

            if (dataCoin != null)
            {
                coins = dataCoin.Coins;
            }
            else
            {
                coins = 0;
            }
            return coins;
        }

        internal static List<CoinData> ReadFile()
        {
            try
            {
                List<CoinData>? CoinDataString;
                if (!System.IO.File.Exists("CoinInfo.json"))
                {
                    System.IO.File.Create("CoinInfo.json");
                }

                var docString = System.IO.File.ReadAllText("CoinInfo.json");
                CoinDataString = JsonConvert.DeserializeObject<List<CoinData>>(docString);

                return CoinDataString;
            }
            catch (Exception ex)
            {
                return new List<CoinData>();
            }
        }

        internal async static Task<List<CoinData>> WriteToList(CoinData element)
        {
            globalCoinList = ReadFile();
            if (globalCoinList.Count > 0)
            {
                if (!globalCoinList.Contains(globalCoinList.FirstOrDefault(e => e.UserId == element.UserId)))
                {
                    globalCoinList.Add(element);
                    System.IO.File.WriteAllText("CoinInfo.json", JsonConvert.SerializeObject(globalCoinList));
                }
            }
            else
            {
                globalCoinList.Add(element);
                System.IO.File.WriteAllText("CoinInfo.json", JsonConvert.SerializeObject(globalCoinList));
            }

            return globalCoinList;
        }

        internal static List<CoinData> UpdateList(int coinDiff, int spentDiff, long id)
        {
            var gcI = globalCoinList.First(e => e.UserId == id);

            gcI.Coins += coinDiff;
            if (gcI.Coins < 0)
                gcI.Coins = 0;
            gcI.CoinsSpent += spentDiff;
            if (gcI.CoinsSpent < 0)
                gcI.CoinsSpent = 0;

            System.IO.File.WriteAllText("CoinInfo.json", JsonConvert.SerializeObject(globalCoinList));
            return globalCoinList;
        }
    }
}
