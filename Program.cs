using System.Diagnostics;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

var client = new TelegramBotClient("6935253966:AAGkvzlnTjx9z7qyd7Qd3RsT9vLZ1EDYIK8");

//
bool CheckPrice1 = false;
bool CheckPrice2 = false;
//

client.StartReceiving(Update, Error);

Console.ReadLine();

async Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
{
    ReplyKeyboardMarkup replyKeyboardMainMenu = new(new[]
    {
    new KeyboardButton[] { "Рассчитать стоимость 📲", "Отзывы 🙏" },
    new  KeyboardButton[] { "Скачать POIZON 📱" },
    new  KeyboardButton[] { "Связаться с админом ✅" }
    })
    {
        ResizeKeyboard = true
    };

    ReplyKeyboardMarkup replyKeyboardPrice = new(new[]
   {
    new KeyboardButton[] { "Обувь/Верхняя одежда 👟"},
    new  KeyboardButton[] { "Зимняя/Тяжелая обувь 🥾" },
    new  KeyboardButton[] { "Толстовки/Штаны 👘"},
    new  KeyboardButton[] { "Футболка/Шорты 👕"},
    new  KeyboardButton[] { "Носки/Нижнее белье 🧦"},
    new  KeyboardButton[] { "В главное меню"}
    })
    {
        ResizeKeyboard = true
    };

    try
    {
        var message = update.Message;

        if (message.Text != null)
        {
            try
            {
                Console.WriteLine($"{message.Chat.FirstName}  |  {message.Text}");

                if (message.Text.ToLower() == "/start")
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Здравствуйте! Ознакомьтесь с меню", replyMarkup: replyKeyboardMainMenu);
                    CheckPrice1 = false;
                    CheckPrice2 = false;
                    return;
                }

                if (message.Text.ToLower() == "в главное меню")
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Выберите:", replyMarkup: replyKeyboardMainMenu);
                    CheckPrice1 = false;
                    CheckPrice2 = false;
                    return;
                }

                #region Price
                if (message.Text.ToLower().Contains("рассчитать стоимость"))
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, $"{message.Chat.FirstName} ,выберите категорию товаров, если нет вашего товара, пишите для расчета админу.", replyMarkup: replyKeyboardPrice);

                    return;
                }

                if (message.Text.ToLower().Contains("обувь/верхняя одежда") || message.Text.ToLower().Contains("толстовки/штаны") || message.Text.ToLower().Contains("футболка/шорты") || message.Text.ToLower().Contains("носки/нижнее белье"))
                {
                    try
                    {
                        await botClient.SendPhotoAsync(update.Message.Chat.Id, InputFile.FromUri("https://i.postimg.cc/VLhjfK3G/IMG-20240314-112144.jpg"), caption: "Введите стоимость или заказа в юанях из зеленой кнопки.\n Без учета скидки, так как скидки у всех индивидуальны!");

                        CheckPrice1 = true;

                        return;
                    }
                    catch 
                    {
                        await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Введите стоимость или заказа в юанях из зеленой кнопки.\n Без учета скидки, так как скидки у всех индивидуальны!");

                        CheckPrice1 = true;

                        return;
                    }
                }
                if (message.Text.ToLower().Contains("зимняя/тяжелая обувь"))
                {

                    try
                    {
                        await botClient.SendPhotoAsync(update.Message.Chat.Id, InputFile.FromUri("https://i.postimg.cc/VLhjfK3G/IMG-20240314-112144.jpg"), caption: "Введите стоимость или заказа в юанях из зеленой кнопки.\n Без учета скидки, так как скидки у всех индивидуальны!");

                        CheckPrice2 = true;

                        return;
                    }
                    catch
                    {
                        await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Введите стоимость или заказа в юанях из зеленой кнопки.\n Без учета скидки, так как скидки у всех индивидуальны!");

                        CheckPrice2 = true;

                        return;
                    }
                }

                if (update.Type == UpdateType.Message && update.Message != null && update.Message.Text != null)
                {

                    if (CheckPrice1 || CheckPrice2)
                    {
                        var message1 = update.Message.Text;
                        var price = Convert.ToDouble(message1);

                        try
                        {
                            // Определяем коэффициент в зависимости от значения переменной price
                            double coefficient;
                            if (price < 500)
                            {
                                coefficient = CheckPrice1 ? 25 : 34;
                            }
                            else if (price < 1000)
                            {
                                coefficient = CheckPrice1 ? 16.2 : 18;
                            }
                            else
                            {
                                coefficient = CheckPrice1 ? 15.3 : 16.2;
                            }

                            price *= coefficient; // Умножаем стоимость на коэффициент

                            CheckPrice1 = false;
                            CheckPrice2 = false;

                            await botClient.SendTextMessageAsync(update.Message.Chat.Id, $"Стоимость доставки будет {Math.Round(price, 1)}", replyMarkup: replyKeyboardMainMenu);
                        }
                        catch
                        {
                            await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Введите стоимость вашего заказа", replyMarkup: replyKeyboardMainMenu);
                        }
                    }
                }

                #endregion

                if (message.Text.ToLower().Contains("отзывы"))
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Тут будет ссылка на отзывы ...", replyMarkup: replyKeyboardMainMenu);
                    return;
                }

                if (message.Text.ToLower().Contains("связаться с админом"))
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "[@Heyrano](tg://user?id=713706717) или [@bugzis](tg://user?id=808426527)", replyMarkup: replyKeyboardMainMenu, parseMode: ParseMode.MarkdownV2);
                    return;
                }

                if (message.Text.ToLower().Contains("скачать poizon"))
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Выберите ваше платформу и перейдите по ссылке для загрузки \n <a href=\"https://www.anxinapk.com/rj/12201303.html\">Android</a> или <a href=\"https://apps.apple.com/us/app/得物-得到运动x潮流x好物/id1012871328\">iPhone</a>", replyMarkup: replyKeyboardMainMenu, parseMode: ParseMode.Html, disableWebPagePreview: true);
                    return;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }


    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.ToString());
    }
}

async Task Error(ITelegramBotClient client, Exception exception, CancellationToken token)
{
    throw new NotImplementedException();
}

