using Newtonsoft.Json;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace CyberPupsekBot.Games
{
    internal class Roulette
    {
        static readonly ITelegramBotClient bot = new TelegramBotClient("6038220012:AAFtvZFgGTRDsYTPX5wS7FAu4ANWGORIk4Y");
        [JsonProperty]
        static public List<GameModel>? gameList = ReadFile();
        static InlineKeyboardButton[][] keyboardBasic = new[]
{
                        new[] { new InlineKeyboardButton("btn1") { Text = "Присоединиться", CallbackData = "join_rgame" } },
                        new[] { new InlineKeyboardButton("btn2") { Text = "Выйти", CallbackData = "cancel_rgame" } }
                    };
        static InlineKeyboardButton[][] keyboardStart = new[]
                        { new[] { new InlineKeyboardButton("btn1") { Text = "Присоединиться", CallbackData = "join_rgame" } },
                          new[] { new InlineKeyboardButton("btn2") { Text = "Выйти", CallbackData = "cancel_rgame" } },
                          new[] { new InlineKeyboardButton("btn3") { Text = "Начать", CallbackData = "start_rgame"} }
                        };
        static InlineKeyboardButton[][] keyboardGame = new[]
        {
            new [] { new InlineKeyboardButton("btn1") { Text = "Выстрелить", CallbackData = "shoot_rgame"} }
        };

        static internal async Task Start(Update update, CancellationToken cts)
        {
            if(update.Message.Text.ToLower() == "/rgame" || update.Message.Text.ToLower() == "/rgame@syber_pupsek_bot")
            {
                if (!gameList.Any(gm => gm.GameId == update.Message.Chat.Id) || gameList.First(gm => gm.GameId == update.Message.Chat.Id).Status == "Finished")
                {
                    InlineKeyboardMarkup markup = new InlineKeyboardMarkup(keyboardBasic);
                    var message = await bot.SendTextMessageAsync(update.Message.Chat.Id, "Игра: Русская рулетка\n\nСоберите от 2 до 6 пользователей и начните игру. Каждый игрок по очереди нажимает на кнопку \"Выстрелить\" после чего появляется сообщение о том убил ли себя игрок или нет.\n\nЕсли один из игроков умирает, далее игра продолжается, но уже без убитого игрока. После того как все кроме одного игрока умерли, игра завершается.\n\nПобеждает тот кто не погиб. Удачи в игре!", replyMarkup: markup, cancellationToken: cts);
                    GameModel gm = new GameModel
                    {
                        RootMessageId = message.MessageId,
                        GameId = update.Message.Chat.Id,
                        Round = 1,
                        Bullets = Bullet.CreateBulletPack(6, 1),
                        generalRounds = 6,
                        UserNames = new List<string>() { update.Message.From.Username },
                        Ids = new List<long>() { update.Message.From.Id },
                        Status = "Joining"
                    };
                    gameList.Add(gm);
                    UpdateFile();

                    StringBuilder sb = new StringBuilder();
                    foreach (var item in gm.UserNames)
                    {
                        sb.Append("@" + item.ToString());
                        sb.Append('\n');
                    }
                    await bot.EditMessageTextAsync(update.Message.Chat.Id, message.MessageId, "Игра: Русская рулетка\n\nСоберите от 2 до 6 пользователей и начните игру. Каждый игрок по очереди нажимает на кнопку \"Выстрелить\" после чего появляется сообщение о том убил ли себя игрок или нет.\n\nЕсли один из игроков умирает, далее игра продолжается, но уже без убитого игрока. После того как все кроме одного игрока умерли, игра завершается.\n\nПобеждает тот кто не погиб. Удачи в игре!\n" + sb, replyMarkup: markup, cancellationToken: cts);
                }
                else
                {
                    var gameInstance = gameList.First(gm => gm.GameId == update.Message.Chat.Id);
                    try
                    {
                        await bot.EditMessageReplyMarkupAsync(gameInstance.GameId, gameInstance.RootMessageId);
                        await bot.SendTextMessageAsync(update.Message.Chat.Id, "Игра была закончена.\nЧтобы начать новую введите команду /rgame");
                    }
                    catch (Exception ex) { }
                    try
                    {
                        gameList.Remove(gameList.FirstOrDefault(gm => gm.GameId == update.Message.Chat.Id));
                        System.IO.File.WriteAllText("GameFile.json", JsonConvert.SerializeObject(gameList));
                    }
                    catch { }
                }
            }
        }

        static internal async Task UpdateGameEventJoin(GameModel gm, Update upd)
        {
            if(gm.UserNames is not null)
            {
                if (gm.UserNames.Count < 6)
                {
                    if (!gm.UserNames.Contains(upd.CallbackQuery.From.Username))
                    {
                        gm.UserNames.Add(upd.CallbackQuery.From.Username);
                        gm.Ids.Add(upd.CallbackQuery.From.Id);
                        gameList.First(gm => gm.GameId == upd.CallbackQuery.Message.Chat.Id).UserNames = gm.UserNames;
                        gameList.First(gm => gm.GameId == upd.CallbackQuery.Message.Chat.Id).Ids = gm.Ids;
                        UpdateFile();
                    }
                }
                if (gm.UserNames.Count > 1)
                {
                    try
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (var item in gm.UserNames)
                        {
                            sb.Append("@" + item.ToString());
                            sb.Append('\n');
                        }
                        await bot.EditMessageTextAsync(upd.CallbackQuery.Message.Chat.Id, gm.RootMessageId, "Игра: Русская рулетка\n\nСоберите от 2 до 6 пользователей и начните игру. Каждый игрок по очереди нажимает на кнопку \"Выстрелить\" после чего появляется сообщение о том убил ли себя игрок или нет.\n\nЕсли один из игроков умирает, далее игра продолжается, но уже без убитого игрока. После того как все кроме одного игрока умерли, игра завершается.\n\nПобеждает тот кто не погиб. Удачи в игре!\n\nСписок игроков: \n" + sb, replyMarkup: keyboardStart);
                        await bot.EditMessageReplyMarkupAsync(upd.CallbackQuery.Message.Chat.Id, gm.RootMessageId, keyboardStart);
                    }
                    catch (Exception ex) { }
                }
            }
        }

        static internal async Task UpdateGameEventCancel(GameModel gm, Update upd)
        {
            if(gm is not null)
            {
                if (gm.UserNames is not null)
                {
                    gm.UserNames.Remove(upd.CallbackQuery.From.Username);
                    gm.Ids.Remove(upd.CallbackQuery.From.Id);
                    gameList.First(gm => gm.GameId == upd.CallbackQuery.Message.Chat.Id).UserNames = gm.UserNames;
                    gameList.First(gm => gm.GameId == upd.CallbackQuery.Message.Chat.Id).Ids = gm.Ids;

                    StringBuilder sb = new StringBuilder();
                    foreach (var item in gm.UserNames)
                    {
                        sb.Append( "@" + item.ToString());
                        sb.Append('\n');
                    }
                    try
                    {
                        await bot.EditMessageTextAsync(upd.CallbackQuery.Message.Chat.Id, gm.RootMessageId, "Игра: Русская рулетка\nПравила: Каждый игрок по очереди нажимает на кнопку \"Выстрелить\". Раунд длится до первой смерти. Далее игра продолжается, но уже без убитого игрока. Игра продолжается до того как умрут все, кроме одного." + "\n" + "Список игроков:" + "\n" + sb, replyMarkup: keyboardBasic);
                    }
                    catch (Exception ex) { }

                    if (gm.UserNames.Count == 0)
                    {
                        try
                        {
                            await bot.DeleteMessageAsync(upd.CallbackQuery.Message.Chat.Id, gm.RootMessageId);
                            await bot.SendTextMessageAsync(upd.CallbackQuery.Message.Chat.Id, "Игра отменена");
                        }
                        catch (Exception ex) { }
                        gameList.Remove(gameList.First(gm => gm.GameId == upd.CallbackQuery.Message.Chat.Id));
                    }
                    UpdateFile();
                }
                else
                {
                    gameList.Remove(gameList.FirstOrDefault(e => e.GameId == upd.CallbackQuery.Message.Chat.Id));
                }
            }
        }

        static internal List<GameModel> ReadFile() //достает список из файла
        {
            try
            {
                var lines = System.IO.File.ReadAllText("GameFile.json");
                var list = JsonConvert.DeserializeObject<List<GameModel>>(lines);
                if(list != null)
                    return list;
                else
                    return new List<GameModel> { };
            }
            catch (Exception ex) { return new List<GameModel>(); }
        }

        static internal void UpdateFile()
        {
            var json = JsonConvert.SerializeObject(gameList);
            System.IO.File.WriteAllText("GameFile.json", json);
        }

        static internal async Task UpdateGameEventStart(GameModel gm, Update upd, CancellationToken cts) //старт игры
        {
            await bot.SendTextMessageAsync(gm.GameId, "Заряжайте револьвер!");
            await bot.EditMessageReplyMarkupAsync(gm.GameId, gm.RootMessageId);

            Random rnd = new Random();
            gm.UserNames = gm.UserNames.OrderBy(x => rnd.Next()).ToList();
            var gmI = gameList.First(gm => gm.GameId == upd.CallbackQuery.Message.Chat.Id);

            gmI.UserNames = gm.UserNames;
            gmI.TurnUsername = gm.UserNames[gm.indexUserNames];
            gmI.Status = "Game";
            
            InlineKeyboardMarkup markup = new InlineKeyboardMarkup(keyboardGame);
            StringBuilder sb = new StringBuilder();
            var mes = await bot.SendTextMessageAsync(gm.GameId, "Очередь игрока: @" + gm.TurnUsername, replyMarkup: markup, cancellationToken: cts);

            int coinAmount = 1;
            int coinLoss = 1;
            switch (gmI.Ids.Count)
            {
                case 1: coinAmount = 1; coinLoss = 1; break;
                case 2: coinAmount = 2; coinLoss = 1; break;
                case 3: coinAmount = 3; coinLoss = 1; break;
                case 4: coinAmount = 4; coinLoss = 2; break;
                case 5: coinAmount = 5; coinLoss = 2; break;
                case 6: coinAmount = 6; coinLoss = 3; break;
            }
            gmI.AppendCoins = coinAmount;
            gmI.RemoveCoins = coinLoss;

            gmI.RootMessageId = mes.MessageId;
            UpdateFile();
        }

        static internal async Task TryShooting(GameModel gm, Update upd, CancellationToken cts)
        {
            if (gm.Ids.Count > 0)
            {
                var gmI = gameList.First(gm => gm.GameId == upd.CallbackQuery.Message.Chat.Id);

                if (upd.CallbackQuery.From.Username == gmI.TurnUsername)
                {
                    try
                    {
                        await bot.EditMessageReplyMarkupAsync(gmI.GameId, gmI.RootMessageId);
                    }
                    catch (Exception ex) { }
                    if (gm.Bullets[gm.indexBullets].IsReal == false)
                    {
                        Thread.Sleep(2000);
                        await bot.SendTextMessageAsync(upd.CallbackQuery.Message.Chat.Id, "Патрон холостой");
                    }
                    else
                    {
                        Thread.Sleep(2000);
                        await bot.SendTextMessageAsync(upd.CallbackQuery.Message.Chat.Id, "Патрон реальный\n\nИгрок " + "@" + upd.CallbackQuery.From.Username + $" погиб\n-{gmI.RemoveCoins} монеты");
                        await CoinData.DecCoins(upd.CallbackQuery.From.Id, gmI.RemoveCoins);
                        gmI.UserNames.Remove(upd.CallbackQuery.From.Username);
                        gmI.Ids.Remove(upd.CallbackQuery.From.Id);
                        UpdateFile();
                    }

                    if (gmI.UserNames.Count == 1)
                    {
                        Thread.Sleep(2000);
                        var mes = await bot.SendTextMessageAsync(upd.CallbackQuery.Message.Chat.Id, "Игра окончена" + "\n" + "Победил: " + "@" + gmI.UserNames[0] + "\n" + $"Начислено: {gmI.AppendCoins} монет");
                        await CoinData.IncCoins(gmI.Ids[0], gmI.AppendCoins);
                        gameList.Remove(gmI);
                    }
                    else
                    {
                        gmI = gameList.First(gm => gm.GameId == upd.CallbackQuery.Message.Chat.Id);

                        gmI.indexUserNames++;
                        if (gmI.indexUserNames >= gmI.UserNames.Count())
                        { gmI.indexUserNames = 0; }
                        gmI.TurnUsername = gmI.UserNames[gmI.indexUserNames];
                        gmI.indexBullets++;
                        if (gmI.indexBullets >= gmI.Bullets.Count())
                        {
                            gmI.indexBullets = 0;
                            gmI.Bullets = Bullet.CreateBulletPack(6, 1);
                            await bot.SendTextMessageAsync(upd.CallbackQuery.Message.Chat.Id, "Перезаряжаемся...");
                            Thread.Sleep(5000);
                            await bot.SendTextMessageAsync(upd.CallbackQuery.Message.Chat.Id, "Револьвер перезаряжен, \n\nпродолжайте стрелять...");
                            Thread.Sleep(2000);
                        }

                        InlineKeyboardMarkup markup = new InlineKeyboardMarkup(keyboardGame);
                        var mes = await bot.SendTextMessageAsync(gm.GameId, "Очередь игрока: @" + gmI.TurnUsername, replyMarkup: markup, cancellationToken: cts);
                        gmI.RootMessageId = mes.MessageId;
                    }
                    UpdateFile();
                }
            }
            else
            {
                await bot.SendTextMessageAsync(gm.GameId, "Похоже все умерли, жалко", cancellationToken: cts);
                gameList.Remove(gameList.First(gm => gm.GameId == upd.CallbackQuery.Message.Chat.Id));
                UpdateFile();
            }
        }
    }
}
