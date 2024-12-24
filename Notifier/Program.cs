using EasyNetQ;
using Microsoft.AspNetCore.SignalR.Client;
using Notifier.Models;

var rabbitMqConnectionString = "amqp://guest:guest@localhost:5672";
var signalRHubUrl = "https://localhost:7049/paymentHub";

// Подключаемся к SignalR
var connection = new HubConnectionBuilder()
    .WithUrl(signalRHubUrl)
    .Build();

try
{
    await connection.StartAsync();
    Console.WriteLine("[✓] Подключено к SignalR.");
}
catch (Exception ex)
{
    Console.WriteLine($"[!] Ошибка при подключении к SignalR: {ex.Message}");
    return;
}

// Подключаемся к RabbitMQ
using var bus = RabbitHutch.CreateBus(rabbitMqConnectionString);
Console.WriteLine("[✓] Подключено к RabbitMQ.");

// Слушаем сообщения о платеже
bus.PubSub.Subscribe<PaymentNotificationMessage>("PaymentNotifier", async message =>
{
    Console.WriteLine($"[x] Получено сообщение о платеже: UserId={message.UserId}, Amount={message.Amount}, Status={message.Status}");

    // Отправляем начальное уведомление через SignalR
    await connection.InvokeAsync("SendPaymentNotification", message);
    Console.WriteLine("[✓] Начальное уведомление отправлено через SignalR.");

    // Задержка перед обновлением статуса
    await Task.Delay(5000);

    // Имитация случайного результата
    var random = new Random();
    var isFailed = random.Next(1, 101) <= 20; // 20% шанс на отказ

    message.Status = isFailed ? "Failed" : "Success";
    message.Message = isFailed ? "Платеж не удалось обработать" : "Платеж успешно обработан";

    // Отправляем обновленное уведомление через SignalR
    await connection.InvokeAsync("SendPaymentNotification", message);
    Console.WriteLine($"[✓] Обновленное уведомление отправлено со статусом: {message.Status}");
});

Console.WriteLine("Нажмите [Enter] для выхода.");
Console.ReadLine();
