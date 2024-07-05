using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace CyberPupsekBot.Functions
{
    internal class HelpCommand
    {
        static string helpMessage = "Помощь по командам: \n" +
                     "/kick [тэг пользователя или слово]" + " - Дополнение команды: [Ваш ник] ударил по жопе [тэг или слово]" + "\n" +
                     "/an [сообщение]" + " - Отправка анонимного сообщения в чат" + "\n\n" +
                     "/rgame" + " - Запускает игры бота" + "\n" +
                     "/mycoins" + " - Показывает количество ваших монет" + "\n\n" +
                     "/number" + " - Возвращает случайное число от 1 до 100" + "\n" +
                     "/roll [вопрос]" + " - Дает ответ на вопрос ( да или нет )" + "\n\n" +
                     "/all" + " - Перечисляет тэги добавленных участников группы" + "\n" +
                     "/callme" + " - Добавляет тег в список перечисления команды /all" + "\n" +
                     "/nocall" + " - Если ваш тег уже добавлен в список, то команда удалит его" + "\n\n";
        async public static Task Send(ITelegramBotClient bot, Update update)
        {
            if(update.Message.Text.StartsWith("/help"))
            {
                try
                {
                    await bot.SendTextMessageAsync(update.Message.From.Id, helpMessage);
                }
                catch(ApiRequestException)
                {
                    if (update.Message.From.Username != null)
                        await bot.SendTextMessageAsync(update.Message.Chat.Id, $"Начните диалог с ботом в личных сообщениях для корректной работы. \n\t@syber_pupsek_bot");
                    await bot.SendTextMessageAsync(update.Message.Chat.Id, helpMessage);
                }
            }
        }

        async public Task Start(ITelegramBotClient bot, Update update)
        {
            if(update.Message.Text.StartsWith("/start"))
            {
                try
                {
                    await bot.SendTextMessageAsync(update.Message.From.Id, helpMessage);
                }
                catch(ApiRequestException e)
                {
                    if(update.Message.From.Username != null)
                        await bot.SendTextMessageAsync(update.Message.Chat.Id, $"Начните диалог с ботом в личных сообщениях для корректной работы. \n\t@syber_pupsek_bot");
                    await bot.SendTextMessageAsync(update.Message.Chat.Id, helpMessage);
                }
            }
        }
    }
}
