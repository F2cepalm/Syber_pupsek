using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using CyberPupsekBot.Functions;
using CyberPupsekBot.Commands;
using CyberPupsekBot.InlineQuery;
using CyberPupsekBot.Games;
using System.Text;
using System.Diagnostics;
using System.Reflection;

namespace Bot
{

    class MainBot
    {
        internal static ITelegramBotClient bot = new TelegramBotClient("6038220012:AAFtvZFgGTRDsYTPX5wS7FAu4ANWGORIk4Y");
        static bool timerExpired = true;
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
                await CheckDateAndSendMessage(bot);
                if (update.Type == Telegram.Bot.Types.Enums.UpdateType.InlineQuery)
                {
                    using (InlineQueryHandler handler = new InlineQueryHandler(botClient, update))
                    {
                        handler.Handle();
                    }
                }
                else if (update.Type == Telegram.Bot.Types.Enums.UpdateType.CallbackQuery)
                {
                    Roulette.gameList = Roulette.ReadFile();
                    if (update.CallbackQuery.Data.StartsWith("shoot_rgame"))
                    {
                        await Roulette.TryShooting(Roulette.gameList.First(gm => update.CallbackQuery.Message.Chat.Id == gm.GameId), update, cancellationToken);
                    }
                    else if (update.CallbackQuery.Data.StartsWith("join_rgame"))
                    {
                        await Roulette.UpdateGameEventJoin(Roulette.gameList.FirstOrDefault(gm => update.CallbackQuery.Message.Chat.Id == gm.GameId), update);
                    }
                    else if (update.CallbackQuery.Data == "cancel_rgame")
                    {
                        await Roulette.UpdateGameEventCancel(Roulette.gameList.FirstOrDefault(gm => update.CallbackQuery.Message.Chat.Id == gm.GameId), update);
                    }
                    else if (update.CallbackQuery.Data == "start_rgame")
                    {
                        await Roulette.UpdateGameEventStart(Roulette.gameList.FirstOrDefault(gm => update.CallbackQuery.Message.Chat.Id == gm.GameId), update, cancellationToken);
                    }


                    Console.WriteLine("From: " + update.CallbackQuery.From.Username + " \n Data: " + update.CallbackQuery.Data + " \n Type: " + update.Type + " \n ID: " + update.CallbackQuery.From.Id + "\n ------------------------------------------------------------------------------------------------------");
                }
                else if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
                {
                    if (update.Message.MigrateToChatId != 0)
                    {
                        if (update.Message.From.Id == 815655901 && Admin.RIsDeletable == true)
                        {
                            await botClient.BanChatMemberAsync(-1001729685476, 815655901);
                            await botClient.SendTextMessageAsync(-1001729685476, "Пока, " + update.Message.From.Username);
                        }

                        if (update.Message.Text is not null)
                        {
                            await CoinData.WriteToList(new CoinData(update.Message.From.Id, 0));
                            Admin.ChangeRStatus(update, bot);
                            if (update.Message.Text.StartsWith('/'))
                            {
                                await CoinData.GetAmount(update, bot);
                                HelpCommand.Send(botClient, update);
                                await Roulette.Start(update, cancellationToken);
                                CallUsernames cu = new CallUsernames(bot, update);
                                Randomizer rm = new Randomizer(bot, update);
                                TextExtensionCommands tec = new TextExtensionCommands(botClient, update); //заменить типы void в Task в методах и реструктурировать вызовы

                                AnonymousMessage.Send(botClient, update);
                                await tec.Process();
                                await cu.Process();
                                await rm.Process();
                                Admin.AdminMessage(bot, update);
                            }
                        }
                    }
                }

                var message = update.Message;
                if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
                    Console.WriteLine("From: " + message.From.Username + " \n Message: " + message.Text + " \n Type: " + message.Type + " \n ID: " + message.From.Id + "\n ------------------------------------------------------------------------------------------------------");
        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            StringBuilder sb = new StringBuilder();
            await Console.Out.WriteLineAsync(sb.Append(exception));
        }

        static void Main()
        {
            Console.WriteLine("Запущен " + bot.GetMeAsync().Result.FirstName + " v 0.4.2 Beta : /rgame - coins - patch");

            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }, 
            };
            bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );

/*            RestartBot();*/
            Console.ReadLine();
        }
        static async Task CheckDateAndSendMessage(ITelegramBotClient botClient)
        {
            if(timerExpired == true)
            {
                DateTime currentDate = DateTime.Now;
                DateTime Alikh = new DateTime(2024, 8, 16); //должен создаваться только 1 объект класса и меняться значения
                DateTime Zhanik = new DateTime(2024, 7, 6);
                DateTime Hamza = new DateTime(2024, 7, 1);
                DateTime Vova = new DateTime(2024, 6, 14);
                DateTime Duman = new DateTime(2024, 6, 22);
                DateTime Nurba = new DateTime(2024, 3, 19);
                DateTime Raha = new DateTime(2024, 8, 17);
                DateTime Dima = new DateTime(2024, 11, 25);
                DateTime Era = new DateTime(2024, 5, 25);
                DateTime Artem = new DateTime(2024, 11, 21);

                if (currentDate.Date.Month == Alikh.Date.Month && currentDate.Date.Day == Alikh.Date.Day)
                {
                    await botClient.SendTextMessageAsync(-1001729685476, "С ДНЮХОЙ АЛИХАНЧИК!!!!!!!!!!!!!");
                }
                else if (currentDate.Date.Month == Hamza.Date.Month && currentDate.Date.Day == Hamza.Date.Day)
                {
                    await botClient.SendTextMessageAsync(-1001729685476, "ХАМЗИКА С ДНЕМ ПОЯВЛЕНИЯ!!!!!!!!!!!!!");
                }
                else if (currentDate.Date.Month == Vova.Date.Month && currentDate.Date.Day == Vova.Date.Day)
                {
                    await botClient.SendTextMessageAsync(-1001729685476, "У ВОВАНЧИКА ДР!!!!!!!!!!!!!!!!!!");
                }
                else if (currentDate.Date.Month == Zhanik.Date.Month && currentDate.Date.Day == Zhanik.Date.Day)
                {
                    await botClient.SendTextMessageAsync(-1001729685476, "ЖАНИК С ДРШКОЙ, МЫ ТЕБЯ ЛЮБИМ!!!!!!!!!!!!!!!!!!!!");
                }
                else if (currentDate.Date.Month == Duman.Date.Month && currentDate.Date.Day == Duman.Date.Day)
                {
                    await botClient.SendTextMessageAsync(-1001729685476, "ДУМАНЧИК, С ДНЕМ РОЖДЕНИЯ!!!!!!!!!!!!!!!");
                }
                else if (currentDate.Date.Month == Nurba.Date.Month && currentDate.Date.Day == Nurba.Date.Day)
                {
                    await botClient.SendTextMessageAsync(-1001729685476, "НУРБОБА, С ДРШКОЙ РОДНОЙ!!!!!!!!!!!!!!!!!!!");
                }
                else if (currentDate.Date.Month == Raha.Date.Month && currentDate.Date.Day == Raha.Date.Day)
                {
                    await botClient.SendTextMessageAsync(-1001729685476, "РАХА, С ДНЕМ РОЖДЕНИЯ!!!!!!!!!!!!!!!!!!!!!");
                }
                else if (currentDate.Date.Month == Dima.Date.Month && currentDate.Date.Day == Dima.Date.Day)
                {
                    await botClient.SendTextMessageAsync(-1001729685476, "С ДНЕМ РОЖДЕНИЯ ДИМООООООООООООООООООН!!!!!!!!!!!!!!!!!");
                }
                else if (currentDate.Date.Month == Era.Date.Month && currentDate.Date.Day == Era.Date.Day)
                {
                    await botClient.SendTextMessageAsync(-1001729685476, "ЕРА, ТЫ СТАЛ СТАРЕЕ!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                }
                else if (currentDate.Date.Month == Artem.Date.Month && currentDate.Date.Day == Artem.Date.Day)
                {
                    await botClient.SendTextMessageAsync(-1001729685476, "СДР тёма");
                }
                timerExpired = false;
                Timer timer = new Timer(TimerCallback, null, TimeSpan.FromDays(1), TimeSpan.FromMilliseconds(-1));
            }
        }
        static void TimerCallback(object state)
        {
            timerExpired = true;
        }

        static public void RestartBot()
        {
            try
            {
                // Get the current process
                var currentProcess = Process.GetCurrentProcess();

                // Get the executable file path
                string exePath = currentProcess.MainModule.FileName;

                // Create a new process start info object
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = exePath,
                    UseShellExecute = false,
                    WorkingDirectory = Path.GetDirectoryName(exePath)
                };

                // Start the new process
                Process.Start(startInfo);

                // Close the current process
                currentProcess.CloseMainWindow();
                currentProcess.Close();
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during the restart process
                Console.WriteLine($"Error restarting the bot: {ex.Message}");
            }
        }
    }
}