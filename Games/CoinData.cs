using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
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
            if (upd.Message.Text.ToLower().StartsWith("/mycoins"))
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
                await bot.SendTextMessageAsync(upd.Message.Chat.Id, "@" + upd.Message.From.Username + $", у вас {coins} монет");
            }
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
            globalCoinList.First(e => e.UserId == id).Coins += coinDiff;
            if (globalCoinList.First(e => e.UserId == id).Coins < 0)
                globalCoinList.First(e => e.UserId == id).Coins = 0;
            globalCoinList.First(e => e.UserId == id).CoinsSpent += spentDiff;
            if (globalCoinList.First(e => e.UserId == id).CoinsSpent < 0)
                globalCoinList.First(e => e.UserId == id).CoinsSpent = 0;

            System.IO.File.WriteAllText("CoinInfo.json", JsonConvert.SerializeObject(globalCoinList));
            return globalCoinList;
        }
    }
}
