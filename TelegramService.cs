using Telegram.Bot;

namespace BotHardware;

public class TelegramService
{
    private readonly ITelegramBotClient _botClient;

    public TelegramService(string botToken)
    {
        // Instanciamos o motor do Telegram
        _botClient = new TelegramBotClient(botToken);
    }

    public async Task EnviarMensagemAsync(string chatId, string mensagem)
    {
        // Método que realmente envia o texto para o seu canal
        await _botClient.SendMessage(chatId, mensagem);
        Console.WriteLine("✅ Mensagem enviada para o Telegram!");
    }
}