
using BotHardware;
using Telegram.Bot;
using Microsoft.Extensions.Configuration; // Adicione este using

// 1. CARREGANDO O ARQUIVO DE CONFIGURAÇÃO
var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

// Buscando os dados do JSON com segurança
var tokenAdmitad = config["AdmitadConfig:AccessToken"];
var tokenTelegram = config["TelegramConfig:BotToken"];
var idCanalTelegram = config["TelegramConfig:ChatId"];

// 2. INSTANCIAÇÃO (O resto do código continua igual!)
var cliente = new AdmitadClient(tokenAdmitad!);
var telegram = new TelegramService(tokenTelegram!);

Console.WriteLine("--- Monitor de Status Resiliente Ativado ---");

while (true)
{
    try 
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"\n[ {DateTime.Now:HH:mm:ss} ] Verificando Admitad...");
        Console.ResetColor();

        var dados = await cliente.GetWebsitesParsedAsync();

        if (dados?.results != null)
        {
            foreach (var canal in dados.results)
            {
                if (canal.name != null && canal.name.Contains("achadinhos", StringComparison.OrdinalIgnoreCase))
                {
                    if (canal.validation_passed)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("🎉 APROVAÇÃO DETECTADA!");
                        Console.ResetColor();

                        string msgSucesso = $"✅ URGENTE: O canal {canal.name} foi APROVADO! Agora podemos gerar links.";
                        await telegram.EnviarMensagemAsync(idCanalTelegram, msgSucesso);
                        
                        return; // Objetivo alcançado, encerra o bot.
                    }
                    else
                    {
                        Console.WriteLine($"Status: {canal.name} ainda em análise...");
                    }
                }
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("⚠️ Resposta vazia da Admitad. Verificando novamente na próxima hora.");
            Console.ResetColor();
        }

        // Se chegou aqui sem erros, espera 1 hora
        await Task.Delay(TimeSpan.FromHours(1));
    }
    catch (Exception ex)
    {
        // O "Coração" do Try-Catch: Se qualquer erro ocorrer lá em cima, ele cai aqui.
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\n[ {DateTime.Now:HH:mm:ss} ] ⚠️ ERRO DETECTADO:");
        Console.WriteLine($"Mensagem: {ex.Message}");
        Console.ResetColor();

        Console.WriteLine("O bot não será encerrado. Tentando novamente em 10 minutos...");
        
        // Espera um tempo menor (10 min) para tentar se recuperar da falha de rede
        await Task.Delay(TimeSpan.FromMinutes(10));
    }
}