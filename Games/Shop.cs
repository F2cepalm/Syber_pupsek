using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace CyberPupsekBot.Games
{
    internal class Shop
    {
        internal static List<Product> products = new List<Product>()
        { 
            new Product("\"Я все\"", "Фраза смерти ", 15, 1),
            new Product("\"Может еще раз?\"","Фраза победы ", 30, 2),
            new Product("\"Умелый выстрел\"", "Фраза выстрела", 35, 3)
        }
        ;
        [JsonProperty]
        internal static Dictionary<long, List<int>> userProducts = Read().Result;

        internal static async Task ProcessMessage(Update upd, ITelegramBotClient bot)
        {
            if(upd.Message.Text.StartsWith("/shop"))
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var item in products)
                    {
                        sb.Append("1." + item.Name + " - " + item.Description + "\n" + $"Цена: {item.Price} монет" + "\n");
                    }
                    await bot.SendTextMessageAsync(upd.Message.From.Id, $"Магазин: \n\nУ вас {0} монет. \nОписание товаров:\n\n" + sb);
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
    }
}
