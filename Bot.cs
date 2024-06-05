using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using CyberPupsekBot.Functions;
using CyberPupsekBot.Commands;
using CyberPupsekBot.Games;
using System.Text;
using Telegram.Bot.Types.Enums;

namespace Bot
{
    class MainBot
    {
        internal static ITelegramBotClient bot = new TelegramBotClient("6038220012:AAFtvZFgGTRDsYTPX5wS7FAu4ANWGORIk4Y");
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
                if (update.Type == Telegram.Bot.Types.Enums.UpdateType.CallbackQuery)
                {
                    Roulette.gameList = Roulette.ReadFile();
                    if (update.CallbackQuery.Data == ("shoot_rgame"))
                    { await Roulette.TryShooting(Roulette.gameList.First(gm => update.CallbackQuery.Message.Chat.Id == gm.GameId), update, cancellationToken); }

                    else if (update.CallbackQuery.Data == ("join_rgame"))
                    { await Roulette.UpdateGameEventJoin(Roulette.gameList.First(gm => update.CallbackQuery.Message.Chat.Id == gm.GameId), update); }

                    else if (update.CallbackQuery.Data == "cancel_rgame")
                    { await Roulette.UpdateGameEventCancel(Roulette.gameList.First(gm => update.CallbackQuery.Message.Chat.Id == gm.GameId), update); }

                    else if (update.CallbackQuery.Data == "start_rgame")
                    { await Roulette.UpdateGameEventStart(Roulette.gameList.First(gm => update.CallbackQuery.Message.Chat.Id == gm.GameId), update, cancellationToken); }

                    Console.WriteLine("From: " + update.CallbackQuery.From.Username + " \n Data: " + update.CallbackQuery.Data + " \n Type: " + update.Type + " \n ID: " + update.CallbackQuery.From.Id + "\n ------------------------------------------------------------------------------------------------------");
                }
                else if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
                {
                    if (update.Message.MigrateToChatId != 0)
                    {
                        if (update.Message.Text is not null)
                        {
                            if (update.Message.From.Username == "Gekkooni" && update.Message.Text == "@TurnOff_2111")
                            { Environment.Exit(0); }
                            await CoinData.WriteToList(new CoinData(update.Message.From.Id, 0));

                            if (update.Message.Text.StartsWith('/'))
                            {
                                CallUsernames cu = new CallUsernames(bot, update);
                                Randomizer rm = new Randomizer(bot, update);
                                TextExtensionCommands tec = new TextExtensionCommands(botClient, update);

                                await CoinData.GetAmount(update, bot);
                                await HelpCommand.Send(botClient, update);
                                await Roulette.Start(update, cancellationToken);

                                await AnonymousMessage.Send(botClient, update);
                                await tec.Process();
                                await cu.Process();
                                await rm.Process();
                                await Admin.AdminMessage(bot, update);
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
            await Console.Out.WriteLineAsync(sb.Append(exception), cancellationToken);
            Console.ReadLine();
        }

        static void Main()
        {
            Console.WriteLine("Запущен " + bot.GetMeAsync().Result.FirstName + " v 0.5.1 - Independence - partial code fix");

            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = new UpdateType[]
                {
                    UpdateType.Message,       
                    UpdateType.CallbackQuery
                }
            };
            bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );
            Console.ReadLine();
        }
    }
}