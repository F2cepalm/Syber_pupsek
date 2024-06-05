using Telegram.Bot;
using Telegram.Bot.Types;

namespace CyberPupsekBot.Functions
{
    internal class HelpCommand
    {
        async public static Task Send(ITelegramBotClient bot, Update update)
        {
            if(update.Message.Text.StartsWith("/help"))
            {
                string helpMessage = "Помощь по командам: \n" +
                                     "/kick [тэг пользователя или слово]" + " - Дополнение команды: [Ваш ник] ударил по жопе [тэг или слово]" + "\n" +
                                     "/an [сообщение]" + " - Отправка анонимного сообщения в чат" + "\n\n" +
                                     "/rgame" + " - Запускает игры бота" + "\n" +
                                     "/mycoins" + " - Показывает количество ваших монет" + "\n\n" +
                                     "/number" + " - Возвращает случайное число от 1 до 100" + "\n" +
                                     "/roll [вопрос]" + " - Дает ответ на вопрос ( да или нет )" + "\n\n" +
                                     "/all" + " - Перечисляет тэги добавленных участников группы" + "\n" +
                                     "/callme" + " - Добавляет тег в список перечисления команды /all" + "\n" +
                                     "/nocall" + " - Если ваш тег уже добавлен в список, то команда удалит его" + "\n\n";

                try
                {
                    await bot.SendTextMessageAsync(update.Message.From.Id, helpMessage);
                }
                catch
                {
                    await bot.SendTextMessageAsync(update.Message.Chat.Id, helpMessage);
                }
            }
        }
    }
}
